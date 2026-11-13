
namespace SupercellProxy.Networking;

/// <summary>Provides the shared per-user persistence paths used by SupercellProxy applications.</summary>
public static class UserDataPaths
{
    private const string ApplicationDirectoryName = "SupercellProxy";
    private const string AssetDirectoryName = "assets";
    private const string CaptureDirectoryName = "captures";
    private const string OutOfSyncDirectoryName = "out-of-sync";

    /// <summary>Gets the shared root directory for persisted SupercellProxy application data.</summary>
    /// <value>A <c>SupercellProxy</c> folder inside the user's local application-data folder, computed once; the folder is not created here.</value>
    public static string RootDirectoryPath { get; } = CreateRootDirectoryPath();

    /// <summary>Gets the shared root directory for versioned game assets.</summary>
    /// <value>The <c>assets</c> subfolder of <see cref="RootDirectoryPath"/>.</value>
    public static string AssetDirectoryPath { get; } = Path.Combine(RootDirectoryPath, AssetDirectoryName);

    /// <summary>Gets the shared directory for retained proxy captures.</summary>
    /// <value>The <c>captures</c> subfolder of <see cref="RootDirectoryPath"/>.</value>
    public static string CaptureDirectoryPath { get; } = Path.Combine(RootDirectoryPath, CaptureDirectoryName);

    /// <summary>Gets managed connection failure recordings, separate from retained real-device replay captures.</summary>
    /// <value>The <c>out-of-sync</c> subfolder of <see cref="RootDirectoryPath"/>, the default home of the replay windows that managed connections save only when a session fails.</value>
    public static string OutOfSyncDirectoryPath { get; } = Path.Combine(RootDirectoryPath, OutOfSyncDirectoryName);

    private static string CreateRootDirectoryPath()
    {
        string localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        return string.IsNullOrWhiteSpace(localApplicationData)
            ? throw new PlatformNotSupportedException(message: "The current platform does not provide a per-user local application-data directory.")
            : Path.Combine(localApplicationData, ApplicationDirectoryName);
    }
}
