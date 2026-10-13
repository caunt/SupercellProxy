using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Events;

/// <summary>Requests the neighborhood events.</summary>
public sealed record RequestNeighborhoodEventsMessage : CodecMessage<RequestNeighborhoodEventsMessage>, ICodecProvider<RequestNeighborhoodEventsMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestNeighborhoodEventsMessage> Codec { get; } = Codecs.Unit(static () => new RequestNeighborhoodEventsMessage());
}
