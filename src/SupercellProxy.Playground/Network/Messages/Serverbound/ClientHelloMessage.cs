using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Protocol;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

/// <summary>
/// Represents the <c>ClientHelloMessage</c> protocol message.
/// </summary>
public record ClientHelloMessage : IMessage
{
    /// <summary>
    /// Gets or sets the <c>ProtocolVersion</c> value.
    /// </summary>
    public required int ProtocolVersion { get; set; }

    /// <summary>
    /// Gets or sets the <c>KeyVersion</c> value.
    /// </summary>
    public required int KeyVersion { get; set; }

    /// <summary>
    /// Gets or sets the <c>MajorVersion</c> value.
    /// </summary>
    public required int MajorVersion { get; set; }

    /// <summary>
    /// Gets or sets the <c>MinorVersion</c> value.
    /// </summary>
    public required int MinorVersion { get; set; }

    /// <summary>
    /// Gets or sets the <c>PatchVersion</c> value.
    /// </summary>
    public required int PatchVersion { get; set; }

    /// <summary>
    /// Gets or sets the <c>FingerprintSha1</c> value.
    /// </summary>
    public required string FingerprintSha1 { get; set; }

    /// <summary>
    /// Gets or sets the <c>DeviceType</c> value.
    /// </summary>
    public required int DeviceType { get; set; }

    /// <summary>
    /// Gets or sets the <c>AppStore</c> value.
    /// </summary>
    public required AppStore AppStore { get; set; }

    /// <summary>
    /// Gets or sets the <c>Unknown1</c> value.
    /// </summary>
    public int Unknown1 { get; set; }

    /// <summary>
    /// Creates a <c>ClientHelloMessage</c> from the supplied data.
    /// </summary>
    public static ClientHelloMessage Create(MessageContainer container)
    {
        return new ClientHelloMessage
        {
            ProtocolVersion = container.Payload.ReadInt32(),
            KeyVersion = container.Payload.ReadInt32(),

            MajorVersion = container.Payload.ReadInt32(),
            MinorVersion = container.Payload.ReadInt32(),
            PatchVersion = container.Payload.ReadInt32(),

            FingerprintSha1 = container.Payload.ReadString(),

            DeviceType = container.Payload.ReadInt32(),
            AppStore = System.Runtime.CompilerServices.Unsafe.BitCast<int, AppStore>(
                container.Payload.ReadInt32()
            ),
            Unknown1 = container.Payload.ReadInt32(),
        };
    }

    /// <summary>
    /// Executes the <c>ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var supercellStream = MessageStream.Create();

        supercellStream.WriteInt32(ProtocolVersion);
        supercellStream.WriteInt32(KeyVersion);

        supercellStream.WriteInt32(MajorVersion);
        supercellStream.WriteInt32(MinorVersion);
        supercellStream.WriteInt32(PatchVersion);

        supercellStream.WriteString(FingerprintSha1);

        supercellStream.WriteInt32(DeviceType);
        supercellStream.WriteInt32(
            System.Runtime.CompilerServices.Unsafe.BitCast<AppStore, int>(AppStore)
        );

        supercellStream.WriteInt32(Unknown1);

        return new MessageContainer(id, version, supercellStream);
    }
}
