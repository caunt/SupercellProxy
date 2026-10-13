using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Quests;

/// <summary>Claims the reward of a completed valley quest.</summary>
/// <param name="QuestGlobalId">The quest data id.</param>
public sealed record ClaimMapGameQuestRewardCommand(int QuestGlobalId) : CodecCommand<ClaimMapGameQuestRewardCommand>, ICommandCodecProvider<ClaimMapGameQuestRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimMapGameQuestRewardCommand> Codec { get; } = Codecs.Command(
        type: 303,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.QuestGlobalId,
            static (questGlobalId) => new ClaimMapGameQuestRewardCommand(questGlobalId)
        )
    );
}
