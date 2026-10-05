using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using SupercellProxy.Keys.DecryptDay;

using CloakBrowser;
using CloakBrowser.Wrappers;

using Microsoft.Playwright;

using SupercellProxy.Keys.Models;

namespace SupercellProxy.Keys;

internal sealed class DecryptDayClient(HttpClient client)
{
    private const string ApplicationProgrammingInterfaceUserAgent = "PlayCover/3.0 CFNetwork/1494.0.7 Darwin/23.4.0";

    private readonly HttpClient _client = client;
    private readonly Dictionary<string, DecryptDayAppDetail> _details = new(StringComparer.OrdinalIgnoreCase);

    public static async Task DownloadAsync(IpaDownload download, Stream destination, CancellationToken cancellationToken)
    {
        await Console
            .Error.WriteLineAsync(value: "Solving decrypt.day verification in headless CloakBrowser...")
            .ConfigureAwait(continueOnCapturedContext: false);
        await EnsureMacCloakBrowserAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        await Console
            .Error.WriteLineAsync(value: "Loading decrypt.day download page...")
            .ConfigureAwait(continueOnCapturedContext: false);

        cancellationToken.ThrowIfCancellationRequested();

        CloakBrowserHandle browser = await CloakLauncher
            .LaunchAsync(CreateBrowserLaunchOptions())
            .ConfigureAwait(continueOnCapturedContext: false);

        await using (browser.ConfigureAwait(continueOnCapturedContext: false))
        {
            IPage page = await browser.NewPageAsync(new BrowserNewPageOptions { AcceptDownloads = true })
                .ConfigureAwait(continueOnCapturedContext: false);

            // DOM snapshots inject scripts and patch browser APIs in every frame, disrupting Turnstile.
            await page.Context.Tracing.StartAsync(new TracingStartOptions { Screenshots = true, Snapshots = false })
                .ConfigureAwait(continueOnCapturedContext: false);

            try
            {
                await DownloadWithBrowserAsync(page, download, destination, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception exception) when (IsDownloadFailure(exception, cancellationToken))
            {
                string diagnostics;

                try
                {
                    string directory = await SaveDiagnosticsAsync(page, exception, cancellationToken)
                        .ConfigureAwait(continueOnCapturedContext: false);

                    diagnostics = $"Diagnostics saved to {directory}.";
                }
                catch (Exception diagnosticException) when (diagnosticException is not OutOfMemoryException)
                {
                    diagnostics = $"Diagnostics could not be saved: {diagnosticException.Message}";
                }

                throw new InvalidOperationException($"{exception.Message} {diagnostics}", exception);
            }
        }
    }

    public async Task<IpaApp> GetAppAsync(string appStoreIdentifier, CancellationToken cancellationToken)
    {
        DecryptDayAppDetail detail = await GetDetailAsync(NormalizeAppStoreIdentifier(appStoreIdentifier), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return new IpaApp(detail.BundleIdentifier, AppVersion.CreateMany(detail.Versions));
    }

    public async Task<IpaDownload?> TryAuthorizeAsync(string appStoreIdentifier, AppVersion version, CancellationToken cancellationToken)
    {
        string identifier = NormalizeAppStoreIdentifier(appStoreIdentifier);
        DecryptDayAppDetail detail = await GetDetailAsync(identifier, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        string? fileIdentifier = null;

        foreach (string sourceName in version.SourceNames)
        {
            fileIdentifier = await GetFileIdentifierAsync(identifier, detail.Identifier, sourceName, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (fileIdentifier is not null)
                break;
        }

        return fileIdentifier is null
            ? null
            : new IpaDownload(version.Value, new Uri($"https://decrypt.day/app/id{identifier}/dl/{Uri.EscapeDataString(fileIdentifier)}"));
    }

    private static string BuildFilePayload(string appIdentifier, string version)
    {
        List<byte> bytes = [0xA3];

        foreach (string? value in new[] { "appId", appIdentifier, "version", version, "isPremier" })
        {
            byte[] encoded = Encoding.UTF8.GetBytes(value);

            if (encoded.Length <= 15)
            {
                bytes.Add(byte.CreateTruncating(0x60 + encoded.Length));
            }
            else
            {
                bytes.Add(item: 0x78);
                bytes.Add(byte.CreateChecked(encoded.Length));
            }

            bytes.AddRange(encoded);
        }

        bytes.Add(item: 0xF7);

        return string.Join(separator: ',', bytes);
    }

    private static async Task ClickDownloadControlAsync(ILocator control, CancellationToken cancellationToken)
    {
        // Run the overlay handlers and actionability checks without sending the actual click.
        await Humanize.Unwrap(control).ClickAsync(new LocatorClickOptions { Trial = true, Timeout = 30_000 })
            .WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        await control.ClickAsync(new LocatorClickOptions { Timeout = 30_000 })
            .WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    private static async Task<ILocator> CompleteVerificationAsync(IPage page, ILocator verificationButton, CancellationToken cancellationToken)
    {
        await Console
            .Error.WriteLineAsync(value: "Running Turnstile verification...")
            .ConfigureAwait(continueOnCapturedContext: false);

        // The Turnstile component renders its errors next to its .turnstile mount, outside .form-error.
        ILocator verificationError = page.Locator(selector: ".form-error:visible, .turnstile-container > div:not(.turnstile):visible").First;
        ILocator enabledButton = verificationButton.And(page.Locator(selector: "button:enabled:visible"));
        await WaitForVerificationControlAsync(enabledButton, verificationError, timeout: 60_000, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
        await ClickDownloadControlAsync(verificationButton, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        ILocator downloadButton = page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Download", Exact = true });
        await WaitForVerificationControlAsync(downloadButton, verificationError, timeout: 90_000, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return downloadButton;
    }

    private static LaunchOptions CreateBrowserLaunchOptions()
    {
        return new LaunchOptions
        {
            Headless = true,
            Humanize = true,
            Locale = "en-US",
        };
    }

    private static HttpRequestMessage CreateFileRequest(string appStoreIdentifier, string decryptDayIdentifier, string version)
    {
        string boundary = $"----WebKitFormBoundary{Guid.NewGuid():N}";

        string body =
            $"--{boundary}\r\nContent-Disposition: form-data; name=\"data\"\r\n\r\n"
            + $"{BuildFilePayload(decryptDayIdentifier, version)}\r\n--{boundary}--\r\n";

        ByteArrayContent content = new(Encoding.UTF8.GetBytes(body));

        content.Headers.ContentType = new MediaTypeHeaderValue(mediaType: "multipart/form-data");
        content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue(name: "boundary", boundary));

        HttpRequestMessage request = new(HttpMethod.Post, $"https://decrypt.day/app/id{Uri.EscapeDataString(appStoreIdentifier)}?/files")
        {
            Content = content,
        };

        request.Headers.UserAgent.ParseAdd(ApplicationProgrammingInterfaceUserAgent);
        request.Headers.Referrer = new Uri($"https://decrypt.day/app/id{appStoreIdentifier}");
        request.Headers.Add(name: "Origin", value: "https://decrypt.day");

        return request;
    }

    private static HttpRequestMessage CreateMetadataRequest(string appStoreIdentifier)
    {
        HttpRequestMessage request = new(HttpMethod.Get, $"https://decrypt.day/app/id{Uri.EscapeDataString(appStoreIdentifier)}/__data.json");

        request.Headers.UserAgent.ParseAdd(ApplicationProgrammingInterfaceUserAgent);

        return request;
    }

    private static async Task DownloadBrowserArchiveAsync(string version, string archivePath, CancellationToken cancellationToken)
    {
        using HttpClient downloadClient = new() { Timeout = TimeSpan.FromMinutes(minutes: 10) };

        FileStream archive = new(
            archivePath,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 131_072,
            FileOptions.Asynchronous | FileOptions.SequentialScan
        );

        await using (archive.ConfigureAwait(continueOnCapturedContext: false))
        {
            using HttpResponseMessage response = await downloadClient
                .SendWithRetryAsync(() => new HttpRequestMessage(HttpMethod.Get, Config.GetDownloadUrl(version)), cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            await response.EnsureSuccessStatusCode().Content.CopyToAsync(archive, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    private static async Task DownloadWithBrowserAsync(IPage page, IpaDownload download, Stream destination, CancellationToken cancellationToken)
    {
        string stage = "opening the download page";

        try
        {
            await HandleDownloadOverlaysAsync(page).ConfigureAwait(continueOnCapturedContext: false);
            await NavigateToDownloadAsync(page, download, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            stage = "waiting for the verification controls";

            ILocator verificationButton = page.Locator(selector: "button.btn-download")
                .Filter(new LocatorFilterOptions { HasText = "Get download link" });

            await verificationButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 60_000 })
                .WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            stage = "completing Turnstile verification";

            ILocator downloadButton = await CompleteVerificationAsync(page, verificationButton, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            await Console
                .Error.WriteLineAsync(value: "Verification complete; starting IPA transfer...")
                .ConfigureAwait(continueOnCapturedContext: false);

            stage = "starting the IPA download";

            IDownload browserDownload = await WaitForBrowserDownloadAsync(page, downloadButton, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            stage = "transferring the IPA";

            Stream source = await browserDownload.CreateReadStreamAsync().WaitAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            await using (source.ConfigureAwait(continueOnCapturedContext: false))
                await source.CopyToAsync(destination, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (Exception exception) when (IsDownloadFailure(exception, cancellationToken))
        {
            throw new InvalidOperationException($"decrypt.day failed while {stage}: {exception.Message}", exception);
        }
    }

    private static async Task EnsureMacCloakBrowserAsync(CancellationToken cancellationToken)
    {
        if (!OperatingSystem.IsMacOS())
            return;

        if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(variable: "CLOAKBROWSER_BINARY_PATH")))
            return;

        string version = Config.GetChromiumVersion();
        string binaryPath = Config.GetBinaryPath(version, pro: false);

        if (File.Exists(binaryPath))
            return;

        string binaryDirectory = Config.GetBinaryDir(version, pro: false);

        string archivePath = Path.Combine(Path.GetTempPath(), $"cloakbrowser-{Guid.NewGuid():N}.tar.gz");

        try
        {
            await Console
                .Error.WriteLineAsync(value: "Preparing CloakBrowser Chromium with the macOS system extractor...")
                .ConfigureAwait(continueOnCapturedContext: false);
            await DownloadBrowserArchiveAsync(version, archivePath, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            new DirectoryInfo(binaryDirectory).Create();
            await ExtractBrowserArchiveAsync(archivePath, binaryDirectory, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (!File.Exists(binaryPath))
                throw new InvalidOperationException($"CloakBrowser archive did not contain its expected executable at {binaryPath}.");

            File.SetUnixFileMode(
                binaryPath,
                UnixFileMode.UserRead
                    | UnixFileMode.UserWrite
                    | UnixFileMode.UserExecute
                    | UnixFileMode.GroupRead
                    | UnixFileMode.GroupExecute
                    | UnixFileMode.OtherRead
                    | UnixFileMode.OtherExecute
            );
        }
        finally
        {
            File.Delete(archivePath);
        }
    }

    private static async Task ExtractBrowserArchiveAsync(string archivePath, string binaryDirectory, CancellationToken cancellationToken)
    {
        using Process process =
            Process.Start(
                new ProcessStartInfo
                {
                    FileName = "/usr/bin/tar",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    ArgumentList = { "-xzf", archivePath, "-C", binaryDirectory },
                }
            ) ?? throw new InvalidOperationException(message: "Could not start the macOS tar extractor.");

        Task<string> errorTask = process.StandardError.ReadToEndAsync(cancellationToken);

        try
        {
            await process.WaitForExitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (OperationCanceledException)
        {
            if (!process.HasExited)
                process.Kill(entireProcessTree: true);

            throw;
        }

        string error = await errorTask.ConfigureAwait(continueOnCapturedContext: false);

        if (process.ExitCode is not 0)
            throw new InvalidOperationException($"CloakBrowser extraction failed: {error.Trim()}");
    }

    private static async Task HandleDownloadOverlaysAsync(IPage page)
    {
        IPage rawPage = Humanize.Unwrap(page);

        ILocator closeAdvertisement = rawPage
            .FrameLocator(selector: "ins.adsbygoogle[data-vignette-loaded='true'] iframe[title='Advertisement']")
            .GetByRole(AriaRole.Button, new FrameLocatorGetByRoleOptions { Name = "Close ad", Exact = true });

        await rawPage.AddLocatorHandlerAsync(
            closeAdvertisement,
            async () =>
            await closeAdvertisement.ClickAsync(new LocatorClickOptions { Timeout = 10_000 })
                .ConfigureAwait(continueOnCapturedContext: false)
        )
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    private static bool IsDownloadFailure(Exception exception, CancellationToken cancellationToken)
    {
        return exception is not OutOfMemoryException
            && !(exception is OperationCanceledException && cancellationToken.IsCancellationRequested);
    }

    private static async Task NavigateToDownloadAsync(IPage page, IpaDownload download, CancellationToken cancellationToken)
    {
        IResponse? navigation = await page.GotoAsync(download.Address.AbsoluteUri, new PageGotoOptions { Timeout = 60_000, WaitUntil = WaitUntilState.DOMContentLoaded, })
            .WaitAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (!page.Url.Contains(value: "/dl/", StringComparison.Ordinal))
        {
            await Console
                .Error.WriteLineAsync(value: "decrypt.day initialized the app page; reopening the file in the same browser session...")
                .ConfigureAwait(continueOnCapturedContext: false);

            navigation = await page.GotoAsync(
                    download.Address.AbsoluteUri,
                    new PageGotoOptions
                    {
                        Timeout = 60_000,
                        WaitUntil = WaitUntilState.DOMContentLoaded,
                        Referer = page.Url,
                    }
                )
                .WaitAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        if (navigation is { Status: >= 400 })
        {
            await Console.Error.WriteLineAsync($"Download page returned HTTP {navigation.Status}; checking its verification controls.")
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    private static string NormalizeAppStoreIdentifier(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        string normalized = value.StartsWith(value: "id", StringComparison.OrdinalIgnoreCase)
            ? value[2..]
            : value;

        return normalized.Length is 0 || !normalized.All(char.IsAsciiDigit)
            ? throw new ArgumentException($"Invalid App Store ID: {value}", nameof(value))
            : normalized;
    }

    private static async Task<string> SaveDiagnosticsAsync(IPage page, Exception exception, CancellationToken cancellationToken)
    {
        string directory = Path.Combine(
            Environment.CurrentDirectory,
            string.Create(CultureInfo.InvariantCulture, $"decrypt-day-failure-{DateTime.UtcNow:yyyyMMdd-HHmmssfff}-{Guid.NewGuid():N}")
        );

        new DirectoryInfo(directory).Create();
        string reportPath = Path.Combine(directory, path2: "error.txt");
        await File.WriteAllTextAsync(reportPath, exception.ToString() + Environment.NewLine, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        IBrowserContext context = Humanize.Unwrap(page.Context);
        await CaptureAsync(
            description: "browser trace",
            () => context.Tracing.StopAsync(new TracingStopOptions { Path = Path.Combine(directory, path2: "trace.zip") })
                .WaitAsync(TimeSpan.FromSeconds(seconds: 15), cancellationToken)
        )
            .ConfigureAwait(continueOnCapturedContext: false);

        IPage[] pages = [.. context.Pages];

        for (int index = 0; index < pages.Length; index++)
        {
            IPage diagnosticPage = pages[index];
            string pageName = string.Create(CultureInfo.InvariantCulture, $"page-{index + 1}");
            await File.AppendAllTextAsync(reportPath, $"{Environment.NewLine}{pageName}: {diagnosticPage.Url}{Environment.NewLine}", cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            await CaptureAsync(
                pageName + " HTML",
                async () =>
            {
                string content = await diagnosticPage.ContentAsync().WaitAsync(TimeSpan.FromSeconds(seconds: 10), cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);

                await File.WriteAllTextAsync(Path.Combine(directory, pageName + ".html"), content, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
            ).ConfigureAwait(continueOnCapturedContext: false);

            await CaptureAsync(
                pageName + " screenshot",
                async () =>
                {
                    byte[] screenshot = await diagnosticPage.ScreenshotAsync(new PageScreenshotOptions { FullPage = true, Timeout = 10_000 })
                        .WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

                    await File.WriteAllBytesAsync(Path.Combine(directory, pageName + ".png"), screenshot, cancellationToken)
                        .ConfigureAwait(continueOnCapturedContext: false);
                }
            ).ConfigureAwait(continueOnCapturedContext: false);
        }

        return directory;

        async Task CaptureAsync(string description, Func<Task> capture)
        {
            try
            {
                await capture().ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception captureException) when (captureException is not OutOfMemoryException)
            {
                await File.AppendAllTextAsync(reportPath, $"Could not capture {description}: {captureException}{Environment.NewLine}", cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
        }
    }

    private static async Task<IDownload> WaitForBrowserDownloadAsync(IPage page, ILocator downloadButton, CancellationToken cancellationToken)
    {
        IBrowserContext context = Humanize.Unwrap(page.Context);
        Uri origin = new(page.Url);
        TaskCompletionSource<IDownload> completion = new(TaskCreationOptions.RunContinuationsAsynchronously);
        TaskCompletionSource<Exception> responseFailure = new(TaskCreationOptions.RunContinuationsAsynchronously);
        ConcurrentDictionary<IPage, byte> observedPages = new();
        int observing = 1;

        EventHandler<IDownload> onDownload = (sender, browserDownload) =>
        {
            if (!completion.TrySetResult(browserDownload))
                return;
        };

        EventHandler<IPage> observePage = (sender, openedPage) =>
        {
            if (Volatile.Read(ref observing) == 0 || !observedPages.TryAdd(openedPage, value: 0))
                return;

            openedPage.Download += onDownload;

            if (Volatile.Read(ref observing) == 0)
                openedPage.Download -= onDownload;
        };

        EventHandler<IResponse> onResponse = (sender, response) =>
        {
            if (!response.Request.IsNavigationRequest || !Uri.TryCreate(response.Url, UriKind.Absolute, out Uri? address))
                return;

            if (address.Host != origin.Host || !address.AbsolutePath.StartsWith(value: "/fs/dl/", StringComparison.Ordinal))
                return;

            Exception? failure = null;

            bool quotaExceeded = response.Headers.TryGetValue(key: "x-quota-exceeded", out string? exceeded)
                && string.Equals(exceeded, b: "true", StringComparison.OrdinalIgnoreCase);

            if (quotaExceeded)
                failure = new InvalidOperationException(message: "decrypt.day reports that this file's download quota is exceeded; no IPA was returned.");
            else if (response.Status >= 400)
                failure = new HttpRequestException($"The download server returned HTTP {response.Status} for {address.GetLeftPart(UriPartial.Path)}.");

            if (failure is not null && !responseFailure.TrySetResult(failure))
                return;
        };

        // decrypt.day opens the file URL in a new tab. Subscribe before clicking, including
        // the opener for downloads that start before Playwright exposes the popup page.
        context.Page += observePage;
        context.Response += onResponse;

        foreach (IPage existingPage in context.Pages)
            observePage(context, existingPage);

        using CancellationTokenSource deadline = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        deadline.CancelAfter(TimeSpan.FromSeconds(seconds: 60));

        try
        {
            await ClickDownloadControlAsync(downloadButton, deadline.Token)
                .ConfigureAwait(continueOnCapturedContext: false);

            Task result = await Task.WhenAny(completion.Task, responseFailure.Task).WaitAsync(deadline.Token)
                .ConfigureAwait(continueOnCapturedContext: false);

            return result == responseFailure.Task
                ? throw await responseFailure.Task.ConfigureAwait(continueOnCapturedContext: false)
                : await completion.Task.ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (OperationCanceledException exception) when (!cancellationToken.IsCancellationRequested)
        {
            string locations = string.Join(separator: ", ", context.Pages.Select(static candidate => candidate.Url));

            throw new TimeoutException($"No download started within 60 seconds of clicking Download. Open pages: {locations}", exception);
        }
        finally
        {
            Volatile.Write(ref observing, value: 0);
            context.Page -= observePage;
            context.Response -= onResponse;

            foreach (IPage observedPage in observedPages.Keys)
                observedPage.Download -= onDownload;
        }
    }

    private static async Task WaitForVerificationControlAsync(ILocator control, ILocator verificationError, float timeout, CancellationToken cancellationToken)
    {
        // Assertions also run overlay handlers while waiting for verification to finish.
        await Assertions.Expect(Humanize.Unwrap(control.Or(verificationError).First))
            .ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = timeout })
            .WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if (await verificationError.IsVisibleAsync().WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
        {
            string message = (await verificationError.InnerTextAsync(new LocatorInnerTextOptions { Timeout = 5_000 })
                .WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false)).Trim();

            throw new InvalidOperationException($"decrypt.day rejected the download verification: {message}");
        }
    }

    private async Task<DecryptDayAppDetail> GetDetailAsync(string appStoreIdentifier, CancellationToken cancellationToken)
    {
        if (_details.TryGetValue(appStoreIdentifier, out DecryptDayAppDetail? cached))
            return cached;

        using HttpResponseMessage response = await _client
            .SendWithRetryAsync(() => CreateMetadataRequest(appStoreIdentifier), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);


        Stream content = await response.EnsureSuccessStatusCode()
            .Content.ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        DecryptDayPageResponse document;

        await using (content.ConfigureAwait(continueOnCapturedContext: false))
        {
            document =
                await JsonSerializer
                    .DeserializeAsync(content, DecryptDaySerializationContext.Default.DecryptDayPageResponse, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false)
                ?? throw new InvalidDataException(message: "decrypt.day returned empty app metadata.");
        }

        foreach (DecryptDayPageNode? node in document.Nodes)
        {
            DecryptDayApplicationPayload? payload = node?.Data?.Decode(DecryptDaySerializationContext.Default.DecryptDayApplicationPayload);

            if (payload?.Application?.BundleIdentifier is not { } bundleIdentifier)
                continue;

            string[] versions = [.. payload.Versions.OfType<DecryptDayVersionMetadata>()
                .Select(static version => version.Name).OfType<string>()
                .Where(static version => !string.IsNullOrWhiteSpace(version))];

            string identifier = payload.Application.Identifier
                ?? throw new InvalidDataException(message: "decrypt.day metadata omitted its internal app ID.");

            DecryptDayAppDetail detail = new(identifier, bundleIdentifier, versions);

            _details[appStoreIdentifier] = detail;

            return detail;
        }

        throw new InvalidDataException(message: "decrypt.day did not return recognizable app metadata.");
    }

    private async Task<string?> GetFileIdentifierAsync(string appStoreIdentifier, string decryptDayIdentifier, string version, CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await _client
            .SendWithRetryAsync(() => CreateFileRequest(appStoreIdentifier, decryptDayIdentifier, version), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (!response.IsSuccessStatusCode)
        {
            string body = await response
                .Content.ReadAsStringAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            throw new HttpRequestException(
                string.Create(CultureInfo.InvariantCulture, $"decrypt.day file lookup failed with {(int)response.StatusCode}: {body}"),
                inner: null,
                response.StatusCode
            );
        }

        Stream responseContent = await response.EnsureSuccessStatusCode()
            .Content.ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        DecryptDayFileEnvelope? envelope;

        await using (responseContent.ConfigureAwait(continueOnCapturedContext: false))
        {
            envelope =
                await JsonSerializer
                    .DeserializeAsync(responseContent, DecryptDaySerializationContext.Default.DecryptDayFileEnvelope, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
        }

        string serialized = envelope?.Data
            ?? throw new InvalidDataException(message: "decrypt.day returned an unrecognized file list.");

        DecryptDayFileMetadata?[] files = JsonSerializer.Deserialize(serialized, DecryptDaySerializationContext.Default.SvelteData)?.Decode(DecryptDaySerializationContext.Default.DecryptDayFilePayload)?.Data?.Files
            ?? throw new InvalidDataException(message: "decrypt.day returned an unrecognized file list.");

        return files.OfType<DecryptDayFileMetadata>()
            .Where(static file => file.Premium is not true && file.LoginRequired is not true)
            .Select(static file => file.Identifier)
            .FirstOrDefault(static identifier => !string.IsNullOrWhiteSpace(identifier));
    }
}
