using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

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
) : CodecMessage<RoadsideListingBuyerMessage>, ICodecProvider<RoadsideListingBuyerMessage>
{
    /// <inheritdoc />
    public static ICodec<RoadsideListingBuyerMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.VarInt,
            static message => message.SlotIndex,
            Codecs.LongId,
            static message => message.BuyerId,
            Codecs.LongId,
            static message => message.HomeOwnerId,
            static (slotIndex, buyerId, homeOwnerId) => new RoadsideListingBuyerMessage(slotIndex, buyerId, homeOwnerId)
        ),
        trailingDataMessage: "Roadside listing buyer update has trailing data."
    );
}
