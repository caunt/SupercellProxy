namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>
/// Defines the <c language="csharp">ProtocolConfiguration</c> settings.
/// </summary>
public sealed record ProtocolConfiguration(int MajorVersion, int MinorVersion, int PatchVersion, int ProtocolVersion, int KeyVersion)
{
    /// Gets the protocol configuration for the current native client version.
    public static ProtocolConfiguration Current { get; } = new(MajorVersion: 1, MinorVersion: 72, PatchVersion: 86, ProtocolVersion: 3, KeyVersion: 43);
}
