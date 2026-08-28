using System.Globalization;
using System.Resources;

namespace SupercellProxy.Playground;

internal static class ApplicationText
{
    private static readonly ResourceManager ResourceManager = new(
        "SupercellProxy.Playground.ApplicationText",
        typeof(ApplicationText).Assembly
    );

    public static string ClientAuthoritativeStateKeepAliveCompleted =>
        GetString(nameof(ClientAuthoritativeStateKeepAliveCompleted));

    public static string ClientConnectionClosed => GetString(nameof(ClientConnectionClosed));

    public static string ClientKeepAliveCompleted => GetString(nameof(ClientKeepAliveCompleted));

    public static string ClientLoadingHarvestState => GetString(nameof(ClientLoadingHarvestState));

    public static string ClientLoggedIn => GetString(nameof(ClientLoggedIn));

    public static string ClientRestartingForHarvestVerification =>
        GetString(nameof(ClientRestartingForHarvestVerification));

    public static string ClientWaitingForAuthoritativeHomeState =>
        GetString(nameof(ClientWaitingForAuthoritativeHomeState));

    public static string ClientWaitingForKeepAlive => GetString(nameof(ClientWaitingForKeepAlive));

    private static string GetString(string name) =>
        ResourceManager.GetString(name, CultureInfo.CurrentUICulture)
        ?? throw new MissingManifestResourceException(
            $"Application text resource '{name}' was not found."
        );
}
