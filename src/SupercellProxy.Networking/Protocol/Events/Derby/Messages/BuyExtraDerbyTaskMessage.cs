using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Buys an additional task using the offer value received with the board.</summary>
/// <param name="OfferValue">The <see cref="DerbyBoardMessage.ExtraTaskOfferValue"/> from the latest board reply, echoed back unchanged as the first var-int of the request.</param>
/// <param name="DiamondCost">The diamond price the sender expects to pay for the extra task; the server answers with the amount it actually charged in <see cref="BuyExtraDerbyTaskResponseMessage"/>.</param>
public sealed record BuyExtraDerbyTaskMessage(int OfferValue, int DiamondCost) : CodecMessage<BuyExtraDerbyTaskMessage>, ICodecProvider<BuyExtraDerbyTaskMessage>
{
    /// <inheritdoc />
    public static ICodec<BuyExtraDerbyTaskMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.VarInt,
            static message => message.OfferValue,
            Codecs.VarInt,
            static message => message.DiamondCost,
            static (offerValue, diamondCost) => new BuyExtraDerbyTaskMessage(offerValue, diamondCost)),
        trailingDataMessage: "The derby message has trailing data.");
}
