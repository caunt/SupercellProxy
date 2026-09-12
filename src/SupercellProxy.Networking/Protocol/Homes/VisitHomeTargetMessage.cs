using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Represents the <c language="csharp">VisitHomeTargetMessage</c> protocol message.
/// </summary>
public sealed record VisitHomeTargetMessage : IMessage
{

    /// <summary>
    /// Gets or sets the <c language="csharp">Target</c> value.
    /// </summary>
    public required LongIdentifier Target { get; init; }
    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public required byte Unknown0 { get; init; }

    /// <summary>
    /// Creates a <c language="csharp">VisitHomeTargetMessage</c> from the supplied data.
    /// </summary>
    public static VisitHomeTargetMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        return new VisitHomeTargetMessage
        {
            Unknown0 = container.Payload.ReadByte(),
            Target = container.Payload.ReadLongIdentifier(),
        };
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream supercellStream = MessageStream.Create();

        supercellStream.WriteByte(Unknown0);
        supercellStream.WriteLongIdentifier(Target);

        return new MessageContainer(identifier, version, supercellStream);
    }
}
