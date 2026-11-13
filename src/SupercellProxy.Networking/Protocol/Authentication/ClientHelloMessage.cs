using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>
/// Represents the <c language="csharp">ClientHelloMessage</c> protocol message.
/// </summary>
public sealed record ClientHelloMessage : CodecMessage<ClientHelloMessage>, ICodecProvider<ClientHelloMessage>
{
    /// <inheritdoc />
    public static ICodec<ClientHelloMessage> Codec { get; } = Codecs.Composite(Codecs.BigEndianInt32,
        static message => message.ProtocolVersion,
        Codecs.BigEndianInt32,
        static message => message.KeyVersion,
        Codecs.BigEndianInt32,
        static message => message.MajorVersion,
        Codecs.BigEndianInt32,
        static message => message.MinorVersion,
        Codecs.BigEndianInt32,
        static message => message.PatchVersion,
        Codecs.Utf8String,
        static message => message.FingerprintSha1,
        Codecs.BigEndianInt32,
        static message => message.DeviceType,
        Codecs.Enum<AppStore>(Codecs.BigEndianInt32),
        static message => message.AppStore,
        Codecs.BigEndianInt32,
        static message => message.Unknown1,
        static (protocolVersion, keyVersion, majorVersion, minorVersion, patchVersion, fingerprintSha1, deviceType, appStore, unknown1) => new ClientHelloMessage
        {
            ProtocolVersion = protocolVersion,
            KeyVersion = keyVersion,
            MajorVersion = majorVersion,
            MinorVersion = minorVersion,
            PatchVersion = patchVersion,
            FingerprintSha1 = fingerprintSha1,
            DeviceType = deviceType,
            AppStore = appStore,
            Unknown1 = unknown1
        });

    /// <summary>
    /// Gets or sets the store the game was installed from; it decides which billing and store features the server enables.
    /// </summary>
    /// <value>
    /// The store, written as its 32-bit number; the managed client sends <see cref="AppStore.GooglePlay"/>.
    /// </value>
    public required AppStore AppStore { get; set; }

    /// <summary>
    /// Gets or sets the platform number of the device; the managed client sends 2 together with the Google Play store.
    /// </summary>
    /// <value>
    /// A 32-bit platform code; this client only ever sends 2, and the codes of other platforms have not been mapped.
    /// </value>
    public required int DeviceType { get; set; }

    /// <summary>
    /// Gets or sets the hash of the asset bundle the client has. A stale or empty hash makes the server answer the login with an out-of-date-content
    /// failure that carries the current manifest.
    /// </summary>
    /// <value>
    /// The SHA-1 of the asset fingerprint as a hexadecimal string, or an empty string when the client has no assets yet and wants the manifest.
    /// </value>
    public required string FingerprintSha1 { get; set; }

    /// <summary>
    /// Gets or sets which server key pair the client was built with; the server answers with the matching key, and an unknown version fails.
    /// </summary>
    /// <value>
    /// The key version listed for the release in KEYS.md; the managed client takes it from <see cref="ProtocolConfiguration.KeyVersion"/>.
    /// </value>
    public required int KeyVersion { get; set; }

    /// <summary>
    /// Gets or sets the first part of the game release, such as 1 in 1.73.81; with the minor and patch parts it selects the message ids and layouts.
    /// </summary>
    /// <value>
    /// The major release number; the transport records the three parts as the stream's game version as soon as the hello is written.
    /// </value>
    public required int MajorVersion { get; set; }

    /// <summary>
    /// Gets or sets the second part of the game release, such as 73 in 1.73.81.
    /// </summary>
    /// <value>
    /// The minor release number; it must fit in 10 bits because Login packs it into bits 10 to 19 of its version.
    /// </value>
    public required int MinorVersion { get; set; }

    /// <summary>
    /// Gets or sets the third part of the game release, such as 81 in 1.73.81.
    /// </summary>
    /// <value>
    /// The build number of the release the client claims to be; it must fit in 10 bits because Login packs it into the low 10 bits of its version.
    /// </value>
    public required int PatchVersion { get; set; }

    /// <summary>
    /// Gets or sets the version of the connection protocol itself, separate from the game release.
    /// </summary>
    /// <value>
    /// The first field of the hello; the managed client sends <see cref="ProtocolConfiguration.WireProtocolVersion"/>, which is 3.
    /// </value>
    public required int ProtocolVersion { get; set; }

    /// <summary>
    /// Gets or sets the last field of the hello, whose meaning is not identified.
    /// </summary>
    /// <value>
    /// A 32-bit integer written after <see cref="AppStore"/>; the managed client always sends -1, and it defaults to 0 when not set.
    /// </value>
    public int Unknown1 { get; set; }
}
