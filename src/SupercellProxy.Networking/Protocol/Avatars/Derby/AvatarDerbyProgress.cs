using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Derby;

/// <summary>The avatar's current and previous derby points, ranks, seasons and leagues, in native field order.</summary>
public sealed record AvatarDerbyProgress : ICodecProvider<AvatarDerbyProgress>
{
    /// <inheritdoc />
    public static ICodec<AvatarDerbyProgress> Codec { get; } = Codecs.Composite(Codecs.VarInt,
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
        static (currentNeighborhoodPoints,
            previousPlayerPoints,
            previousNeighborhoodPoints,
            previousRank,
            currentSeasonId,
            previousSeasonId,
            currentLeagueIndex,
            currentParticipantCount,
            previousLeagueIndex,
            previousParticipantCount,
            nextLeagueIndex) => new AvatarDerbyProgress
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
            });

    /// <summary>Gets or sets the league index of the running derby.</summary>
    /// <value>The index of the league the neighborhood plays in this derby, the seventh VarInt of the block; 0 by default.</value>
    public int CurrentLeagueIndex { get; init; }

    /// <summary>Gets or sets the neighborhood points of the running derby.</summary>
    /// <value>The neighborhood's point total so far, the first VarInt of the block; 0 by default and when no derby is running.</value>
    public int CurrentNeighborhoodPoints { get; init; }

    /// <summary>Gets or sets the number of neighborhoods in the running derby.</summary>
    /// <value>The size of the neighborhood's derby group, the eighth VarInt of the block; 0 by default.</value>
    public int CurrentParticipantCount { get; init; }

    /// <summary>Gets or sets the season of the running derby.</summary>
    /// <value>The derby season id, the fifth VarInt of the block; 0 by default.</value>
    public int CurrentSeasonId { get; init; }

    /// <summary>Gets or sets the league index the next derby starts in.</summary>
    /// <value>The index of the league the neighborhood moves to after the running derby, the eleventh and last VarInt of the block; the server also sends it when the derby ends or a neighborhood is joined.</value>
    public int NextLeagueIndex { get; init; }

    /// <summary>Gets or sets the league index of the previous derby.</summary>
    /// <value>The index of the league the neighborhood played in last derby, the ninth VarInt of the block; 0 by default.</value>
    public int PreviousLeagueIndex { get; init; }

    /// <summary>Gets or sets the neighborhood points of the previous derby.</summary>
    /// <value>The neighborhood's final point total of the last derby, the third VarInt of the block; 0 by default.</value>
    public int PreviousNeighborhoodPoints { get; init; }

    /// <summary>Gets or sets the number of neighborhoods in the previous derby.</summary>
    /// <value>The size of the last derby's group, the tenth VarInt of the block; 0 by default.</value>
    public int PreviousParticipantCount { get; init; }

    /// <summary>Gets or sets the player's points in the previous derby.</summary>
    /// <value>The player's own contribution to the last derby, the second VarInt of the block; 0 by default.</value>
    public int PreviousPlayerPoints { get; init; }

    /// <summary>Gets or sets the neighborhood's rank in the previous derby.</summary>
    /// <value>The neighborhood's final placing within its group, the fourth VarInt of the block; 0 by default.</value>
    public int PreviousRank { get; init; }

    /// <summary>Gets or sets the season of the previous derby.</summary>
    /// <value>The derby season id of the last derby, the sixth VarInt of the block; 0 by default.</value>
    public int PreviousSeasonId { get; init; }

    /// <summary>Gets the values in native field order.</summary>
    /// <returns>A new eleven-element array in wire order, from <see cref="CurrentNeighborhoodPoints"/> to <see cref="NextLeagueIndex"/>.</returns>
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
