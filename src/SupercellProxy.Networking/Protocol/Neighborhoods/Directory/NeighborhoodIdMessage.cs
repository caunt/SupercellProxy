using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Directory;

/// <summary>Carries a neighborhood id; the client ignores it.</summary>
/// <param name="NeighborhoodId">Neighbourhood the message is about.</param>
public sealed record NeighborhoodIdMessage(LongId NeighborhoodId) : CodecMessage<NeighborhoodIdMessage>, ICodecProvider<NeighborhoodIdMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodIdMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.NeighborhoodId, static neighborhoodId => new NeighborhoodIdMessage(neighborhoodId));
}
