using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Sets Neighborhood Object leaderboard scores by their long identifiers.</summary>
public sealed record SetNeighborhoodObjectLeaderboardScoresCommand : Command
{
    /// <summary>Initializes a leaderboard score update command.</summary>
    public SetNeighborhoodObjectLeaderboardScoresCommand(
        ReadOnlyMemory<int> scores,
        ReadOnlyMemory<long> scoreIdentifiers,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    ) : base(executionPhaseCounter, debugData0, debugData1)
    {
        Scores = scores.ToArray();
        ScoreIdentifiers = scoreIdentifiers.ToArray();
    }

    /// <summary>Gets the corresponding long identifiers in wire order.</summary>
    public ReadOnlyMemory<long> ScoreIdentifiers { get; }

    /// <summary>Gets the scores in wire order.</summary>
    public ReadOnlyMemory<int> Scores { get; }

    /// <inheritdoc />
    public override int Type => CommandRegistry.SetNeighborhoodObjectLeaderboardScoresCommandType;

    /// <summary>Decodes the base fields followed by scores and long identifiers.</summary>
    public static SetNeighborhoodObjectLeaderboardScoresCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);
        ReadOnlyMemory<int> scores = CommandVariableIntArrayField.Decode(stream).Values;
        ReadOnlyMemory<long> identifiers = CommandVariableLongArrayField.Decode(stream).Values;

        return new SetNeighborhoodObjectLeaderboardScoresCommand(scores, identifiers, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        EncodeCommand(stream, environment);
        new CommandVariableIntArrayField(Scores).Encode(stream);
        new CommandVariableLongArrayField(ScoreIdentifiers).Encode(stream);
    }
}
