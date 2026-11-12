using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Returns the additional-task purchase status and actual diamond charge.</summary>
/// <param name="Status">The raw native result code of the purchase, written first; the individual codes are not yet mapped to named outcomes.</param>
/// <param name="DiamondCost">The number of diamonds the server actually charged, which can differ from the price sent in <see cref="BuyExtraDerbyTaskMessage"/>.</param>
public sealed record BuyExtraDerbyTaskResponseMessage(int Status, int DiamondCost) : CodecMessage<BuyExtraDerbyTaskResponseMessage>, ICodecProvider<BuyExtraDerbyTaskResponseMessage>
{
    /// <inheritdoc />
    public static ICodec<BuyExtraDerbyTaskResponseMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.VarInt,
            static message => message.Status,
            Codecs.VarInt,
            static message => message.DiamondCost,
            static (status, diamondCost) => new BuyExtraDerbyTaskResponseMessage(status, diamondCost)),
        trailingDataMessage: "The derby message has trailing data.");
}
