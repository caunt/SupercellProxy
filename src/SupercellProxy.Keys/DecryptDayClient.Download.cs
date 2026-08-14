using CloakBrowser;
using Microsoft.Playwright;
using System.Diagnostics;

namespace SupercellProxy.Keys;

internal sealed partial class DecryptDayClient
{
    private const int MaximumBrowserAttempts = 3;

    public async Task DownloadAsync(
        IpaDownload download,
        Stream destination,
        CancellationToken cancellationToken)
    {
        Console.Error.WriteLine("Solving decrypt.day verification in headless CloakBrowser...");
        await EnsureMacCloakBrowserAsync(cancellationToken);

        Console.Error.WriteLine("Loading decrypt.day download page...");

        for (var attempt = 1; attempt <= MaximumBrowserAttempts; attempt++)
        {
            await using var browser = await CloakLauncher.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Humanize = true,
                Locale = "en-US"
            });
            var page = await browser.NewPageAsync();

            try
            {
                await DownloadWithBrowserAsync(page, download, destination, cancellationToken);
                return;
            }
            catch (TimeoutException exception)
            {
                var title = await page.TitleAsync();
                var location = page.Url;

                if (attempt == MaximumBrowserAttempts)
                {
                    var diagnostics = await SaveDiagnosticsAsync(page, cancellationToken);

                    throw new InvalidOperationException(
                        $"decrypt.day did not reach a usable download page after " +
                        $"{MaximumBrowserAttempts} fresh browser identities. Final page: " +
                        $"\"{title}\" ({location}). Diagnostics saved to " +
                        $"{diagnostics.ScreenshotPath} and {diagnostics.HtmlPath}.",
                        exception);
                }

                Console.Error.WriteLine(
                    $"decrypt.day did not show its download controls for browser identity " +
                    $"{attempt}/{MaximumBrowserAttempts}: \"{title}\" ({location}). " +
                    "Relaunching with a fresh identity...");
            }
        }

        throw new UnreachableException();
    }

    private static async Task DownloadWithBrowserAsync(
        IPage page,
        IpaDownload download,
        Stream destination,
        CancellationToken cancellationToken)
    {
        await page.GotoAsync(download.Url.AbsoluteUri, new PageGotoOptions
        {
            Timeout = 60_000,
            WaitUntil = WaitUntilState.DOMContentLoaded
        });

        if (!page.Url.Contains("/dl/", StringComparison.Ordinal))
        {
            Console.Error.WriteLine(
                "decrypt.day initialized the app page; reopening the file in the same browser session...");

            await page.GotoAsync(download.Url.AbsoluteUri, new PageGotoOptions
            {
                Timeout = 60_000,
                WaitUntil = WaitUntilState.DOMContentLoaded,
                Referer = page.Url
            });
        }

        var verificationButton = page.Locator("button.btn-download")
            .Filter(new LocatorFilterOptions { HasText = "Get download link" });

        await verificationButton.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = 60_000
        });

        Console.Error.WriteLine("Running Turnstile verification...");

        await Assertions.Expect(verificationButton).ToBeEnabledAsync(new LocatorAssertionsToBeEnabledOptions
        {
            Timeout = 60_000
        });
        await verificationButton.ClickAsync(new LocatorClickOptions { Timeout = 30_000 });

        var downloadButton = page.GetByRole(AriaRole.Button, new PageGetByRoleOptions
        {
            Name = "Download",
            Exact = true
        });
        var formError = page.Locator(".form-error");

        await downloadButton.Or(formError).First.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = 90_000
        });

        if (await formError.IsVisibleAsync())
        {
            var message = (await formError.InnerTextAsync()).Trim();
            throw new InvalidOperationException(
                $"decrypt.day rejected the download verification: {message}");
        }

        Console.Error.WriteLine("Verification complete; starting IPA transfer...");

        var browserDownload = await page.RunAndWaitForDownloadAsync(
            async () => await downloadButton.ClickAsync(new LocatorClickOptions { Timeout = 30_000 }),
            new PageRunAndWaitForDownloadOptions { Timeout = 60_000 });
        await using var source = await browserDownload.CreateReadStreamAsync();

        await source.CopyToAsync(destination, cancellationToken);
    }

    private static async Task<(string ScreenshotPath, string HtmlPath)> SaveDiagnosticsAsync(
        IPage page,
        CancellationToken cancellationToken)
    {
        var diagnosticBase = Path.Combine(
            Environment.CurrentDirectory,
            $"decrypt-day-failure-{DateTime.UtcNow:yyyyMMdd-HHmmss}");
        var screenshotPath = diagnosticBase + ".png";
        var htmlPath = diagnosticBase + ".html";

        await page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = screenshotPath,
            FullPage = true
        });
        await File.WriteAllTextAsync(
            htmlPath,
            await page.ContentAsync(),
            cancellationToken);

        return (screenshotPath, htmlPath);
    }

    private static async Task EnsureMacCloakBrowserAsync(CancellationToken cancellationToken)
    {
        if (!OperatingSystem.IsMacOS() ||
            !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("CLOAKBROWSER_BINARY_PATH")))
        {
            return;
        }

        var version = Config.GetChromiumVersion();
        var binaryPath = Config.GetBinaryPath(version, false);

        if (File.Exists(binaryPath))
            return;

        var binaryDirectory = Config.GetBinaryDir(version, false);
        var archivePath = Path.Combine(
            Path.GetTempPath(),
            $"cloakbrowser-{Guid.NewGuid():N}.tar.gz");

        try
        {
            Console.Error.WriteLine(
                "Preparing CloakBrowser Chromium with the macOS system extractor...");

            using (var downloadClient = new HttpClient { Timeout = TimeSpan.FromMinutes(10) })
            await using (var archive = new FileStream(
                             archivePath,
                             FileMode.CreateNew,
                             FileAccess.Write,
                             FileShare.None,
                             131_072,
                             FileOptions.Asynchronous | FileOptions.SequentialScan))
            {
                using var response = await downloadClient.SendWithRetryAsync(
                    () => new HttpRequestMessage(HttpMethod.Get, Config.GetDownloadUrl(version)),
                    cancellationToken);

                response.EnsureSuccessStatusCode();
                await response.Content.CopyToAsync(archive, cancellationToken);
            }

            Directory.CreateDirectory(binaryDirectory);

            using var process = Process.Start(new ProcessStartInfo
            {
                FileName = "/usr/bin/tar",
                UseShellExecute = false,
                RedirectStandardError = true,
                ArgumentList = { "-xzf", archivePath, "-C", binaryDirectory }
            }) ?? throw new InvalidOperationException("Could not start the macOS tar extractor.");
            var errorTask = process.StandardError.ReadToEndAsync(cancellationToken);

            try
            {
                await process.WaitForExitAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                if (!process.HasExited)
                    process.Kill(entireProcessTree: true);

                throw;
            }

            var error = await errorTask;

            if (process.ExitCode != 0)
                throw new InvalidOperationException($"CloakBrowser extraction failed: {error.Trim()}");

            if (!File.Exists(binaryPath))
            {
                throw new InvalidOperationException(
                    $"CloakBrowser archive did not contain its expected executable at {binaryPath}.");
            }

            File.SetUnixFileMode(
                binaryPath,
                UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute |
                UnixFileMode.GroupRead | UnixFileMode.GroupExecute |
                UnixFileMode.OtherRead | UnixFileMode.OtherExecute);
        }
        finally
        {
            File.Delete(archivePath);
        }
    }
}
