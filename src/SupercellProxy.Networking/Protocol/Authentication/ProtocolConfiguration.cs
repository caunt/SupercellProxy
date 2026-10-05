namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>
/// Defines the <c language="csharp">ProtocolConfiguration</c> settings.
/// </summary>
public sealed record ProtocolConfiguration(int MajorVersion, int MinorVersion, int PatchVersion, int ProtocolVersion, int KeyVersion)
{
    /// <summary>The supported ClientHello wire protocol, independent of the game release.</summary>
    public const int WireProtocolVersion = 3;

    /// <summary>Gets the packed application version sent in Login, matching ClientHello.</summary>
    public int LoginVersion => (MajorVersion << 20) | (MinorVersion << 10) | PatchVersion;

    /// <summary>Gets the low 16 bits written to encrypted client-message headers.</summary>
    public ushort MessageVersion => unchecked((ushort)LoginVersion);
}
