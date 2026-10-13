using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>Updates one roadside shop slot of the visited farm.</summary>
/// <param name="Quantity">The quantity.</param>
/// <param name="Slot">The slot.</param>
/// <param name="Price">The price.</param>
/// <param name="ItemId">The item's data id.</param>
/// <param name="OwnerId">The farm owner; the client ignores the message unless it is visiting that farm.</param>
public sealed record RoadsideShopSlotUpdatedMessage(int Quantity, int Slot, int Price, int ItemId, LongId OwnerId) : CodecMessage<RoadsideShopSlotUpdatedMessage>, ICodecProvider<RoadsideShopSlotUpdatedMessage>
{
    /// <inheritdoc />
    public static ICodec<RoadsideShopSlotUpdatedMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.Quantity,
        Codecs.VarInt,
        static message => message.Slot,
        Codecs.VarInt,
        static message => message.Price,
        Codecs.VarInt,
        static message => message.ItemId,
        Codecs.LongId,
        static message => message.OwnerId,
        static (quantity, slot, price, itemId, ownerId) => new RoadsideShopSlotUpdatedMessage(quantity, slot, price, itemId, ownerId)
    );
}
