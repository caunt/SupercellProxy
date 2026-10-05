using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>
/// Represents the <c language="csharp">ClientHelloMessage</c> protocol message.
/// </summary>
public sealed record ClientHelloMessage : IMessage
{
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

    /// <summary>
    /// Creates a <c language="csharp">ClientHelloMessage</c> from the supplied data.
    /// </summary>
    public static ClientHelloMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new ClientHelloMessage
        {
            ProtocolVersion = stream.ReadInt32(),
            KeyVersion = stream.ReadInt32(),

            MajorVersion = stream.ReadInt32(),
            MinorVersion = stream.ReadInt32(),
            PatchVersion = stream.ReadInt32(),

            FingerprintSha1 = stream.ReadString(),

            DeviceType = stream.ReadInt32(),
            AppStore = System.Runtime.CompilerServices.Unsafe.BitCast<int, AppStore>(stream.ReadInt32()),
            Unknown1 = stream.ReadInt32(),
        };
    }

    /// <summary>
    /// Executes the <c language="csharp">Encode</c> operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteInt32(ProtocolVersion);
        stream.WriteInt32(KeyVersion);

        stream.WriteInt32(MajorVersion);
        stream.WriteInt32(MinorVersion);
        stream.WriteInt32(PatchVersion);

        stream.WriteString(FingerprintSha1);

        stream.WriteInt32(DeviceType);
        stream.WriteInt32(System.Runtime.CompilerServices.Unsafe.BitCast<AppStore, int>(AppStore));

        stream.WriteInt32(Unknown1);
    }
}
