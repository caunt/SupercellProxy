namespace SupercellProxy.Playground.Network.Captures;

public sealed record RoadsideShopSlotCapture(
    int Slot,
    bool IsEmpty,
    string? BuyerTag,
    bool IsAdvertised,
    int Price,
    int Quantity,
    int ItemGlobalId,
    string? ItemName,
    int? DataTableId,
    int? DataRowIndex,
    string? ItemDataFile);
