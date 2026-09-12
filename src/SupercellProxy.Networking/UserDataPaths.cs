using SupercellProxy.Networking.Sessions;

namespace SupercellProxy.Networking;

/// <summary>Provides the shared per-user persistence paths used by SupercellProxy applications.</summary>
public static class UserDataPaths
{
    private const string ApplicationDirectoryName = "SupercellProxy";
    private const string AssetDirectoryName = "assets";
    private const string CaptureDirectoryName = "captures";

    /// <summary>Gets the shared root directory for persisted SupercellProxy application data.</summary>
    public static string RootDirectoryPath { get; } = CreateRootDirectoryPath();

    /// <summary>Gets the shared root directory for versioned game assets.</summary>
    public static string AssetDirectoryPath { get; } = Path.Combine(RootDirectoryPath, AssetDirectoryName);

    /// <summary>Gets the shared directory for retained proxy captures.</summary>
    public static string CaptureDirectoryPath { get; } = Path.Combine(RootDirectoryPath, CaptureDirectoryName);

    /// <summary>Gets the shared client-session ledger path.</summary>
    public static string SessionLedgerFilePath { get; } = Path.Combine(RootDirectoryPath, ClientSessionLedger.DefaultFileName);

    private static string CreateRootDirectoryPath()
    {
        string localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        return string.IsNullOrWhiteSpace(localApplicationData)
            ? throw new PlatformNotSupportedException(message: "The current platform does not provide a per-user local application-data directory.")
            : Path.Combine(localApplicationData, ApplicationDirectoryName);
    }
}
