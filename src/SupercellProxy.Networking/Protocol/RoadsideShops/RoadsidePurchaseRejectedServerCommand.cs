using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>A server refusal containing the attempted listing and the client's material-purchase limit.</summary>
public sealed record RoadsidePurchaseRejectedServerCommand(LongId BuyerId, LongId ShopOwnerId, int ItemGlobalId, int SlotIndex, int Quantity, int Price, int DailyCollectionToolLimit) : ServerCommand
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.RoadsidePurchaseRejectedServerCommandType;

    /// <summary>Decodes the native field order without treating the limit as an error code.</summary>
    public static RoadsidePurchaseRejectedServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongId buyer = stream.ReadLongId();
        LongId owner = stream.ReadLongId();
        int item = stream.ReadInt32();
        int slot = stream.ReadInt32();
        int quantity = stream.ReadInt32();
        int price = stream.ReadInt32();
        int limit = stream.ReadVarInt();


        return new(buyer, owner, item, slot, quantity, price, limit);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongId(BuyerId);
        stream.WriteLongId(ShopOwnerId);
        stream.WriteInt32(ItemGlobalId);
        stream.WriteInt32(SlotIndex);
        stream.WriteInt32(Quantity);
        stream.WriteInt32(Price);
        stream.WriteVarInt(DailyCollectionToolLimit);
    }
}
