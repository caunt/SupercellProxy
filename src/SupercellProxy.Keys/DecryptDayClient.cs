using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using SupercellProxy.Keys.DecryptDay;

using CloakBrowser;

using Microsoft.Playwright;

using SupercellProxy.Keys.Models;

namespace SupercellProxy.Keys;

internal sealed class DecryptDayClient(HttpClient client)
{
    private const string ApplicationProgrammingInterfaceUserAgent = "PlayCover/3.0 CFNetwork/1494.0.7 Darwin/23.4.0";

    private const int MaximumBrowserAttempts = 3;
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

        for (int attempt = 1; attempt <= MaximumBrowserAttempts; attempt++)
        {
            CloakBrowserHandle browser = await CloakLauncher
                .LaunchAsync(CreateBrowserLaunchOptions())
                .ConfigureAwait(continueOnCapturedContext: false);

            await using (browser.ConfigureAwait(continueOnCapturedContext: false))
            {
                IPage page = await browser.NewPageAsync().ConfigureAwait(continueOnCapturedContext: false);

                try
                {
                    await DownloadWithBrowserAsync(page, download, destination, cancellationToken)
                        .ConfigureAwait(continueOnCapturedContext: false);

                    return;
                }
                catch (TimeoutException exception)
                {
                    string title = await page.TitleAsync().ConfigureAwait(continueOnCapturedContext: false);
                    string location = page.Url;

                    if (attempt == MaximumBrowserAttempts)
                    {
                        (string screenshotPath, string markupPath) = await SaveDiagnosticsAsync(page, cancellationToken)
                            .ConfigureAwait(continueOnCapturedContext: false);

                        throw new InvalidOperationException(
                            "decrypt.day did not reach a usable download page after "
                                + $"{MaximumBrowserAttempts} fresh browser identities. Final page: "
                                + $"\"{title}\" ({location}). Diagnostics saved to "
                                + $"{screenshotPath} and {markupPath}.",
                            exception
                        );
                    }

                    await Console
                        .Error.WriteLineAsync(
                            "decrypt.day did not show its download controls for browser identity "
                                + string.Create(CultureInfo.InvariantCulture, $"{attempt}/{MaximumBrowserAttempts}: \"{title}\" ({location}). ")
                                + "Relaunching with a fresh identity..."
                        )
                        .ConfigureAwait(continueOnCapturedContext: false);
                }
            }
        }

        throw new UnreachableException();
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

    private static async Task<ILocator> CompleteVerificationAsync(IPage page)
    {
        ILocator verificationButton = page.Locator(selector: "button.btn-download")
            .Filter(new LocatorFilterOptions { HasText = "Get download link" });

        await verificationButton
            .WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 60_000 })
            .ConfigureAwait(continueOnCapturedContext: false);
        await Console
            .Error.WriteLineAsync(value: "Running Turnstile verification...")
            .ConfigureAwait(continueOnCapturedContext: false);
        await Assertions
            .Expect(verificationButton)
            .ToBeEnabledAsync(new LocatorAssertionsToBeEnabledOptions { Timeout = 60_000 })
            .ConfigureAwait(continueOnCapturedContext: false);
        await verificationButton
            .ClickAsync(new LocatorClickOptions { Timeout = 30_000 })
            .ConfigureAwait(continueOnCapturedContext: false);

        ILocator downloadButton = page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Download", Exact = true });

        ILocator formError = page.Locator(selector: ".form-error");
        await downloadButton
            .Or(formError)
            .First.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 90_000 })
            .ConfigureAwait(continueOnCapturedContext: false);

        if (await formError.IsVisibleAsync().ConfigureAwait(continueOnCapturedContext: false))
        {
            string message = (await formError.InnerTextAsync().ConfigureAwait(continueOnCapturedContext: false)).Trim();

            throw new InvalidOperationException($"decrypt.day rejected the download verification: {message}");
        }

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
        IResponse? navigation = await page.GotoAsync(download.Address.AbsoluteUri, new PageGotoOptions { Timeout = 60_000, WaitUntil = WaitUntilState.DOMContentLoaded, })
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
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        if (navigation is { Status: >= 400 })
        {
            await Console.Error.WriteLineAsync($"Download page returned HTTP {navigation.Status}; checking its verification controls.")
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        ILocator downloadButton = await CompleteVerificationAsync(page).ConfigureAwait(continueOnCapturedContext: false);

        await Console
            .Error.WriteLineAsync(value: "Verification complete; starting IPA transfer...")
            .ConfigureAwait(continueOnCapturedContext: false);

        IDownload browserDownload = await page.RunAndWaitForDownloadAsync(
                async () =>
                    await downloadButton
                        .ClickAsync(new LocatorClickOptions { Timeout = 30_000 })
                        .ConfigureAwait(continueOnCapturedContext: false),
                new PageRunAndWaitForDownloadOptions { Timeout = 60_000 }
            )
            .ConfigureAwait(continueOnCapturedContext: false);

        Stream source = await browserDownload.CreateReadStreamAsync().ConfigureAwait(continueOnCapturedContext: false);

        await using (source.ConfigureAwait(continueOnCapturedContext: false))
            await source.CopyToAsync(destination, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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

    private static async Task<(string ScreenshotPath, string HtmlPath)> SaveDiagnosticsAsync(IPage page, CancellationToken cancellationToken)
    {
        string diagnosticBase = Path.Combine(
            Environment.CurrentDirectory,
            string.Create(CultureInfo.InvariantCulture, $"decrypt-day-failure-{DateTime.UtcNow:yyyyMMdd-HHmmss}")
        );

        string screenshotPath = diagnosticBase + ".png";
        string hypertextMarkupPath = diagnosticBase + ".html";

        byte[] screenshot = await page.ScreenshotAsync(new PageScreenshotOptions { FullPage = true })
            .ConfigureAwait(continueOnCapturedContext: false);

        await File.WriteAllBytesAsync(screenshotPath, screenshot, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
        string content = await page.ContentAsync().ConfigureAwait(continueOnCapturedContext: false);
        await File.WriteAllTextAsync(hypertextMarkupPath, content, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return (screenshotPath, hypertextMarkupPath);
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
