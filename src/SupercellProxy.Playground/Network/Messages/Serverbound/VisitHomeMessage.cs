using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

/// <summary>
/// Represents the <c language="csharp">VisitHomeMessage</c> protocol message.
/// </summary>
internal sealed record VisitHomeMessage : IMessage
{
    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public required byte Unknown0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public required byte Unknown1 { get; init; }

    /// <summary>
    /// Creates a <c language="csharp">VisitHomeMessage</c> from the supplied data.
    /// </summary>
    public static VisitHomeMessage Create(MessageContainer container)
    {
        return new VisitHomeMessage
        {
            Unknown0 = container.Payload.ReadByte(),
            Unknown1 = container.Payload.ReadByte(),
        };
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var supercellStream = MessageStream.Create();

        supercellStream.WriteByte(Unknown0);
        supercellStream.WriteByte(Unknown1);

        return new MessageContainer(id, version, supercellStream);
    }
}
