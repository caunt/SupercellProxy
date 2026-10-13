using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Quests;

/// <summary>Records progress of a valley quest.</summary>
/// <param name="QuestGlobalId">The quest data id.</param>
/// <param name="Progress">The progress.</param>
public sealed record UpdateMapGameQuestProgressCommand(int QuestGlobalId, int Progress) : CodecCommand<UpdateMapGameQuestProgressCommand>, ICommandCodecProvider<UpdateMapGameQuestProgressCommand>
{
    /// <inheritdoc />
    public static CommandCodec<UpdateMapGameQuestProgressCommand> Codec { get; } = Codecs.Command(
        type: 302,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.QuestGlobalId,
            Codecs.VarInt,
            static command => command.Progress,
            static (questGlobalId, progress) => new UpdateMapGameQuestProgressCommand(questGlobalId, progress)
        )
    );
}
