using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Buys an additional task using the offer value received with the board.</summary>
public sealed record BuyExtraDerbyTaskMessage(int OfferValue, int DiamondCost) : CodecMessage<BuyExtraDerbyTaskMessage>, ICodecProvider<BuyExtraDerbyTaskMessage>
{
    /// <inheritdoc />
    public static ICodec<BuyExtraDerbyTaskMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.VarInt,
            static message => message.OfferValue,
            Codecs.VarInt,
            static message => message.DiamondCost,
            static (offerValue, diamondCost) => new BuyExtraDerbyTaskMessage(offerValue, diamondCost)
        ),
        trailingDataMessage: "The derby message has trailing data."
    );
}
