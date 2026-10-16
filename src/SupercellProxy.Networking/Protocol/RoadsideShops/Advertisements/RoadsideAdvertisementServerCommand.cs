using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops.Advertisements;

/// <summary>Updates one advertised roadside stand; the client does not act on it.</summary>
/// <param name="HomeId">Home whose shop advertises the stand.</param>
/// <param name="StandIndex">Stand position in that shop.</param>
/// <param name="ItemGlobalId">Global id of the advertised goods.</param>
/// <param name="Sold">Whether the advertised goods were bought.</param>
/// <param name="Amount">How many goods the stand holds.</param>
/// <param name="Price">Coin price of the whole stand.</param>
public sealed record RoadsideAdvertisementServerCommand(LongId HomeId, int StandIndex, int ItemGlobalId, bool Sold, int Amount, int Price) : CodecServerCommand<RoadsideAdvertisementServerCommand>, ICommandCodecProvider<RoadsideAdvertisementServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RoadsideAdvertisementServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.HomeId,
            Codecs.Int32,
            static command => command.StandIndex,
            Codecs.Int32,
            static command => command.ItemGlobalId,
            Codecs.Boolean,
            static command => command.Sold,
            Codecs.Int32,
            static command => command.Amount,
            Codecs.Int32,
            static command => command.Price,
            static (homeId, standIndex, itemGlobalId, sold, amount, price) => new RoadsideAdvertisementServerCommand(homeId, standIndex, itemGlobalId, sold, amount, price)
        )
    );
}
