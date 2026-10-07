using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Preserves the board reply's optional six-value supplement.</summary>
public sealed record DerbyBoardSupplement(int Value0, int Value1, int Value2, int Value3, int Value4, int Value5)
{
    /// <summary>Decodes the six native values.</summary>
    public static DerbyBoardSupplement Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <summary>Encodes the six native values.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(Value0);
        stream.WriteVarInt(Value1);
        stream.WriteVarInt(Value2);
        stream.WriteVarInt(Value3);
        stream.WriteVarInt(Value4);
        stream.WriteVarInt(Value5);
    }
}
