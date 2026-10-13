using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Joining;

/// <summary>Answers a neighborhood membership change with a result code the neighborhood popup reacts to.</summary>
/// <param name="Value">The value.</param>
/// <param name="Result">The result.</param>
public sealed record LeaveNeighborhoodResponseMessage(int Value, int Result) : CodecMessage<LeaveNeighborhoodResponseMessage>, ICodecProvider<LeaveNeighborhoodResponseMessage>
{
    /// <inheritdoc />
    public static ICodec<LeaveNeighborhoodResponseMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.Value,
        Codecs.VarInt,
        static message => message.Result,
        static (@value, result) => new LeaveNeighborhoodResponseMessage(value, result)
    );
}
