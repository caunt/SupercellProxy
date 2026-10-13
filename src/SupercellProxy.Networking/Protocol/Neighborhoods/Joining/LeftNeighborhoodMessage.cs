using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Joining;

/// <summary>Tells the client it left its neighborhood; the client clears the neighborhood lists and closes the neighborhood popups.</summary>
public sealed record LeftNeighborhoodMessage : CodecMessage<LeftNeighborhoodMessage>, ICodecProvider<LeftNeighborhoodMessage>
{
    /// <inheritdoc />
    public static ICodec<LeftNeighborhoodMessage> Codec { get; } = Codecs.Unit(static () => new LeftNeighborhoodMessage());
}
