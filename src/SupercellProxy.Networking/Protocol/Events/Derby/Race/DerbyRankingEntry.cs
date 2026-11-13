using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Race;

/// <summary>Preserves the native neighborhood ranking entry and badge.</summary>
/// <param name="Rank">The neighborhood's place in the rankings, written first.</param>
/// <param name="Score">The neighborhood's ranking score, written right after <paramref name="Rank"/>.</param>
/// <param name="RankValue">A third ranking var-int written before the neighborhood identifier; how it differs from <paramref name="Rank"/> is not yet identified.</param>
/// <param name="NeighborhoodId">The identifier of the ranked neighborhood.</param>
/// <param name="NeighborhoodName">The ranked neighborhood's display name, or <see langword="null"/> when native wrote the absent-string marker.</param>
/// <param name="Badge0">The first of the three var-ints that together select the neighborhood's badge; which badge part it picks is not yet mapped.</param>
/// <param name="Badge1">The second badge var-int, read after <paramref name="Badge0"/>.</param>
/// <param name="Badge2">The third badge var-int, read after <paramref name="Badge1"/>.</param>
/// <param name="Value">The var-int that ends the entry; its meaning is not yet identified.</param>
public sealed record DerbyRankingEntry(int Rank,
    int Score,
    int RankValue,
    LongId NeighborhoodId,
    string? NeighborhoodName,
    int Badge0,
    int Badge1,
    int Badge2,
    int Value) : ICodecProvider<DerbyRankingEntry>
{
    /// <inheritdoc />
    public static ICodec<DerbyRankingEntry> Codec { get; } = Codecs.Composite(Codecs.VarInt,
        static value => value.Rank,
        Codecs.VarInt,
        static value => value.Score,
        Codecs.VarInt,
        static value => value.RankValue,
        Codecs.LongId,
        static value => value.NeighborhoodId,
        Codecs.OptionalUtf8String,
        static value => value.NeighborhoodName,
        Codecs.VarInt,
        static value => value.Badge0,
        Codecs.VarInt,
        static value => value.Badge1,
        Codecs.VarInt,
        static value => value.Badge2,
        Codecs.VarInt,
        static value => value.Value,
        static (rank, score, rankValue, neighborhoodId, neighborhoodName, badge0, badge1, badge2, @value) => new DerbyRankingEntry(rank, score, rankValue, neighborhoodId, neighborhoodName, badge0, badge1, badge2, value));

    /// <summary>Omits neighborhood details from diagnostic text.</summary>
    /// <returns>The type name <c>DerbyRankingEntry</c> alone, so the neighborhood name and identifier never reach logs.</returns>
    public override string ToString()
    {
        return nameof(DerbyRankingEntry);
    }
}
