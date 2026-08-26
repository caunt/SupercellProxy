using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using SupercellProxy.PublicKeyExtractor;

namespace SupercellProxy.Keys;

internal static partial class Application
{
    private static async Task<int> RunSearchAsync(
        string[] args,
        CancellationToken cancellationToken
    )
    {
        if (args.Any(IsHelp))
            return PrintCommandHelp("search QUERY", "Find apps with versions on decrypt.day");

        RequireOneArgument(args, "search QUERY");

        var appStoreClient = new AppStoreClient(HttpClient);
        var decryptDayClient = new DecryptDayClient(HttpClient);
        var response = await appStoreClient
            .SearchAsync(args[0], cancellationToken)
            .ConfigureAwait(false);
        var found = 0;

        foreach (var result in response.Results)
        {
            var appStoreId = result.TrackId.ToString(CultureInfo.InvariantCulture);
            IpaApp app;

            try
            {
                app = await decryptDayClient
                    .GetAppAsync(appStoreId, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception exception)
                when (exception is HttpRequestException or InvalidDataException)
            {
                continue;
            }

            if (app.Versions.Count is 0)
                continue;

            Console.WriteLine(
                string.Create(CultureInfo.InvariantCulture, $"{++found}. {result.Name}")
            );

            if (!string.IsNullOrWhiteSpace(result.SellerName))
                Console.WriteLine($"   Developer: {result.SellerName}");

            Console.WriteLine($"   Bundle ID: {app.BundleId}");
            Console.WriteLine($"   App ID: {appStoreId}");
            Console.WriteLine(
                $"   Available: {string.Join(", ", app.Versions.Take(10))}"
                    + (
                        app.Versions.Count > 10
                            ? string.Create(
                                CultureInfo.InvariantCulture,
                                $" (+{app.Versions.Count - 10} more)"
                            )
                            : string.Empty
                    )
            );
        }

        if (found is 0)
            Console.WriteLine($"No apps found on decrypt.day for \"{args[0]}\".");

        return 0;
    }
}
