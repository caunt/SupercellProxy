using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Quests;

/// <summary>Marks one completed Valley daily quest as presented to the player.</summary>
public sealed record MarkMapGameDailyQuestSeenCommand(int QuestType) : CodecCommand<MarkMapGameDailyQuestSeenCommand>, ICommandCodecProvider<MarkMapGameDailyQuestSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkMapGameDailyQuestSeenCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.QuestType, static questType => new MarkMapGameDailyQuestSeenCommand(questType))
    );
}
