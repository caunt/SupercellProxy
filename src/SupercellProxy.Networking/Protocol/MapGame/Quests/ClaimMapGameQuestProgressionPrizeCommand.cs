using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Quests;

/// <summary>Claims the mystery-box prize for one completed Valley quest-progression milestone.</summary>
public sealed record ClaimMapGameQuestProgressionPrizeCommand(int ProgressionPrizeGlobalId) : CodecCommand<ClaimMapGameQuestProgressionPrizeCommand>, ICommandCodecProvider<ClaimMapGameQuestProgressionPrizeCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimMapGameQuestProgressionPrizeCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.ProgressionPrizeGlobalId, static id => new ClaimMapGameQuestProgressionPrizeCommand(id))
    );
}
