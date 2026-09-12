using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Represents the <c language="csharp">VisitHomeMessage</c> protocol message.
/// </summary>
public sealed record VisitHomeMessage : IMessage
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
        ArgumentNullException.ThrowIfNull(container);

        return new VisitHomeMessage
        {
            Unknown0 = container.Payload.ReadByte(),
            Unknown1 = container.Payload.ReadByte(),
        };
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream supercellStream = MessageStream.Create();

        supercellStream.WriteByte(Unknown0);
        supercellStream.WriteByte(Unknown1);

        return new MessageContainer(identifier, version, supercellStream);
    }
}
