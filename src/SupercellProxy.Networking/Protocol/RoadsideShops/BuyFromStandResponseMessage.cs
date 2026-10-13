using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

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
) : CodecMessage<BuyFromStandResponseMessage>, ICodecProvider<BuyFromStandResponseMessage>
{
    /// <summary>
    /// Provides the Buyer Update Status value or operation.
    /// </summary>
    public const int BuyerUpdateStatus = 3;

    /// <summary>
    /// Provides the Successful Status value or operation.
    /// </summary>
    public const int SuccessfulStatus = 0;

    /// <inheritdoc />
    public static ICodec<BuyFromStandResponseMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.OptionalLongId,
            static message => message.BuyerId,
            Codecs.OptionalLongId,
            static message => message.HomeOwnerId,
            Codecs.VarInt,
            static message => message.Status,
            Codecs.VarInt,
            static message => message.Quantity,
            Codecs.VarInt,
            static message => message.Price,
            Codecs.VarInt,
            static message => message.SlotIndex,
            Codecs.VarInt,
            static message => message.ContextValue,
            Codecs.VarInt,
            static message => message.ItemGlobalId,
            static (buyerId, homeOwnerId, status, quantity, price, slotIndex, contextValue, itemGlobalId) => new BuyFromStandResponseMessage(buyerId, homeOwnerId, status, quantity, price, slotIndex, contextValue, itemGlobalId)
        ),
        trailingDataMessage: "Roadside purchase result has trailing data."
    );
}
