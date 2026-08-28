using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c language="csharp">RoadsideShopEntry</c>.
/// </summary>
internal sealed record RoadsideShopEntry(
    LongId? BuyerId,
    bool IsSold,
    int Price,
    int Quantity,
    int ItemGlobalId
)
{
    internal static RoadsideShopEntry Decode(MessageStream stream) =>
        new(
            stream.ReadOptionalLongId(),
            stream.ReadBoolean(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt()
        );

    internal void Encode(MessageStream stream)
    {
        stream.WriteOptionalLongId(BuyerId);
        stream.WriteBoolean(IsSold);
        stream.WriteVarInt(Price);
        stream.WriteVarInt(Quantity);
        stream.WriteVarInt(ItemGlobalId);
    }
}
