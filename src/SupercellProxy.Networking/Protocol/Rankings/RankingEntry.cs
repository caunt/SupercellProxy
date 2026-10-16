using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>
/// One player's line on a leaderboard.
/// </summary>
/// <param name="Rank">The player's place on the board, starting at 1.</param>
/// <param name="Score">The score the board ranks by.</param>
/// <param name="RankValue">Second number sent with the rank; its meaning is not identified.</param>
/// <param name="AvatarId">The player's account.</param>
/// <param name="Name">The player's farm name.</param>
/// <param name="Level">The player's experience level.</param>
/// <param name="HomeId">The player's farm, for visiting from the board.</param>
public sealed record RankingEntry(
    int Rank,
    int Score,
    int RankValue,
    [property: JsonPropertyName("AvatarId")] LongId AvatarId,
    string? Name,
    int Level,
    [property: JsonPropertyName("HomeId")] LongId HomeId
) : ICodecProvider<RankingEntry>
{
    /// <inheritdoc />
    public static ICodec<RankingEntry> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.Rank,
        Codecs.VarInt,
        static value => value.Score,
        Codecs.VarInt,
        static value => value.RankValue,
        Codecs.LongId,
        static value => value.AvatarId,
        Codecs.OptionalString,
        static value => value.Name,
        Codecs.VarInt,
        static value => value.Level,
        Codecs.LongId,
        static value => value.HomeId,
        static (rank, score, rankValue, avatarId, name, level, homeId) => new RankingEntry(rank, score, rankValue, avatarId, name, level, homeId)
    );

    /// <summary>
    /// Gets the ranking list codec: native writes -1 for an absent list and rejects counts the payload cannot hold.
    /// </summary>
    internal static ICodec<RankingEntry[]?> ListCodec => field ??= Codecs.NullableArray(Codec, CollectionCount.PerElement(minimumElementSize: 24).WithStrictAbsence());
}
