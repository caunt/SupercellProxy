using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Race;

/// <summary>Preserves the native neighborhood ranking entry and badge.</summary>
public sealed record DerbyRankingEntry(
    int Rank,
    int Score,
    int RankValue,
    LongId NeighborhoodId,
    string? NeighborhoodName,
    int Badge0,
    int Badge1,
    int Badge2,
    int Value
)
{
    /// <summary>Decodes the inherited ranking fields followed by the badge and final value.</summary>
    public static DerbyRankingEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadLongId(),
            stream.ReadOptionalString(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt()
        );
    }

    /// <summary>Encodes the complete neighborhood ranking entry.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(Rank);
        stream.WriteVarInt(Score);
        stream.WriteVarInt(RankValue);
        stream.WriteLongId(NeighborhoodId);
        stream.WriteOptionalString(NeighborhoodName);
        stream.WriteVarInt(Badge0);
        stream.WriteVarInt(Badge1);
        stream.WriteVarInt(Badge2);
        stream.WriteVarInt(Value);
    }

    /// <summary>Omits neighborhood details from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(DerbyRankingEntry);
    }
}
