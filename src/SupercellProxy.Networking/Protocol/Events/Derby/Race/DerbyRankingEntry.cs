using SupercellProxy.Networking.Protocol.Serialization;

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
) : ICodecProvider<DerbyRankingEntry>
{
    /// <inheritdoc />
    public static ICodec<DerbyRankingEntry> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.Rank,
        Codecs.VarInt,
        static value => value.Score,
        Codecs.VarInt,
        static value => value.RankValue,
        Codecs.LongId,
        static value => value.NeighborhoodId,
        Codecs.OptionalString,
        static value => value.NeighborhoodName,
        Codecs.VarInt,
        static value => value.Badge0,
        Codecs.VarInt,
        static value => value.Badge1,
        Codecs.VarInt,
        static value => value.Badge2,
        Codecs.VarInt,
        static value => value.Value,
        static (rank, score, rankValue, neighborhoodId, neighborhoodName, badge0, badge1, badge2, @value) => new DerbyRankingEntry(rank, score, rankValue, neighborhoodId, neighborhoodName, badge0, badge1, badge2, value)
    );

    /// <summary>Omits neighborhood details from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(DerbyRankingEntry);
    }
}
