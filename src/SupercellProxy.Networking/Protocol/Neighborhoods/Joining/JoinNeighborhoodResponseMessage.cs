using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Joining;

/// <summary>Answers a neighborhood join with a result code and, on success, the joined neighborhood.</summary>
/// <param name="Neighborhood">The joined neighbourhood with its profile and members; null when the join failed.</param>
/// <param name="Result">The result code; 1 and 2 are success, 3 to 9 are failures such as the target already being in another neighborhood.</param>
/// <param name="Value">Number sent after the result; its meaning is not identified.</param>
public sealed record JoinNeighborhoodResponseMessage(NeighborhoodFullEntry? Neighborhood, int Result, int Value) : CodecMessage<JoinNeighborhoodResponseMessage>, ICodecProvider<JoinNeighborhoodResponseMessage>
{
    /// <inheritdoc />
    public static ICodec<JoinNeighborhoodResponseMessage> Codec { get; } = Codecs.Composite(
        Codecs.Optional(NeighborhoodFullEntry.Codec),
        static message => message.Neighborhood,
        Codecs.VarInt,
        static message => message.Result,
        Codecs.VarInt,
        static message => message.Value,
        static (neighborhood, result, @value) => new JoinNeighborhoodResponseMessage(neighborhood, result, value)
    );
}
