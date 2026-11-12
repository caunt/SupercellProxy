using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages.History;

/// <summary>Preserves one player's derby task-log counters; bingo lists use the count and points for bingo progress.</summary>
/// <param name="PlayerId">The account identifier of the neighborhood member this row describes.</param>
/// <param name="PlayerName">The member's display name, or <see langword="null"/> when native wrote the absent-string marker instead of a name.</param>
/// <param name="PlayerLevel">The member's experience level as shown in the log.</param>
/// <param name="TaskCount">How many tasks the member has completed in this derby; in a bingo list it counts bingo progress instead.</param>
/// <param name="Points">The derby points the member has earned; in a bingo list it holds the bingo progress points.</param>
/// <param name="TaskLimit">The member's total task allowance, against which <paramref name="TaskCount"/> is shown.</param>
/// <param name="TrashedTaskCount">How many tasks the member has trashed from the board in this derby, the last counter of the row.</param>
public sealed record DerbyMemberTaskLogEntry(LongId PlayerId, string? PlayerName, int PlayerLevel, int TaskCount, int Points, int TaskLimit, int TrashedTaskCount) : ICodecProvider<DerbyMemberTaskLogEntry>
{
    /// <inheritdoc />
    public static ICodec<DerbyMemberTaskLogEntry> Codec { get; } = Codecs.Composite(Codecs.LongId,
        static value => value.PlayerId,
        Codecs.OptionalString,
        static value => value.PlayerName,
        Codecs.VarInt,
        static value => value.PlayerLevel,
        Codecs.VarInt,
        static value => value.TaskCount,
        Codecs.VarInt,
        static value => value.Points,
        Codecs.VarInt,
        static value => value.TaskLimit,
        Codecs.VarInt,
        static value => value.TrashedTaskCount,
        static (playerId, playerName, playerLevel, taskCount, points, taskLimit, trashedTaskCount) => new DerbyMemberTaskLogEntry(playerId, playerName, playerLevel, taskCount, points, taskLimit, trashedTaskCount));

    /// <summary>Omits player details from diagnostic text.</summary>
    /// <returns>The type name <c>DerbyMemberTaskLogEntry</c> alone, so the member's identifier and name never reach logs.</returns>
    public override string ToString()
    {
        return nameof(DerbyMemberTaskLogEntry);
    }
}
