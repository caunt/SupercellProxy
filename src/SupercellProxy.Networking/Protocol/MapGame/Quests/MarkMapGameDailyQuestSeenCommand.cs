using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MapGame.Quests;

/// <summary>Marks one completed Valley daily quest as presented to the player.</summary>
public sealed record MarkMapGameDailyQuestSeenCommand(int QuestType, int ExecutionPhaseCounter = -1, CommandData? DebugData0 = null, CommandData? DebugData1 = null) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.MarkMapGameDailyQuestSeenCommandType;

    /// <summary>Decodes a command from its native wire representation.</summary>
    public static MarkMapGameDailyQuestSeenCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int questType = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields = DecodeCommand(stream, environment);

        return new MarkMapGameDailyQuestSeenCommand(questType, commandFields.ExecutionPhaseCounter, commandFields.DebugData0, commandFields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(QuestType);
        EncodeCommand(stream, environment);
    }
}
