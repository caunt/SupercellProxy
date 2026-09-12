using System.Globalization;

using SupercellProxy.Keys.AppStore;
using SupercellProxy.Keys.Models;

namespace SupercellProxy.Keys;

internal static partial class Application
{
    private static async Task<int> RunSearchAsync(string[] arguments, CancellationToken cancellationToken)
    {
        if (arguments.Any(IsHelp))
            return PrintCommandHelp(usage: "search QUERY", description: "Find apps with versions on decrypt.day");

        RequireOneArgument(arguments, usage: "search QUERY");

        AppStoreClient appStoreClient = new(WebClient);
        DecryptDayClient decryptDayClient = new(WebClient);

        AppStoreSearchResponse response = await appStoreClient
            .SearchAsync(arguments[0], cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        int found = 0;

        foreach (AppStoreSearchResult result in response.Results)
        {
            string appStoreIdentifier = result.TrackIdentifier.ToString(CultureInfo.InvariantCulture);
            IpaApp app;

            try
            {
                app = await decryptDayClient
                    .GetAppAsync(appStoreIdentifier, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception exception)
                when (exception is HttpRequestException or InvalidDataException)
            {
                continue;
            }

            if (app.Versions.Count is 0)
                continue;

            Console.WriteLine(string.Create(CultureInfo.InvariantCulture, $"{++found}. {result.Name}"));

            if (!string.IsNullOrWhiteSpace(result.SellerName))
                Console.WriteLine($"   Developer: {result.SellerName}");

            Console.WriteLine($"   Bundle ID: {app.BundleIdentifier}");
            Console.WriteLine($"   App ID: {appStoreIdentifier}");
            Console.WriteLine(
                $"   Available: {string.Join(separator: ", ", app.Versions.Take(count: 10))}"
                    + (
                        app.Versions.Count > 10
                            ? string.Create(CultureInfo.InvariantCulture, $" (+{app.Versions.Count - 10} more)")
                            : string.Empty
                    )
            );
        }

        if (found is 0)
            Console.WriteLine($"No apps found on decrypt.day for \"{arguments[0]}\".");

        return 0;
    }
}
