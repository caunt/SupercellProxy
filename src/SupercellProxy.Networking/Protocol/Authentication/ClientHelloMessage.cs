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
    /// Gets or sets the <c language="csharp">AppStore</c> value.
    /// </summary>
    public required AppStore AppStore { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">DeviceType</c> value.
    /// </summary>
    public required int DeviceType { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">FingerprintSha1</c> value.
    /// </summary>
    public required string FingerprintSha1 { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">KeyVersion</c> value.
    /// </summary>
    public required int KeyVersion { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">MajorVersion</c> value.
    /// </summary>
    public required int MajorVersion { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">MinorVersion</c> value.
    /// </summary>
    public required int MinorVersion { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PatchVersion</c> value.
    /// </summary>
    public required int PatchVersion { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ProtocolVersion</c> value.
    /// </summary>
    public required int ProtocolVersion { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public int Unknown1 { get; set; }
}
