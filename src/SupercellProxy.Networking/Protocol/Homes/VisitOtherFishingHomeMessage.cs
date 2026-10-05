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
    public required LongId Target { get; init; }

    /// <summary>
    /// Creates a <c language="csharp">VisitOtherFishingHomeMessage</c> from the supplied data.
    /// </summary>
    public static VisitOtherFishingHomeMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new VisitOtherFishingHomeMessage { Target = stream.ReadLongId() };
    }

    /// <summary>
    /// Executes the <c language="csharp">Encode</c> operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteLongId(Target);
    }
}
