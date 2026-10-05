using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MapGame.Quests;

/// <summary>Marks one completed Valley daily quest as presented to the player.</summary>
public sealed record MarkMapGameDailyQuestSeenCommand(int QuestType) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.MarkMapGameDailyQuestSeenCommandType;

    /// <summary>Decodes a command from its native wire representation.</summary>
    public static MarkMapGameDailyQuestSeenCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int questType = stream.ReadVarInt();

        return new MarkMapGameDailyQuestSeenCommand(questType);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(QuestType);
    }
}
