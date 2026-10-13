using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Derby;

/// <summary>The avatar's current and previous derby points, ranks, seasons and leagues, in native field order.</summary>
public sealed record AvatarDerbyProgress : ICodecProvider<AvatarDerbyProgress>
{
    /// <inheritdoc />
    public static ICodec<AvatarDerbyProgress> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static progress => progress.CurrentNeighborhoodPoints,
        Codecs.VarInt,
        static progress => progress.PreviousPlayerPoints,
        Codecs.VarInt,
        static progress => progress.PreviousNeighborhoodPoints,
        Codecs.VarInt,
        static progress => progress.PreviousRank,
        Codecs.VarInt,
        static progress => progress.CurrentSeasonId,
        Codecs.VarInt,
        static progress => progress.PreviousSeasonId,
        Codecs.VarInt,
        static progress => progress.CurrentLeagueIndex,
        Codecs.VarInt,
        static progress => progress.CurrentParticipantCount,
        Codecs.VarInt,
        static progress => progress.PreviousLeagueIndex,
        Codecs.VarInt,
        static progress => progress.PreviousParticipantCount,
        Codecs.VarInt,
        static progress => progress.NextLeagueIndex,
        static (
            currentNeighborhoodPoints,
            previousPlayerPoints,
            previousNeighborhoodPoints,
            previousRank,
            currentSeasonId,
            previousSeasonId,
            currentLeagueIndex,
            currentParticipantCount,
            previousLeagueIndex,
            previousParticipantCount,
            nextLeagueIndex
        ) => new AvatarDerbyProgress
        {
            CurrentNeighborhoodPoints = currentNeighborhoodPoints,
            PreviousPlayerPoints = previousPlayerPoints,
            PreviousNeighborhoodPoints = previousNeighborhoodPoints,
            PreviousRank = previousRank,
            CurrentSeasonId = currentSeasonId,
            PreviousSeasonId = previousSeasonId,
            CurrentLeagueIndex = currentLeagueIndex,
            CurrentParticipantCount = currentParticipantCount,
            PreviousLeagueIndex = previousLeagueIndex,
            PreviousParticipantCount = previousParticipantCount,
            NextLeagueIndex = nextLeagueIndex,
        }
    );

    /// <summary>Gets the league index of the running derby.</summary>
    public int CurrentLeagueIndex { get; init; }

    /// <summary>Gets the neighborhood points of the running derby.</summary>
    public int CurrentNeighborhoodPoints { get; init; }

    /// <summary>Gets the number of neighborhoods in the running derby.</summary>
    public int CurrentParticipantCount { get; init; }

    /// <summary>Gets the season of the running derby.</summary>
    public int CurrentSeasonId { get; init; }

    /// <summary>Gets the league index the next derby starts in.</summary>
    public int NextLeagueIndex { get; init; }

    /// <summary>Gets the league index of the previous derby.</summary>
    public int PreviousLeagueIndex { get; init; }

    /// <summary>Gets the neighborhood points of the previous derby; native awards the previous derby's keys from them.</summary>
    public int PreviousNeighborhoodPoints { get; init; }

    /// <summary>Gets the number of neighborhoods in the previous derby.</summary>
    public int PreviousParticipantCount { get; init; }

    /// <summary>Gets the player's points in the previous derby.</summary>
    public int PreviousPlayerPoints { get; init; }

    /// <summary>Gets the neighborhood's rank in the previous derby.</summary>
    public int PreviousRank { get; init; }

    /// <summary>Gets the season of the previous derby.</summary>
    public int PreviousSeasonId { get; init; }

    /// <summary>Gets the values in native field order.</summary>
    public int[] ToValues()
    {
        return [
        CurrentNeighborhoodPoints,
        PreviousPlayerPoints,
        PreviousNeighborhoodPoints,
        PreviousRank,
        CurrentSeasonId,
        PreviousSeasonId,
        CurrentLeagueIndex,
        CurrentParticipantCount,
        PreviousLeagueIndex,
        PreviousParticipantCount,
        NextLeagueIndex,
    ];
    }
}
