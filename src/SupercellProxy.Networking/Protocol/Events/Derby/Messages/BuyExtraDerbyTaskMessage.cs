using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Buys an additional task using the offer value received with the board.</summary>
public sealed record BuyExtraDerbyTaskMessage(int OfferValue, int DiamondCost) : IMessage
{
    /// <summary>Decodes the board offer and quoted diamond price.</summary>
    public static BuyExtraDerbyTaskMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        BuyExtraDerbyTaskMessage message = new(stream.ReadVarInt(), stream.ReadVarInt());
        DerbyMessageCodec.RequireEnd(stream);

        return message;
    }

    /// <summary>Encodes the additional-task purchase request.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(OfferValue);
        stream.WriteVarInt(DiamondCost);
    }
}
