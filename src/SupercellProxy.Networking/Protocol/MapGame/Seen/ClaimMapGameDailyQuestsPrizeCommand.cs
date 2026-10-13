using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Seen;

/// <summary>Claims the Valley daily quests prize.</summary>
public sealed record ClaimMapGameDailyQuestsPrizeCommand : CodecCommand<ClaimMapGameDailyQuestsPrizeCommand>, ICommandCodecProvider<ClaimMapGameDailyQuestsPrizeCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimMapGameDailyQuestsPrizeCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new ClaimMapGameDailyQuestsPrizeCommand()));
}
