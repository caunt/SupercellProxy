using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Directory;

/// <summary>Requests a comparison between two neighborhoods.</summary>
/// <param name="FirstNeighborhoodId">One neighbourhood to compare; null when not named.</param>
/// <param name="SecondNeighborhoodId">The other neighbourhood to compare; null when not named.</param>
public sealed record RequestNeighborhoodComparisonMessage(LongId? FirstNeighborhoodId, LongId? SecondNeighborhoodId) : CodecMessage<RequestNeighborhoodComparisonMessage>, ICodecProvider<RequestNeighborhoodComparisonMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestNeighborhoodComparisonMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalLongId,
        static message => message.FirstNeighborhoodId,
        Codecs.OptionalLongId,
        static message => message.SecondNeighborhoodId,
        static (firstNeighborhoodId, secondNeighborhoodId) => new RequestNeighborhoodComparisonMessage(firstNeighborhoodId, secondNeighborhoodId)
    );
}
