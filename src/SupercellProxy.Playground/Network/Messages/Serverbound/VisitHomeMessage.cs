using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

public record VisitHome : IMessage
{
    public required byte Unknown0 { get; init; }
    public required byte Unknown1 { get; init; }

    public static VisitHome Create(MessageContainer container)
    {
        return new VisitHome
        {
            Unknown0 = container.Payload.ReadByte(),
            Unknown1 = container.Payload.ReadByte()
        };
    }

    public MessageContainer ToContainer(ushort id, ushort version = 5213)
    {
        using var supercellStream = SupercellStream.Create();

        supercellStream.WriteByte(Unknown0);
        supercellStream.WriteByte(Unknown1);

        return new MessageContainer(id, version, supercellStream);
    }
}
