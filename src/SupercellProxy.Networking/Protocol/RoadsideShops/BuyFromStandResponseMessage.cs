using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Defines the buy from stand response message contract.
/// </summary>
/// <summary>
/// Defines the Buyer Id contract.
/// </summary>
/// <summary>
/// Defines the Home Owner Id contract.
/// </summary>
/// <summary>
/// Defines the Status contract.
/// </summary>
/// <summary>
/// Defines the Quantity contract.
/// </summary>
/// <summary>
/// Defines the Price contract.
/// </summary>
/// <summary>
/// Defines the Slot Index contract.
/// </summary>
/// <summary>
/// Defines the Context Value contract.
/// </summary>
/// <summary>
/// Defines the Item Global Id contract.
/// </summary>
public sealed record BuyFromStandResponseMessage(
    [property: System.Text.Json.Serialization.JsonPropertyName("BuyerId")] LongId? BuyerId,
    [property: System.Text.Json.Serialization.JsonPropertyName("HomeOwnerId")] LongId? HomeOwnerId,
    int Status,
    int Quantity,
    int Price,
    int SlotIndex,
    int ContextValue,
    [property: System.Text.Json.Serialization.JsonPropertyName("ItemGlobalId")] int ItemGlobalId
) : IMessage
{
    /// <summary>
    /// Provides the Buyer Update Status value or operation.
    /// </summary>
    public const int BuyerUpdateStatus = 3;
    /// <summary>
    /// Provides the Successful Status value or operation.
    /// </summary>
    public const int SuccessfulStatus = 0;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static BuyFromStandResponseMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        BuyFromStandResponseMessage message = new(
            stream.ReadOptionalLongId(),
            stream.ReadOptionalLongId(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt()
        );

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "Roadside purchase result has trailing data.")
            : message;
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteOptionalLongId(BuyerId);
        stream.WriteOptionalLongId(HomeOwnerId);
        stream.WriteVarInt(Status);
        stream.WriteVarInt(Quantity);
        stream.WriteVarInt(Price);
        stream.WriteVarInt(SlotIndex);
        stream.WriteVarInt(ContextValue);
        stream.WriteVarInt(ItemGlobalId);
    }
}
