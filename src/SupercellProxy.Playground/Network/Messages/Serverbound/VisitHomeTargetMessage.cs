using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

/// <summary>
/// Represents the <c>VisitHomeTargetMessage</c> protocol message.
/// </summary>
public record VisitHomeTargetMessage : IMessage
{
    /// <summary>
    /// Gets or sets the <c>Unknown0</c> value.
    /// </summary>
    public required byte Unknown0 { get; init; }

    /// <summary>
    /// Gets or sets the <c>Target</c> value.
    /// </summary>
    public required LongId Target { get; init; }

    /// <summary>
    /// Creates a <c>VisitHomeTargetMessage</c> from the supplied data.
    /// </summary>
    public static VisitHomeTargetMessage Create(MessageContainer container)
    {
        return new VisitHomeTargetMessage
        {
            Unknown0 = container.Payload.ReadByte(),
            Target = container.Payload.ReadLongId(),
        };
    }

    /// <summary>
    /// Executes the <c>ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var supercellStream = MessageStream.Create();

        supercellStream.WriteByte(Unknown0);
        supercellStream.WriteLongId(Target);

        return new MessageContainer(id, version, supercellStream);
    }
}
