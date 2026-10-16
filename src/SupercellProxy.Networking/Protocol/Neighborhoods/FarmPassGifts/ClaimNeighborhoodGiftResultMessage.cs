using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.FarmPassGifts;

/// <summary>Reports the result of claiming a neighborhood gift.</summary>
/// <param name="Result">0 and 1 are success; other codes are already claimed, not part of the neighborhood, changed neighborhood or expired gift.</param>
public sealed record ClaimNeighborhoodGiftResultMessage(int Result) : CodecMessage<ClaimNeighborhoodGiftResultMessage>, ICodecProvider<ClaimNeighborhoodGiftResultMessage>
{
    /// <inheritdoc />
    public static ICodec<ClaimNeighborhoodGiftResultMessage> Codec { get; } = Codecs.Composite(Codecs.Int32, static message => message.Result, static result => new ClaimNeighborhoodGiftResultMessage(result));
}
