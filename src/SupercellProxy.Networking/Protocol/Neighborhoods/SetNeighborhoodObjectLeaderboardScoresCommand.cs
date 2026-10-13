using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Sets Neighborhood Object leaderboard scores by their long ids.</summary>
public sealed record SetNeighborhoodObjectLeaderboardScoresCommand : CodecCommand<SetNeighborhoodObjectLeaderboardScoresCommand>, ICommandCodecProvider<SetNeighborhoodObjectLeaderboardScoresCommand>
{
    /// <summary>Initializes a leaderboard score update command.</summary>
    public SetNeighborhoodObjectLeaderboardScoresCommand(ReadOnlyMemory<int> scores, ReadOnlyMemory<long> scoreIds)
    {
        Scores = scores.ToArray();
        ScoreIds = scoreIds.ToArray();
    }

    /// <inheritdoc />
    public static CommandCodec<SetNeighborhoodObjectLeaderboardScoresCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.Memory(Codecs.VarInt),
            static command => command.Scores,
            Codecs.Memory(Codecs.VarLong),
            static command => command.ScoreIds,
            static (scores, ids) => new SetNeighborhoodObjectLeaderboardScoresCommand(scores, ids)
        ),
        CommandLayout.MetadataFirst
    );

    /// <summary>Gets the corresponding long ids in wire order.</summary>
    public ReadOnlyMemory<long> ScoreIds { get; }

    /// <summary>Gets the scores in wire order.</summary>
    public ReadOnlyMemory<int> Scores { get; }
}
