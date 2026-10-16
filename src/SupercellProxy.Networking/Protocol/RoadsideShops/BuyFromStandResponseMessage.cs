using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Clientbound answer to a roadside purchase, also sent to every visitor of the shop when a stand gets a buyer; a visitor's copy
/// of the stand records the buyer.
/// </summary>
/// <param name="BuyerId">Home of the player who bought the stand; null when nobody did.</param>
/// <param name="HomeOwnerId">Home that owns the shop.</param>
/// <param name="Status">
/// Outcome: <see cref="SuccessfulStatus"/> when the purchase went through, <see cref="BuyerUpdateStatus"/> when the message only
/// reports that the stand got a buyer; other values are refusals whose meanings are not identified.
/// </param>
/// <param name="Quantity">How many goods the stand holds.</param>
/// <param name="Price">Coin price of the whole stand.</param>
/// <param name="SlotIndex">Stand position in the shop.</param>
/// <param name="ContextValue">Extra number the server sends; the client does not read it and its meaning is not identified.</param>
/// <param name="ItemGlobalId">Global id of the goods on the stand.</param>
public sealed record BuyFromStandResponseMessage(
    [property: JsonPropertyName("BuyerId")] LongId? BuyerId,
    [property: JsonPropertyName("HomeOwnerId")] LongId? HomeOwnerId,
    int Status,
    int Quantity,
    int Price,
    int SlotIndex,
    int ContextValue,
    [property: JsonPropertyName("ItemGlobalId")] int ItemGlobalId
) : CodecMessage<BuyFromStandResponseMessage>, ICodecProvider<BuyFromStandResponseMessage>
{
    /// <summary>Status of a message that only reports that a stand got a buyer, sent to the shop's visitors.</summary>
    public const int BuyerUpdateStatus = 3;

    /// <summary>Status of a purchase that went through.</summary>
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
