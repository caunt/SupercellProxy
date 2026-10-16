using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>Asks to join a neighborhood.</summary>
/// <param name="NeighborhoodId">Neighbourhood the player wants to join.</param>
/// <param name="NeighborhoodName">Name of that neighbourhood.</param>
/// <param name="Message">Note the player sends with the request to the neighbourhood's members.</param>
public sealed record RequestJoinNeighborhoodMessage(LongId NeighborhoodId, string? NeighborhoodName, string? Message) : CodecMessage<RequestJoinNeighborhoodMessage>, ICodecProvider<RequestJoinNeighborhoodMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestJoinNeighborhoodMessage> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static message => message.NeighborhoodId,
        Codecs.OptionalString,
        static message => message.NeighborhoodName,
        Codecs.OptionalString,
        static message => message.Message,
        static (neighborhoodId, neighborhoodName, message) => new RequestJoinNeighborhoodMessage(neighborhoodId, neighborhoodName, message)
    );
}
