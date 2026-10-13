using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops.Advertisements;

/// <summary>Updates a roadside shop advertisement.</summary>
/// <param name="HomeId">The home id.</param>
/// <param name="StandIndex">The stand index.</param>
/// <param name="ItemGlobalId">The item data id.</param>
/// <param name="Sold">The sold.</param>
/// <param name="Amount">The amount.</param>
/// <param name="Price">The price.</param>
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
