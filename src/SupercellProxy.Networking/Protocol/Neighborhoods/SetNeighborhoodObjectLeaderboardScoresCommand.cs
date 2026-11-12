using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Sets Neighborhood Object leaderboard scores by their long ids.</summary>
public sealed record SetNeighborhoodObjectLeaderboardScoresCommand : CodecCommand<SetNeighborhoodObjectLeaderboardScoresCommand>, ICommandCodecProvider<SetNeighborhoodObjectLeaderboardScoresCommand>
{
    /// <summary>Initializes a new instance of the <see cref="SetNeighborhoodObjectLeaderboardScoresCommand"/> record. Initializes a leaderboard score update command.</summary>
    /// <param name="scores">The new scores, copied into the command; entry <c>i</c> belongs to entry <c>i</c> of <paramref name="scoreIds"/>.</param>
    /// <param name="scoreIds">The 64-bit ids the scores belong to, copied into the command; it should be as long as <paramref name="scores"/>.</param>
    public SetNeighborhoodObjectLeaderboardScoresCommand(ReadOnlyMemory<int> scores, ReadOnlyMemory<long> scoreIds)
    {
        Scores = scores.ToArray();
        ScoreIds = scoreIds.ToArray();
    }

    /// <inheritdoc />
    public static CommandCodec<SetNeighborhoodObjectLeaderboardScoresCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.Memory(Codecs.VarInt),
            static command => command.Scores,
            Codecs.Memory(Codecs.VarLong),
            static command => command.ScoreIds,
            static (scores, ids) => new SetNeighborhoodObjectLeaderboardScoresCommand(scores, ids)),
        CommandLayout.MetadataFirst);

    /// <summary>Gets the corresponding long ids in wire order.</summary>
    /// <value>A VarLong array written after the scores, one id per entry of <see cref="Scores"/>.</value>
    public ReadOnlyMemory<long> ScoreIds { get; }

    /// <summary>Gets the scores in wire order.</summary>
    /// <value>A VarInt array written first, one score per entry of <see cref="ScoreIds"/>.</value>
    public ReadOnlyMemory<int> Scores { get; }
}
