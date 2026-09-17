using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>A server refusal containing the attempted listing and the client's material-purchase limit.</summary>
public sealed record RoadsidePurchaseRejectedServerCommand(
    LongIdentifier BuyerIdentifier,
    LongIdentifier ShopOwnerIdentifier,
    int ItemGlobalIdentifier,
    int SlotIndex,
    int Quantity,
    int Price,
    int DailyCollectionToolLimit,
    int ServerCommandIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : ServerCommand(ServerCommandIdentifier, ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.RoadsidePurchaseRejectedServerCommandType;

    /// <summary>Decodes the native field order without treating the limit as an error code.</summary>
    public static RoadsidePurchaseRejectedServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongIdentifier buyer = stream.ReadLongIdentifier();
        LongIdentifier owner = stream.ReadLongIdentifier();
        int item = stream.ReadInt32();
        int slot = stream.ReadInt32();
        int quantity = stream.ReadInt32();
        int price = stream.ReadInt32();
        int limit = stream.ReadVariableInt();
        (int identifier, (int phase, CommandData? debug0, CommandData? debug1) fields) = DecodeServerCommand(stream, environment);

        return new(buyer, owner, item, slot, quantity, price, limit, identifier, fields.phase, fields.debug0, fields.debug1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongIdentifier(BuyerIdentifier);
        stream.WriteLongIdentifier(ShopOwnerIdentifier);
        stream.WriteInt32(ItemGlobalIdentifier);
        stream.WriteInt32(SlotIndex);
        stream.WriteInt32(Quantity);
        stream.WriteInt32(Price);
        stream.WriteVariableInt(DailyCollectionToolLimit);
        EncodeServerCommand(stream, environment);
    }
}
