using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Preserves one five-value entry from the board's second optional collection.</summary>
public sealed record DerbyBoardAuxiliaryEntry(int Value0, int Value1, int Value2, int Value3, int Value4)
{
    /// <summary>Decodes the five native values.</summary>
    public static DerbyBoardAuxiliaryEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <summary>Encodes the five native values.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(Value0);
        stream.WriteVarInt(Value1);
        stream.WriteVarInt(Value2);
        stream.WriteVarInt(Value3);
        stream.WriteVarInt(Value4);
    }
}
