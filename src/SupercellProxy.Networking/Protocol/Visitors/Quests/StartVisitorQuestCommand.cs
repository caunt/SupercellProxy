using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Visitors.Quests;

/// <summary>Starts a farm visitor quest.</summary>
/// <param name="Choice">The choice.</param>
/// <param name="VisitorGlobalId">The visitor data id.</param>
/// <param name="QuestIndex">The quest index.</param>
public sealed record StartVisitorQuestCommand(int Choice, int VisitorGlobalId, int QuestIndex) : CodecCommand<StartVisitorQuestCommand>, ICommandCodecProvider<StartVisitorQuestCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StartVisitorQuestCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.Choice,
            Codecs.VarInt,
            static command => command.VisitorGlobalId,
            Codecs.VarInt,
            static command => command.QuestIndex,
            static (choice, visitorGlobalId, questIndex) => new StartVisitorQuestCommand(choice, visitorGlobalId, questIndex)
        ),
        CommandLayout.MetadataFirst
    );
}
