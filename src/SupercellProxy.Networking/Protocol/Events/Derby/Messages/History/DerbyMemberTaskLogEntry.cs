using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages.History;

/// <summary>Preserves one player's derby task-log counters; bingo lists use the count and points for bingo progress.</summary>
public sealed record DerbyMemberTaskLogEntry(LongId PlayerId, string? PlayerName, int PlayerLevel, int TaskCount, int Points, int TaskLimit, int TrashedTaskCount) : ICodecProvider<DerbyMemberTaskLogEntry>
{
    /// <inheritdoc />
    public static ICodec<DerbyMemberTaskLogEntry> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
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
        static (playerId, playerName, playerLevel, taskCount, points, taskLimit, trashedTaskCount) => new DerbyMemberTaskLogEntry(playerId, playerName, playerLevel, taskCount, points, taskLimit, trashedTaskCount)
    );

    /// <summary>Omits player details from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(DerbyMemberTaskLogEntry);
    }
}
