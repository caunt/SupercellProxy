using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Joining;

/// <summary>Reports the result of a neighborhood action to the neighborhood manager.</summary>
/// <param name="Result">The result code; 4 to 8 are handled.</param>
public sealed record NeighborhoodActionResultMessage(int Result) : CodecMessage<NeighborhoodActionResultMessage>, ICodecProvider<NeighborhoodActionResultMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodActionResultMessage> Codec { get; } = Codecs.Composite(Codecs.VarInt, static message => message.Result, static result => new NeighborhoodActionResultMessage(result));
}
