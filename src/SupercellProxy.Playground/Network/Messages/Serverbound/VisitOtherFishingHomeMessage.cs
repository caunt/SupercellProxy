using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

/// <summary>
/// Represents the <c>VisitOtherFishingHomeMessage</c> protocol message.
/// </summary>
public record VisitOtherFishingHomeMessage : IMessage
{
    /// <summary>
    /// Gets or sets the <c>Target</c> value.
    /// </summary>
    public required LongId Target { get; init; }

    /// <summary>
    /// Creates a <c>VisitOtherFishingHomeMessage</c> from the supplied data.
    /// </summary>
    public static VisitOtherFishingHomeMessage Create(MessageContainer container)
    {
        return new VisitOtherFishingHomeMessage { Target = container.Payload.ReadLongId() };
    }

    /// <summary>
    /// Executes the <c>ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var stream = MessageStream.Create();

        stream.WriteLongId(Target);

        return new MessageContainer(id, version, stream);
    }
}
