using System.Globalization;
using System.Text.RegularExpressions;

using SupercellProxy.Keys.AppStore;

namespace SupercellProxy.Keys;

internal static partial class Application
{
    private const string AppStoreIdentifierPattern = @"(?:^|/)id(\d+)(?:/|$)";
    private static readonly HttpClient WebClient = new() { Timeout = TimeSpan.FromMinutes(minutes: 30) };

    [GeneratedRegex(AppStoreIdentifierPattern, RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1_000)]
    private static partial Regex AppStoreIdentifierRegex { get; }

    public static async Task<int> RunAsync(string[] arguments)
    {
        using CancellationTokenSource cancellationTokenSource = new();

        ConsoleCancelEventHandler cancellationHandler = new(
            (unusedParameter0, eventArguments) =>
        {
            eventArguments.Cancel = true;
            cancellationTokenSource.CancelAfter(millisecondsDelay: 0);
        }
        );

        Console.CancelKeyPress += cancellationHandler;

        try
        {
            try
            {
                return arguments.Length is 0 || IsHelp(arguments[0])
                    ? PrintRootHelp()
                    : arguments[0] switch
                    {
                        "download" => await RunDownloadAsync(arguments[1..], cancellationTokenSource.Token)
                            .ConfigureAwait(continueOnCapturedContext: false),
                        "versions" => await RunVersionsAsync(arguments[1..], cancellationTokenSource.Token)
                            .ConfigureAwait(continueOnCapturedContext: false),
                        "search" => await RunSearchAsync(arguments[1..], cancellationTokenSource.Token)
                            .ConfigureAwait(continueOnCapturedContext: false),
                        "games" => await RunGamesAsync(arguments[1..], cancellationTokenSource.Token)
                            .ConfigureAwait(continueOnCapturedContext: false),
                        "update" => await RunUpdateAsync(arguments[1..], cancellationTokenSource.Token)
                            .ConfigureAwait(continueOnCapturedContext: false),
                        _ => throw new ArgumentException($"Unknown command: {arguments[0]}", nameof(arguments)),
                    };
            }
            catch (OperationCanceledException)
                when (cancellationTokenSource.IsCancellationRequested)
            {
                await Console.Error.WriteLineAsync(value: "Operation cancelled.").ConfigureAwait(continueOnCapturedContext: false);

                return 130;
            }
            catch (Exception exception) when (exception is not OutOfMemoryException)
            {
                await Console
                    .Error.WriteLineAsync($"Error: {exception.Message}")
                    .ConfigureAwait(continueOnCapturedContext: false);

                return 1;
            }
        }
        finally
        {
            Console.CancelKeyPress -= cancellationHandler;
        }
    }

    private static bool IsHelp(string value)
    {
        return string.Equals(value, b: "-h", StringComparison.Ordinal)
            || string.Equals(value, b: "--help", StringComparison.Ordinal)
            || string.Equals(value, b: "help", StringComparison.Ordinal);
    }

    private static string Normalize(string value)
    {
        return string.Concat(value.Where(char.IsLetterOrDigit)).ToUpperInvariant();
    }

    private static int Print(string value)
    {
        Console.WriteLine(value);

        return 0;
    }

    private static int PrintCommandHelp(string usage, string description)
    {
        return Print($"{description}{Environment.NewLine}{Environment.NewLine}" + $"Usage:{Environment.NewLine}  SupercellProxy.Keys {usage}");
    }

    private static int PrintDownloadHelp()
    {
        return PrintCommandHelp(usage: "download APP [VERSION] [--output PATH]", description: "Download an IPA (defaults to the newest available version)");
    }

    private static int PrintRootHelp()
    {
        return Print(
            string.Join(
                Environment.NewLine,
                value: ["Search and download decrypted IPAs from decrypt.day.", string.Empty, "Usage:", "  SupercellProxy.Keys download APP [VERSION] [--output PATH]", "  SupercellProxy.Keys versions APP", "  SupercellProxy.Keys search QUERY", "  SupercellProxy.Keys games [FILE] [--json]", "  SupercellProxy.Keys update [FILE] [--app APP_STORE_ID] [--summary PATH]"]
            )
        );
    }

    private static void RequireOneArgument(string[] arguments, string usage)
    {
        if (arguments.Length is not 1 || string.IsNullOrWhiteSpace(arguments[0]))
            throw new ArgumentException($"Usage: SupercellProxy.Keys {usage}", nameof(arguments));
    }

    private static string ResolveAppStoreAddress(Uri address, string value)
    {
        Match match = AppStoreIdentifierRegex.Match(address.AbsolutePath);

        return match.Success
            ? match.Groups[groupnum: 1].Value
            : throw new ArgumentException(message: "Could not find an App Store ID in the URL.", nameof(value));
    }

    private static async Task<string> ResolveAppStoreIdentifierAsync(string value, AppStoreClient appStoreClient, DecryptDayClient decryptDayClient, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        value = value.Trim();

        if (value.All(char.IsAsciiDigit))
            return value;

        if (Uri.TryCreate(value, UriKind.Absolute, out Uri? address))
            return ResolveAppStoreAddress(address, value);

        AppStoreSearchResponse response = await appStoreClient
            .SearchAsync(value, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        string normalizedValue = Normalize(value);

        AppStoreSearchResult[] exactMatches = [.. response
            .Results.Where(
                result =>
                string.Equals(Normalize(result.Name), normalizedValue, StringComparison.Ordinal)
                || result.BundleIdentifier.Equals(value, StringComparison.OrdinalIgnoreCase)
            )];

        foreach (AppStoreSearchResult result in exactMatches.Length > 0 ? exactMatches : response.Results)
        {
            string appStoreIdentifier = result.TrackIdentifier.ToString(CultureInfo.InvariantCulture);

            try
            {

                bool isConditionMet = (
                                        await decryptDayClient
                                            .GetAppAsync(appStoreIdentifier, cancellationToken)
                                            .ConfigureAwait(continueOnCapturedContext: false)
                                    )
                                        .Versions
                                        .Count
                                    is 0;

                if (isConditionMet)
                    continue;

                Console.WriteLine($"Selected search result: {result.Name} ({result.BundleIdentifier}, ID {appStoreIdentifier})");

                return appStoreIdentifier;
            }
            catch (Exception exception)
                when (exception is HttpRequestException or InvalidDataException)
            {
                await Console.Error.WriteLineAsync($"Skipping {result.Name}: {exception.Message}")
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        throw new InvalidOperationException($"No apps found on decrypt.day for \"{value}\".");
    }
}
