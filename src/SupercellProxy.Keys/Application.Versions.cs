using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using SupercellProxy.PublicKeyExtractor;

namespace SupercellProxy.Keys;

internal static partial class Application
{
    private static async Task<int> RunVersionsAsync(
        string[] args,
        CancellationToken cancellationToken
    )
    {
        if (args.Any(IsHelp))
            return PrintCommandHelp("versions APP", "List versions available on decrypt.day");

        RequireOneArgument(args, "versions APP");

        var appStoreClient = new AppStoreClient(HttpClient);
        var decryptDayClient = new DecryptDayClient(HttpClient);
        var appStoreId = await ResolveAppStoreIdAsync(
                args[0],
                appStoreClient,
                decryptDayClient,
                cancellationToken
            )
            .ConfigureAwait(false);
        var app = await decryptDayClient
            .GetAppAsync(appStoreId, cancellationToken)
            .ConfigureAwait(false);

        Console.WriteLine($"Bundle ID: {app.BundleId}");
        Console.WriteLine($"App ID: {appStoreId}");
        Console.WriteLine($"Downloadable versions: {app.Versions.Count}");

        foreach (var version in app.Versions.Reverse())
            Console.WriteLine(version);

        return 0;
    }
}
