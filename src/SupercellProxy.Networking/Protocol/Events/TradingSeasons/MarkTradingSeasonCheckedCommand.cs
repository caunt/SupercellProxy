using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.TradingSeasons;

/// <summary>Records the trading season most recently checked by the player.</summary>
public sealed record MarkTradingSeasonCheckedCommand(int TradingSeasonId) : CodecCommand<MarkTradingSeasonCheckedCommand>, ICommandCodecProvider<MarkTradingSeasonCheckedCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkTradingSeasonCheckedCommand> Codec { get; } = Codecs.Command(
        type: 202,
        Codecs.Composite(Codecs.VarInt, static command => command.TradingSeasonId, static (seasonId) => new MarkTradingSeasonCheckedCommand(seasonId))
    );
}
