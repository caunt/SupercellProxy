using SupercellProxy.Networking.Sessions;

namespace SupercellProxy.Networking;

internal static class UserDataPaths
{
    private const string ApplicationDirectoryName = "SupercellProxy";
    private const string AssetDirectoryName = "assets";
    private const string CaptureDirectoryName = "captures";

    internal static string RootDirectoryPath { get; } = CreateRootDirectoryPath();
    internal static string AssetDirectoryPath { get; } = Path.Combine(RootDirectoryPath, AssetDirectoryName);
    internal static string CaptureDirectoryPath { get; } = Path.Combine(RootDirectoryPath, CaptureDirectoryName);
    internal static string SessionLedgerFilePath { get; } = Path.Combine(RootDirectoryPath, ClientSessionLedger.DefaultFileName);

    private static string CreateRootDirectoryPath()
    {
        string localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        return string.IsNullOrWhiteSpace(localApplicationData)
            ? throw new PlatformNotSupportedException(message: "The current platform does not provide a per-user local application-data directory.")
            : Path.Combine(localApplicationData, ApplicationDirectoryName);
    }
}
