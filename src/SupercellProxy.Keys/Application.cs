using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using SupercellProxy.PublicKeyExtractor;

namespace SupercellProxy.Keys;

internal static partial class Application
{
    private static readonly string AppStoreIdPattern = @"(?:^|/)id(\d+)(?:/|$)";
    private static readonly HttpClient HttpClient = new() { Timeout = TimeSpan.FromMinutes(30) };

    public static async Task<int> RunAsync(string[] args)
    {
        using var cancellationTokenSource = new CancellationTokenSource();

        ConsoleCancelEventHandler cancellationHandler = (_, eventArgs) =>
        {
            eventArgs.Cancel = true;
            cancellationTokenSource.CancelAfter(0);
        };

        Console.CancelKeyPress += cancellationHandler;

        try
        {
            try
            {
                if (args.Length is 0 || IsHelp(args[0]))
                    return PrintRootHelp();

                return args[0] switch
                {
                    "download" => await RunDownloadAsync(args[1..], cancellationTokenSource.Token)
                        .ConfigureAwait(false),
                    "versions" => await RunVersionsAsync(args[1..], cancellationTokenSource.Token)
                        .ConfigureAwait(false),
                    "search" => await RunSearchAsync(args[1..], cancellationTokenSource.Token)
                        .ConfigureAwait(false),
                    "games" => await RunGamesAsync(args[1..], cancellationTokenSource.Token)
                        .ConfigureAwait(false),
                    "update" => await RunUpdateAsync(args[1..], cancellationTokenSource.Token)
                        .ConfigureAwait(false),
                    _ => throw new ArgumentException($"Unknown command: {args[0]}", nameof(args)),
                };
            }
            catch (OperationCanceledException)
                when (cancellationTokenSource.IsCancellationRequested)
            {
                Console.Error.WriteLine("Operation cancelled.");
                return 130;
            }
            catch (Exception exception) when (exception is not OutOfMemoryException)
            {
                Console.Error.WriteLine($"Error: {exception.Message}");
                return 1;
            }
        }
        finally
        {
            Console.CancelKeyPress -= cancellationHandler;
        }
    }

    private static async Task<string> ResolveAppStoreIdAsync(
        string value,
        AppStoreClient appStoreClient,
        DecryptDayClient decryptDayClient,
        CancellationToken cancellationToken
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        value = value.Trim();

        if (value.All(char.IsAsciiDigit))
            return value;

        if (Uri.TryCreate(value, UriKind.Absolute, out var url))
            return ResolveAppStoreUrl(url, value);

        var response = await appStoreClient
            .SearchAsync(value, cancellationToken)
            .ConfigureAwait(false);
        var normalizedValue = Normalize(value);
        var exactMatches = response
            .Results.Where(result =>
                string.Equals(Normalize(result.Name), normalizedValue, StringComparison.Ordinal)
                || result.BundleId.Equals(value, StringComparison.OrdinalIgnoreCase)
            )
            .ToArray();

        foreach (var result in exactMatches.Length > 0 ? exactMatches : response.Results)
        {
            var appStoreId = result.TrackId.ToString(CultureInfo.InvariantCulture);

            try
            {
                if (
                    (
                        await decryptDayClient
                            .GetAppAsync(appStoreId, cancellationToken)
                            .ConfigureAwait(false)
                    )
                        .Versions
                        .Count
                    is 0
                )
                    continue;

                Console.WriteLine(
                    $"Selected search result: {result.Name} ({result.BundleId}, ID {appStoreId})"
                );
                return appStoreId;
            }
            catch (Exception exception)
                when (exception is HttpRequestException or InvalidDataException)
            {
                // This App Store result is not present in decrypt.day.
            }
        }

        throw new InvalidOperationException($"No apps found on decrypt.day for \"{value}\".");
    }

    private static string ResolveAppStoreUrl(Uri url, string value)
    {
        var match = AppStoreIdRegex.Match(url.AbsolutePath);
        return match.Success
            ? match.Groups[1].Value
            : throw new ArgumentException(
                "Could not find an App Store ID in the URL.",
                nameof(value)
            );
    }

    private static string Normalize(string value)
    {
        return string.Concat(value.Where(char.IsLetterOrDigit)).ToUpperInvariant();
    }

    private static bool IsHelp(string value)
    {
        return string.Equals(value, "-h", StringComparison.Ordinal)
            || string.Equals(value, "--help", StringComparison.Ordinal)
            || string.Equals(value, "help", StringComparison.Ordinal);
    }

    private static void RequireOneArgument(string[] args, string usage)
    {
        if (args.Length is not 1 || string.IsNullOrWhiteSpace(args[0]))
            throw new ArgumentException($"Usage: SupercellProxy.Keys {usage}", nameof(args));
    }

    private static int PrintRootHelp()
    {
        return Print(
            string.Join(
                Environment.NewLine,
                "Search and download decrypted IPAs from decrypt.day.",
                string.Empty,
                "Usage:",
                "  SupercellProxy.Keys download APP [VERSION] [--output PATH]",
                "  SupercellProxy.Keys versions APP",
                "  SupercellProxy.Keys search QUERY",
                "  SupercellProxy.Keys games [FILE] [--json]",
                "  SupercellProxy.Keys update [FILE] [--app APP_STORE_ID] [--summary PATH]"
            )
        );
    }

    private static int PrintDownloadHelp()
    {
        return PrintCommandHelp(
            "download APP [VERSION] [--output PATH]",
            "Download an IPA (defaults to the newest available version)"
        );
    }

    private static int PrintCommandHelp(string usage, string description)
    {
        return Print(
            $"{description}{Environment.NewLine}{Environment.NewLine}"
                + $"Usage:{Environment.NewLine}  SupercellProxy.Keys {usage}"
        );
    }

    private static int Print(string value)
    {
        Console.WriteLine(value);
        return 0;
    }

    private static readonly Regex AppStoreIdRegex = new(
        AppStoreIdPattern,
        RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.Compiled,
        TimeSpan.FromSeconds(1)
    );
}
