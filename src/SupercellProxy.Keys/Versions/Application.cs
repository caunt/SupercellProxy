using SupercellProxy.Keys.AppStore;
using SupercellProxy.Keys.Models;

namespace SupercellProxy.Keys;

internal static partial class Application
{
    private static async Task<int> RunVersionsAsync(string[] arguments, CancellationToken cancellationToken)
    {
        if (arguments.Any(IsHelp))
            return PrintCommandHelp(usage: "versions APP", description: "List versions available on decrypt.day");

        RequireOneArgument(arguments, usage: "versions APP");

        AppStoreClient appStoreClient = new(WebClient);
        DecryptDayClient decryptDayClient = new(WebClient);

        string appStoreIdentifier = await ResolveAppStoreIdentifierAsync(arguments[0], appStoreClient, decryptDayClient, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        IpaApp app = await decryptDayClient
            .GetAppAsync(appStoreIdentifier, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        Console.WriteLine($"Bundle ID: {app.BundleIdentifier}");
        Console.WriteLine($"App ID: {appStoreIdentifier}");
        Console.WriteLine($"Downloadable versions: {app.Versions.Count}");

        foreach (AppVersion? version in app.Versions.Reverse())
            Console.WriteLine(version.Value);

        return 0;
    }
}
