namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>
/// Defines the <c language="csharp">ProtocolConfiguration</c> settings.
/// </summary>
/// <param name="MajorVersion">The first part of the game release, such as 1 in 1.73.81, sent in the hello and packed into the login version.</param>
/// <param name="MinorVersion">The second part of the game release, such as 73 in 1.73.81; it must stay below 1024 to pack correctly.</param>
/// <param name="PatchVersion">The third part of the game release, such as 81 in 1.73.81; it must stay below 1024 to pack correctly.</param>
/// <param name="ProtocolVersion">
/// The connection protocol version sent first in the hello; configurations built from KEYS.md always use <see cref="WireProtocolVersion"/>.
/// </param>
/// <param name="KeyVersion">
/// The server key pair version the release was built with, read from KEYS.md; a release with no listed key version cannot be configured.
/// </param>
public sealed record ProtocolConfiguration(int MajorVersion, int MinorVersion, int PatchVersion, int ProtocolVersion, int KeyVersion)
{
    /// <summary>The supported ClientHello wire protocol, independent of the game release.</summary>
    public const int WireProtocolVersion = 3;

    /// <summary>Gets the packed application version sent in Login, matching ClientHello.</summary>
    /// <value>
    /// <c>MajorVersion &lt;&lt; 20 | MinorVersion &lt;&lt; 10 | PatchVersion</c>, so 1.73.81 is 1123409; minor and patch parts above 1023 would
    /// overlap the next part.
    /// </value>
    public int LoginVersion => (MajorVersion << 20) | (MinorVersion << 10) | PatchVersion;

    /// <summary>Gets the low 16 bits written to encrypted client-message headers.</summary>
    /// <value>
    /// <see cref="LoginVersion"/> truncated to its low 16 bits, which drops the major part and the top of the minor part; the managed client uses
    /// it as the stream's outbound message version.
    /// </value>
    public ushort MessageVersion => unchecked(ushort.CreateTruncating(LoginVersion));
}
