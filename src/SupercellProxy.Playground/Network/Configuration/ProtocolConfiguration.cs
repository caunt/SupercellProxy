namespace SupercellProxy.Playground.Network.Configuration;

/// <summary>
/// Defines the <c language="csharp">ProtocolConfiguration</c> settings.
/// </summary>
internal sealed record ProtocolConfiguration(
    int MajorVersion,
    int MinorVersion,
    int PatchVersion,
    int ProtocolVersion,
    int KeyVersion
)
{
    /// Gets the protocol configuration for the current native client version.
    public static ProtocolConfiguration Current { get; } = new(1, 72, 86, 3, 43);
}
