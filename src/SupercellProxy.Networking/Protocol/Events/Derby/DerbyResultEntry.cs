using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Preserves the eight ordered values and final flag in an end-of-derby result entry.</summary>
public sealed record DerbyResultEntry(int Value0, int Value1, int Value2, int Value3, int Value4, int Value5, int Value6, int Value7, bool Flag)
{
    /// <summary>Decodes the native result entry.</summary>
    public static DerbyResultEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadBoolean()
        );
    }

    /// <summary>Encodes the native result entry.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(Value0);
        stream.WriteVarInt(Value1);
        stream.WriteVarInt(Value2);
        stream.WriteVarInt(Value3);
        stream.WriteVarInt(Value4);
        stream.WriteVarInt(Value5);
        stream.WriteVarInt(Value6);
        stream.WriteVarInt(Value7);
        stream.WriteBoolean(Flag);
    }
}
