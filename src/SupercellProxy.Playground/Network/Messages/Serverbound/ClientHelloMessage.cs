using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

public record ClientHelloMessage : IMessage
{
    public static ushort Id => 10100;

    public required int ProtocolVersion { get; init; }
    public required int KeyVersion { get; init; }

    public required int MajorVersion { get; init; }
    public required int MinorVersion { get; init; }
    public required int PatchVersion { get; init; }

    public required string FingerprintSha1 { get; init; }

    public required int DeviceType { get; init; }
    public required int AppStore { get; init; }

    static IMessage IMessage.Create(MessageContainer container)
    {
        return Create(container);
    }

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
            AppStore = container.Payload.ReadInt32()
        };
    }

    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var supercellStream = SupercellStream.Create();

        supercellStream.WriteInt32(ProtocolVersion);
        supercellStream.WriteInt32(KeyVersion);

        supercellStream.WriteInt32(MajorVersion);
        supercellStream.WriteInt32(MinorVersion);
        supercellStream.WriteInt32(PatchVersion);

        supercellStream.WriteString(FingerprintSha1);

        supercellStream.WriteInt32(DeviceType);
        supercellStream.WriteInt32(AppStore);

        return new MessageContainer(id, version, supercellStream);
    }
}
