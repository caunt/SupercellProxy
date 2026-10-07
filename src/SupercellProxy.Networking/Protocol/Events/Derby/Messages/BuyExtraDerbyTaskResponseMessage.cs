using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Returns the additional-task purchase status and actual diamond charge.</summary>
public sealed record BuyExtraDerbyTaskResponseMessage(int Status, int DiamondCost) : IMessage
{
    /// <summary>Decodes the additional-task purchase result.</summary>
    public static BuyExtraDerbyTaskResponseMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        BuyExtraDerbyTaskResponseMessage message = new(stream.ReadVarInt(), stream.ReadVarInt());
        DerbyMessageCodec.RequireEnd(stream);

        return message;
    }

    /// <summary>Encodes the additional-task purchase result.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(Status);
        stream.WriteVarInt(DiamondCost);
    }
}
