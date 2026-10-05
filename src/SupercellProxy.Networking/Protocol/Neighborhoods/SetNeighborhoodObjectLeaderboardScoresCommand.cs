using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Sets Neighborhood Object leaderboard scores by their long ids.</summary>
public sealed record SetNeighborhoodObjectLeaderboardScoresCommand : Command
{
    /// <summary>Initializes a leaderboard score update command.</summary>
    public SetNeighborhoodObjectLeaderboardScoresCommand(ReadOnlyMemory<int> scores, ReadOnlyMemory<long> scoreIds)
    {
        Scores = scores.ToArray();
        ScoreIds = scoreIds.ToArray();
    }

    /// <summary>Gets the corresponding long ids in wire order.</summary>
    public ReadOnlyMemory<long> ScoreIds { get; }

    /// <summary>Gets the scores in wire order.</summary>
    public ReadOnlyMemory<int> Scores { get; }

    /// <inheritdoc />
    public override int Type => CommandRegistry.SetNeighborhoodObjectLeaderboardScoresCommandType;

    /// <summary>Decodes the scores and long ids.</summary>
    public static SetNeighborhoodObjectLeaderboardScoresCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        ReadOnlyMemory<int> scores = CommandVarIntArrayField.Decode(stream).Values;
        ReadOnlyMemory<long> ids = CommandVarLongArrayField.Decode(stream).Values;

        return new SetNeighborhoodObjectLeaderboardScoresCommand(scores, ids);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        new CommandVarIntArrayField(Scores).Encode(stream);
        new CommandVarLongArrayField(ScoreIds).Encode(stream);
    }
}
