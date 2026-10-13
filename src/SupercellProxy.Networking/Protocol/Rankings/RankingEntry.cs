using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>
/// Defines the Avatar Ranking Entry contract.
/// </summary>
/// <summary>
/// Defines the Rank contract.
/// </summary>
/// <summary>
/// Defines the Score contract.
/// </summary>
/// <summary>
/// Defines the Rank Value contract.
/// </summary>
/// <summary>
/// Defines the Avatar Id contract.
/// </summary>
/// <summary>
/// Defines the Name contract.
/// </summary>
/// <summary>
/// Defines the Level contract.
/// </summary>
/// <summary>
/// Defines the Home Id contract.
/// </summary>
public sealed record RankingEntry(
    int Rank,
    int Score,
    int RankValue,
    [property: System.Text.Json.Serialization.JsonPropertyName("AvatarId")] LongId AvatarId,
    string? Name,
    int Level,
    [property: System.Text.Json.Serialization.JsonPropertyName("HomeId")] LongId HomeId
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
