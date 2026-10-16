using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Quests;

/// <summary>Records progress of a valley quest.</summary>
/// <param name="QuestGlobalId">Global id of the quest's table entry.</param>
/// <param name="Progress">How much of the quest's goal is done.</param>
public sealed record UpdateMapGameQuestProgressCommand(int QuestGlobalId, int Progress) : CodecCommand<UpdateMapGameQuestProgressCommand>, ICommandCodecProvider<UpdateMapGameQuestProgressCommand>
{
    /// <inheritdoc />
    public static CommandCodec<UpdateMapGameQuestProgressCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.QuestGlobalId,
            Codecs.VarInt,
            static command => command.Progress,
            static (questGlobalId, progress) => new UpdateMapGameQuestProgressCommand(questGlobalId, progress)
        )
    );
}
