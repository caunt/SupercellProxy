using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops.Sales;

/// <summary>Collects the coins of a roadside shop slot an AI customer bought from.</summary>
/// <param name="SlotIndex">Position of the sold stand in the own shop.</param>
public sealed record CollectRoadsideShopSaleCommand(int SlotIndex) : CodecCommand<CollectRoadsideShopSaleCommand>, ICommandCodecProvider<CollectRoadsideShopSaleCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectRoadsideShopSaleCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.SlotIndex, static slotIndex => new CollectRoadsideShopSaleCommand(slotIndex))
    );
}
