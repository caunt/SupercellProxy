using SupercellProxy.Playground.Supercell;

namespace SupercellProxy.Playground.Network.Captures;

public sealed record RoadsideShopSlotCapture(
    int Slot,
    LogicLong? BuyerId,
    string? BuyerTag,
    bool IsAdvertised,
    int Price,
    int Quantity,
    int ItemGlobalId,
    int? DataTableId,
    int? DataRowIndex,
    string? ItemName,
    string? ItemDataFile);
