namespace SupercellProxy.Playground.Network.Configuration;

/// <summary>
/// Defines the <c>ProtocolConfiguration</c> settings.
/// </summary>
public record ProtocolConfiguration(
    int MajorVersion,
    int MinorVersion,
    int PatchVersion,
    int ProtocolVersion,
    int KeyVersion
);
