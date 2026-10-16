using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>
/// Represents the <c language="csharp">ClientHelloMessage</c> protocol message.
/// </summary>
public sealed record ClientHelloMessage : CodecMessage<ClientHelloMessage>, ICodecProvider<ClientHelloMessage>
{
    /// <inheritdoc />
    public static ICodec<ClientHelloMessage> Codec { get; } = Codecs.Composite(
        Codecs.Int32,
        static message => message.ProtocolVersion,
        Codecs.Int32,
        static message => message.KeyVersion,
        Codecs.Int32,
        static message => message.MajorVersion,
        Codecs.Int32,
        static message => message.MinorVersion,
        Codecs.Int32,
        static message => message.PatchVersion,
        Codecs.String,
        static message => message.FingerprintSha1,
        Codecs.Int32,
        static message => message.DeviceType,
        Codecs.Enum<AppStore>(Codecs.Int32),
        static message => message.AppStore,
        Codecs.Int32,
        static message => message.Unknown1,
        static (protocolVersion, keyVersion, majorVersion, minorVersion, patchVersion, fingerprintSha1, deviceType, appStore, unknown1) => new ClientHelloMessage { ProtocolVersion = protocolVersion, KeyVersion = keyVersion, MajorVersion = majorVersion, MinorVersion = minorVersion, PatchVersion = patchVersion, FingerprintSha1 = fingerprintSha1, DeviceType = deviceType, AppStore = appStore, Unknown1 = unknown1 }
    );

    /// <summary>
    /// Store the game was installed from; it decides which billing and store features the server enables.
    /// </summary>
    public required AppStore AppStore { get; set; }

    /// <summary>
    /// Platform number of the device; the managed client sends 2 together with the Google Play store.
    /// </summary>
    public required int DeviceType { get; set; }

    /// <summary>
    /// Hash of the asset bundle the client has. A stale or empty hash makes the server answer the login with an out-of-date-content
    /// failure that carries the current manifest.
    /// </summary>
    public required string FingerprintSha1 { get; set; }

    /// <summary>
    /// Which server key pair the client was built with; the server answers with the matching key, and an unknown version fails.
    /// </summary>
    public required int KeyVersion { get; set; }

    /// <summary>
    /// First part of the game release, such as 1 in 1.73.81; with the minor and patch parts it selects the message ids and layouts.
    /// </summary>
    public required int MajorVersion { get; set; }

    /// <summary>
    /// Second part of the game release, such as 73 in 1.73.81.
    /// </summary>
    public required int MinorVersion { get; set; }

    /// <summary>
    /// Third part of the game release, such as 81 in 1.73.81.
    /// </summary>
    public required int PatchVersion { get; set; }

    /// <summary>
    /// Version of the connection protocol itself, separate from the game release.
    /// </summary>
    public required int ProtocolVersion { get; set; }

    /// <summary>
    /// Last field of the hello; its meaning is not identified. The managed client sends -1.
    /// </summary>
    public int Unknown1 { get; set; }
}
