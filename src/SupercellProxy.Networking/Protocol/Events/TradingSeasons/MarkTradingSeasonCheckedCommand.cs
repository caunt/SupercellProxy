using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.TradingSeasons;

/// <summary>Records the trading season most recently checked by the player.</summary>
public sealed record MarkTradingSeasonCheckedCommand(int TradingSeasonId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.MarkTradingSeasonCheckedCommandType;

    /// <summary>Decodes the season id.</summary>
    public static MarkTradingSeasonCheckedCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int seasonId = stream.ReadVarInt();

        return new MarkTradingSeasonCheckedCommand(seasonId);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(TradingSeasonId);
    }
}
