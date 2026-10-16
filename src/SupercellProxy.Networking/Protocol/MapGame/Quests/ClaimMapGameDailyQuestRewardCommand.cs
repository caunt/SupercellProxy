using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Quests;

/// <summary>Claims the reward of a completed valley daily quest.</summary>
/// <param name="QuestGlobalId">The valley quest data id.</param>
public sealed record ClaimMapGameDailyQuestRewardCommand(int QuestGlobalId) : CodecCommand<ClaimMapGameDailyQuestRewardCommand>, ICommandCodecProvider<ClaimMapGameDailyQuestRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimMapGameDailyQuestRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.QuestGlobalId,
            static questGlobalId => new ClaimMapGameDailyQuestRewardCommand(questGlobalId)
        )
    );
}
