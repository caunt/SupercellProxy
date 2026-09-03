using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using CloakBrowser;
using Microsoft.Playwright;

namespace SupercellProxy.Keys;

internal sealed class DecryptDayClient(HttpClient client)
{
    private const string ApiUserAgent = "PlayCover/3.0 CFNetwork/1494.0.7 Darwin/23.4.0";
    private readonly HttpClient _client = client;
    private readonly Dictionary<string, DecryptDayAppDetail> _details = new(
        StringComparer.OrdinalIgnoreCase
    );

    public async Task<IpaApp> GetAppAsync(string appStoreId, CancellationToken cancellationToken)
    {
        var detail = await GetDetailAsync(NormalizeAppStoreId(appStoreId), cancellationToken)
            .ConfigureAwait(false);
        return new IpaApp(detail.BundleId, AppVersion.CreateMany(detail.Versions));
    }

    public async Task<IpaDownload?> TryAuthorizeAsync(
        string appStoreId,
        AppVersion version,
        CancellationToken cancellationToken
    )
    {
        var id = NormalizeAppStoreId(appStoreId);
        var detail = await GetDetailAsync(id, cancellationToken).ConfigureAwait(false);
        string? fileId = null;
        foreach (var sourceName in version.SourceNames)
        {
            fileId = await GetFileIdAsync(id, detail.Id, sourceName, cancellationToken)
                .ConfigureAwait(false);
            if (fileId is not null)
                break;
        }

        if (fileId is null)
            return null;

        return new IpaDownload(
            version.Value,
            new Uri($"https://decrypt.day/app/id{id}/dl/{Uri.EscapeDataString(fileId)}")
        );
    }

    private static string NormalizeAppStoreId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        var normalized = value.StartsWith("id", StringComparison.OrdinalIgnoreCase)
            ? value[2..]
            : value;

        if (normalized.Length is 0 || !normalized.All(char.IsAsciiDigit))
            throw new ArgumentException($"Invalid App Store ID: {value}", nameof(value));

        return normalized;
    }

    private async Task<DecryptDayAppDetail> GetDetailAsync(
        string appStoreId,
        CancellationToken cancellationToken
    )
    {
        if (_details.TryGetValue(appStoreId, out var cached))
            return cached;

        using var response = await _client
            .SendWithRetryAsync(() => CreateMetadataRequest(appStoreId), cancellationToken)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var content = await response
            .Content.ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(false);
        JsonNode document;
        await using (content.ConfigureAwait(false))
        {
            document =
                await JsonNode
                    .ParseAsync(content, cancellationToken: cancellationToken)
                    .ConfigureAwait(false)
                ?? throw new InvalidDataException("decrypt.day returned empty app metadata.");
        }

        foreach (var node in document["nodes"]?.AsArray() ?? [])
        {
            if (
                node?["data"] is not JsonArray values
                || SvelteDataDecoder.Decode(values) is not JsonObject root
                || root["app"] is not JsonObject app
            )
            {
                continue;
            }

            var bundleId = GetString(app, "bundle_id");

            if (bundleId is null)
                continue;

            var versions = (root["versions"] as JsonArray ?? [])
                .OfType<JsonObject>()
                .Select(static version => GetString(version, "name"))
                .OfType<string>()
                .Where(static version => !string.IsNullOrWhiteSpace(version))
                .ToArray();
            var id =
                GetString(app, "id")
                ?? throw new InvalidDataException(
                    "decrypt.day metadata omitted its internal app ID."
                );
            var detail = new DecryptDayAppDetail(id, bundleId, versions);

            _details[appStoreId] = detail;
            return detail;
        }

        throw new InvalidDataException("decrypt.day did not return recognizable app metadata.");
    }

    private async Task<string?> GetFileIdAsync(
        string appStoreId,
        string decryptDayId,
        string version,
        CancellationToken cancellationToken
    )
    {
        using var response = await _client
            .SendWithRetryAsync(
                () => CreateFileRequest(appStoreId, decryptDayId, version),
                cancellationToken
            )
            .ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response
                .Content.ReadAsStringAsync(cancellationToken)
                .ConfigureAwait(false);
            throw new HttpRequestException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"decrypt.day file lookup failed with {Convert.ToInt32(response.StatusCode, CultureInfo.InvariantCulture)}: {body}"
                ),
                inner: null,
                response.StatusCode
            );
        }

        var responseContent = await response
            .Content.ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(false);
        JsonObject? envelope;
        await using (responseContent.ConfigureAwait(false))
        {
            envelope =
                await JsonNode
                    .ParseAsync(responseContent, cancellationToken: cancellationToken)
                    .ConfigureAwait(false) as JsonObject;
        }
        var serialized = envelope?["data"]?.GetValue<string>();

        if (
            serialized is null
            || JsonNode.Parse(serialized) is not JsonArray values
            || SvelteDataDecoder.Decode(values) is not JsonObject root
            || root["data"]?["files"] is not JsonArray files
        )
        {
            throw new InvalidDataException("decrypt.day returned an unrecognized file list.");
        }

        return files
            .OfType<JsonObject>()
            .Where(static file =>
                !GetBoolean(file, "premium") && !GetBoolean(file, "login_required")
            )
            .Select(static file => GetString(file, "id"))
            .FirstOrDefault(static id => !string.IsNullOrWhiteSpace(id));
    }

    private static HttpRequestMessage CreateMetadataRequest(string appStoreId)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"https://decrypt.day/app/id{Uri.EscapeDataString(appStoreId)}/__data.json"
        );

        request.Headers.UserAgent.ParseAdd(ApiUserAgent);
        return request;
    }

    private static HttpRequestMessage CreateFileRequest(
        string appStoreId,
        string decryptDayId,
        string version
    )
    {
        var boundary = $"----WebKitFormBoundary{Guid.NewGuid():N}";
        var body =
            $"--{boundary}\r\nContent-Disposition: form-data; name=\"data\"\r\n\r\n"
            + $"{BuildFilePayload(decryptDayId, version)}\r\n--{boundary}--\r\n";
        var content = new ByteArrayContent(Encoding.UTF8.GetBytes(body));

        content.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
        content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("boundary", boundary));

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"https://decrypt.day/app/id{Uri.EscapeDataString(appStoreId)}?/files"
        )
        {
            Content = content,
        };

        request.Headers.UserAgent.ParseAdd(ApiUserAgent);
        request.Headers.Referrer = new Uri($"https://decrypt.day/app/id{appStoreId}");
        request.Headers.TryAddWithoutValidation("Origin", "https://decrypt.day");

        return request;
    }

    private static string BuildFilePayload(string appId, string version)
    {
        var bytes = new List<byte> { 0xA3 };

        foreach (var value in new[] { "appId", appId, "version", version, "isPremier" })
        {
            var encoded = Encoding.UTF8.GetBytes(value);

            if (encoded.Length <= 15)
            {
                bytes.Add(byte.CreateTruncating(0x60 + encoded.Length));
            }
            else
            {
                bytes.Add(0x78);
                bytes.Add(byte.CreateChecked(encoded.Length));
            }

            bytes.AddRange(encoded);
        }

        bytes.Add(0xF7);
        return string.Join(',', bytes);
    }

    private static string? GetString(JsonObject value, string propertyName)
    {
        return value[propertyName] is JsonValue scalar && scalar.TryGetValue<string>(out var result)
            ? result
            : null;
    }

    private static bool GetBoolean(JsonObject value, string propertyName)
    {
        return value[propertyName] is JsonValue scalar
            && scalar.TryGetValue<bool>(out var result)
            && result;
    }

    private const int MaximumBrowserAttempts = 3;

    public static async Task DownloadAsync(
        IpaDownload download,
        Stream destination,
        CancellationToken cancellationToken
    )
    {
        await Console
            .Error.WriteLineAsync("Solving decrypt.day verification in headless CloakBrowser...")
            .ConfigureAwait(false);
        await EnsureMacCloakBrowserAsync(cancellationToken).ConfigureAwait(false);

        await Console
            .Error.WriteLineAsync("Loading decrypt.day download page...")
            .ConfigureAwait(false);

        for (var attempt = 1; attempt <= MaximumBrowserAttempts; attempt++)
        {
            var browser = await CloakLauncher
                .LaunchAsync(CreateBrowserLaunchOptions())
                .ConfigureAwait(false);
            await using (browser.ConfigureAwait(false))
            {
                var page = await browser.NewPageAsync().ConfigureAwait(false);

                try
                {
                    await DownloadWithBrowserAsync(page, download, destination, cancellationToken)
                        .ConfigureAwait(false);
                    return;
                }
                catch (TimeoutException exception)
                {
                    var title = await page.TitleAsync().ConfigureAwait(false);
                    var location = page.Url;

                    if (attempt == MaximumBrowserAttempts)
                    {
                        var diagnostics = await SaveDiagnosticsAsync(page, cancellationToken)
                            .ConfigureAwait(false);

                        throw new InvalidOperationException(
                            "decrypt.day did not reach a usable download page after "
                                + $"{MaximumBrowserAttempts} fresh browser identities. Final page: "
                                + $"\"{title}\" ({location}). Diagnostics saved to "
                                + $"{diagnostics.ScreenshotPath} and {diagnostics.HtmlPath}.",
                            exception
                        );
                    }

                    await Console
                        .Error.WriteLineAsync(
                            "decrypt.day did not show its download controls for browser identity "
                                + string.Create(
                                    CultureInfo.InvariantCulture,
                                    $"{attempt}/{MaximumBrowserAttempts}: \"{title}\" ({location}). "
                                )
                                + "Relaunching with a fresh identity..."
                        )
                        .ConfigureAwait(false);
                }
            }
        }

        throw new UnreachableException();
    }

    private static async Task DownloadWithBrowserAsync(
        IPage page,
        IpaDownload download,
        Stream destination,
        CancellationToken cancellationToken
    )
    {
        await page.GotoAsync(
                download.Url.AbsoluteUri,
                new PageGotoOptions
                {
                    Timeout = 60_000,
                    WaitUntil = WaitUntilState.DOMContentLoaded,
                }
            )
            .ConfigureAwait(false);

        if (!page.Url.Contains("/dl/", StringComparison.Ordinal))
        {
            await Console
                .Error.WriteLineAsync(
                    "decrypt.day initialized the app page; reopening the file in the same browser session..."
                )
                .ConfigureAwait(false);

            await page.GotoAsync(
                    download.Url.AbsoluteUri,
                    new PageGotoOptions
                    {
                        Timeout = 60_000,
                        WaitUntil = WaitUntilState.DOMContentLoaded,
                        Referer = page.Url,
                    }
                )
                .ConfigureAwait(false);
        }

        var downloadButton = await CompleteVerificationAsync(page).ConfigureAwait(false);

        await Console
            .Error.WriteLineAsync("Verification complete; starting IPA transfer...")
            .ConfigureAwait(false);

        var browserDownload = await page.RunAndWaitForDownloadAsync(
                async () =>
                    await downloadButton
                        .ClickAsync(new LocatorClickOptions { Timeout = 30_000 })
                        .ConfigureAwait(false),
                new PageRunAndWaitForDownloadOptions { Timeout = 60_000 }
            )
            .ConfigureAwait(false);
        var source = await browserDownload.CreateReadStreamAsync().ConfigureAwait(false);
        await using (source.ConfigureAwait(false))
        {
            await source.CopyToAsync(destination, cancellationToken).ConfigureAwait(false);
        }
    }

    private static async Task<ILocator> CompleteVerificationAsync(IPage page)
    {
        var verificationButton = page.Locator("button.btn-download")
            .Filter(new LocatorFilterOptions { HasText = "Get download link" });
        await verificationButton
            .WaitForAsync(
                new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 60_000 }
            )
            .ConfigureAwait(false);
        await Console
            .Error.WriteLineAsync("Running Turnstile verification...")
            .ConfigureAwait(false);
        await Assertions
            .Expect(verificationButton)
            .ToBeEnabledAsync(new LocatorAssertionsToBeEnabledOptions { Timeout = 60_000 })
            .ConfigureAwait(false);
        await verificationButton
            .ClickAsync(new LocatorClickOptions { Timeout = 30_000 })
            .ConfigureAwait(false);

        var downloadButton = page.GetByRole(
            AriaRole.Button,
            new PageGetByRoleOptions { Name = "Download", Exact = true }
        );
        var formError = page.Locator(".form-error");
        await downloadButton
            .Or(formError)
            .First.WaitForAsync(
                new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 90_000 }
            )
            .ConfigureAwait(false);
        if (await formError.IsVisibleAsync().ConfigureAwait(false))
        {
            var message = (await formError.InnerTextAsync().ConfigureAwait(false)).Trim();
            throw new InvalidOperationException(
                $"decrypt.day rejected the download verification: {message}"
            );
        }

        return downloadButton;
    }

    private static async Task<(string ScreenshotPath, string HtmlPath)> SaveDiagnosticsAsync(
        IPage page,
        CancellationToken cancellationToken
    )
    {
        var diagnosticBase = Path.Combine(
            Environment.CurrentDirectory,
            string.Create(
                CultureInfo.InvariantCulture,
                $"decrypt-day-failure-{DateTime.UtcNow:yyyyMMdd-HHmmss}"
            )
        );
        var screenshotPath = diagnosticBase + ".png";
        var htmlPath = diagnosticBase + ".html";

        await page.ScreenshotAsync(
                new PageScreenshotOptions { Path = screenshotPath, FullPage = true }
            )
            .ConfigureAwait(false);
        var content = await page.ContentAsync().ConfigureAwait(false);
        await File.WriteAllTextAsync(htmlPath, content, cancellationToken).ConfigureAwait(false);

        return (screenshotPath, htmlPath);
    }

    private static async Task EnsureMacCloakBrowserAsync(CancellationToken cancellationToken)
    {
        if (
            !OperatingSystem.IsMacOS()
            || !string.IsNullOrWhiteSpace(
                Environment.GetEnvironmentVariable("CLOAKBROWSER_BINARY_PATH")
            )
        )
        {
            return;
        }

        var version = Config.GetChromiumVersion();
        var binaryPath = Config.GetBinaryPath(version, pro: false);

        if (File.Exists(binaryPath))
            return;

        var binaryDirectory = Config.GetBinaryDir(version, pro: false);
        var archivePath = Path.Combine(
            Path.GetTempPath(),
            $"cloakbrowser-{Guid.NewGuid():N}.tar.gz"
        );

        try
        {
            await Console
                .Error.WriteLineAsync(
                    "Preparing CloakBrowser Chromium with the macOS system extractor..."
                )
                .ConfigureAwait(false);
            await DownloadBrowserArchiveAsync(version, archivePath, cancellationToken)
                .ConfigureAwait(false);
            Directory.CreateDirectory(binaryDirectory);
            await ExtractBrowserArchiveAsync(archivePath, binaryDirectory, cancellationToken)
                .ConfigureAwait(false);
            if (!File.Exists(binaryPath))
                throw new InvalidOperationException(
                    $"CloakBrowser archive did not contain its expected executable at {binaryPath}."
                );
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

    private static async Task DownloadBrowserArchiveAsync(
        string version,
        string archivePath,
        CancellationToken cancellationToken
    )
    {
        using var downloadClient = new HttpClient { Timeout = TimeSpan.FromMinutes(10) };
        var archive = new FileStream(
            archivePath,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            131_072,
            FileOptions.Asynchronous | FileOptions.SequentialScan
        );
        await using (archive.ConfigureAwait(false))
        {
            using var response = await downloadClient
                .SendWithRetryAsync(
                    () => new HttpRequestMessage(HttpMethod.Get, Config.GetDownloadUrl(version)),
                    cancellationToken
                )
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            await response.Content.CopyToAsync(archive, cancellationToken).ConfigureAwait(false);
        }
    }

    private static async Task ExtractBrowserArchiveAsync(
        string archivePath,
        string binaryDirectory,
        CancellationToken cancellationToken
    )
    {
        using var process =
            Process.Start(
                new ProcessStartInfo
                {
                    FileName = "/usr/bin/tar",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    ArgumentList = { "-xzf", archivePath, "-C", binaryDirectory },
                }
            ) ?? throw new InvalidOperationException("Could not start the macOS tar extractor.");
        var errorTask = process.StandardError.ReadToEndAsync(cancellationToken);
        try
        {
            await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            if (!process.HasExited)
                process.Kill(entireProcessTree: true);
            throw;
        }

        var error = await errorTask.ConfigureAwait(false);
        if (process.ExitCode is not 0)
            throw new InvalidOperationException($"CloakBrowser extraction failed: {error.Trim()}");
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
}
