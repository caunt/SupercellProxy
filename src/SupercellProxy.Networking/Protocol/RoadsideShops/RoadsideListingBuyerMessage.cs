using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Defines the Roadside Listing Buyer Message contract.
/// </summary>
/// <summary>
/// Defines the Slot Index contract.
/// </summary>
/// <summary>
/// Defines the Buyer Id contract.
/// </summary>
/// <summary>
/// Defines the Home Owner Id contract.
/// </summary>
public sealed record RoadsideListingBuyerMessage(
    int SlotIndex,
    [property: System.Text.Json.Serialization.JsonPropertyName("BuyerId")] LongId BuyerId,
    [property: System.Text.Json.Serialization.JsonPropertyName("HomeOwnerId")] LongId HomeOwnerId
)
    : IMessage
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static RoadsideListingBuyerMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        RoadsideListingBuyerMessage result = new(stream.ReadVarInt(), stream.ReadLongId(), stream.ReadLongId());

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "Roadside listing buyer update has trailing data.")
            : result;
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteVarInt(SlotIndex);
        stream.WriteLongId(BuyerId);
        stream.WriteLongId(HomeOwnerId);
    }
}
