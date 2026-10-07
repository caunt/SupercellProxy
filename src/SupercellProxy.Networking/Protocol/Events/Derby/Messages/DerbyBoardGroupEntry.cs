using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Preserves one board group and its associated player identifiers.</summary>
public sealed record DerbyBoardGroupEntry(int Value0, int Value1, int Value2, int Value3, int Value4, int Value5, int Value6, LongId[] PlayerIds)
{
    /// <summary>Decodes the native counters and player list.</summary>
    public static DerbyBoardGroupEntry Decode(MessageStream stream)
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
            stream.ReadArray(static reader => reader.ReadLongId())
        );
    }

    /// <summary>Encodes the native counters and player list.</summary>
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
        stream.WriteArray<LongId>(PlayerIds, static (writer, value) => writer.WriteLongId(value));
    }

    /// <summary>Omits identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(DerbyBoardGroupEntry);
    }
}
