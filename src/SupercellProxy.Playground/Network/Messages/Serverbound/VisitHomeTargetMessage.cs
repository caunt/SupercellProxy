using SupercellProxy.Playground.Network.Streams;
using SupercellProxy.Playground.Supercell;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

public record VisitHomeTargetMessage : IMessage
{
    public required byte Unknown0 { get; init; }
    public required LogicLong Target { get; init; }

    public static VisitHomeTargetMessage Create(MessageContainer container)
    {
        return new VisitHomeTargetMessage
        {
            Unknown0 = container.Payload.ReadByte(),
            Target = container.Payload.ReadLogicLong()
        };
    }

    public MessageContainer ToContainer(ushort id, ushort version = 5213)
    {
        using var supercellStream = SupercellStream.Create();

        supercellStream.WriteByte(Unknown0);
        supercellStream.WriteLogicLong(Target);

        return new MessageContainer(id, version, supercellStream);
    }
}
