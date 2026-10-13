using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Derby;

/// <summary>Requests a comparison between two derby neighborhoods.</summary>
/// <param name="FirstNeighborhoodId">The first neighborhood id.</param>
/// <param name="SecondNeighborhoodId">The second neighborhood id.</param>
public sealed record RequestNeighborhoodDerbyComparisonMessage(LongId? FirstNeighborhoodId, LongId? SecondNeighborhoodId) : CodecMessage<RequestNeighborhoodDerbyComparisonMessage>, ICodecProvider<RequestNeighborhoodDerbyComparisonMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestNeighborhoodDerbyComparisonMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalLongId,
        static message => message.FirstNeighborhoodId,
        Codecs.OptionalLongId,
        static message => message.SecondNeighborhoodId,
        static (firstNeighborhoodId, secondNeighborhoodId) => new RequestNeighborhoodDerbyComparisonMessage(firstNeighborhoodId, secondNeighborhoodId)
    );
}
