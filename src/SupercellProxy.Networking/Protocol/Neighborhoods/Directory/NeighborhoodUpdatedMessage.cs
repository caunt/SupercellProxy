using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Directory;

/// <summary>Tells the client a neighborhood changed so the open neighborhood screen refreshes.</summary>
/// <param name="NeighborhoodId">Neighbourhood that changed.</param>
public sealed record NeighborhoodUpdatedMessage(LongId NeighborhoodId) : CodecMessage<NeighborhoodUpdatedMessage>, ICodecProvider<NeighborhoodUpdatedMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodUpdatedMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.NeighborhoodId, static neighborhoodId => new NeighborhoodUpdatedMessage(neighborhoodId));
}
