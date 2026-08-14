using SupercellProxy.Playground.Network.Streams;
using SupercellProxy.Playground.Supercell;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

public record VisitOtherFishingHomeMessage : IMessage
{
    public required LogicLong Target { get; init; }

    public static VisitOtherFishingHomeMessage Create(MessageContainer container)
    {
        return new VisitOtherFishingHomeMessage
        {
            Target = container.Payload.ReadLogicLong()
        };
    }

    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var stream = SupercellStream.Create();

        stream.WriteLogicLong(Target);

        return new MessageContainer(id, version, stream);
    }
}
