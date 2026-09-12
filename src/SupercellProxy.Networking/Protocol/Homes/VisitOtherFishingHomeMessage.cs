using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Represents the <c language="csharp">VisitOtherFishingHomeMessage</c> protocol message.
/// </summary>
public sealed record VisitOtherFishingHomeMessage : IMessage
{
    /// <summary>
    /// Gets or sets the <c language="csharp">Target</c> value.
    /// </summary>
    public required LongIdentifier Target { get; init; }

    /// <summary>
    /// Creates a <c language="csharp">VisitOtherFishingHomeMessage</c> from the supplied data.
    /// </summary>
    public static VisitOtherFishingHomeMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        return new VisitOtherFishingHomeMessage { Target = container.Payload.ReadLongIdentifier() };
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteLongIdentifier(Target);

        return new MessageContainer(identifier, version, stream);
    }
}
