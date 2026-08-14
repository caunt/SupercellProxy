using SupercellProxy.Playground.Network.Streams;
using SupercellProxy.Playground.Supercell;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

public record ClientHelloMessage : IMessage
{
    public required int ProtocolVersion { get; set; }
    public required int KeyVersion { get; set; }

    public required int MajorVersion { get; set; }
    public required int MinorVersion { get; set; }
    public required int PatchVersion { get; set; }

    public required string FingerprintSha1 { get; set; }

    public required int DeviceType { get; set; }
    public required AppStore AppStore { get; set; }

    public int Unknown1 { get; set; }

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
            AppStore = (AppStore)container.Payload.ReadInt32(),
            Unknown1 = container.Payload.ReadInt32()
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
        supercellStream.WriteInt32((int)AppStore);

        supercellStream.WriteInt32(Unknown1);

        return new MessageContainer(id, version, supercellStream);
    }
}
