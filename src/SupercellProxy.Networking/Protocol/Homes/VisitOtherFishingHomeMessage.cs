using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Represents the <c language="csharp">VisitOtherFishingHomeMessage</c> protocol message.
/// </summary>
public sealed record VisitOtherFishingHomeMessage : CodecMessage<VisitOtherFishingHomeMessage>, ICodecProvider<VisitOtherFishingHomeMessage>
{
    /// <inheritdoc />
    public static ICodec<VisitOtherFishingHomeMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.Target, static (target) => new VisitOtherFishingHomeMessage { Target = target });

    /// <summary>
    /// Gets or sets the <c language="csharp">Target</c> value.
    /// </summary>
    public required LongId Target { get; init; }
}
