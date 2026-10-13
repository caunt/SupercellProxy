using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Defines the Roadside Buyer Message contract.
/// </summary>
/// <summary>
/// Defines the Slot Index contract.
/// </summary>
/// <summary>
/// Defines the Buyer Id contract.
/// </summary>
public sealed record RoadsideBuyerMessage(int SlotIndex, [property: System.Text.Json.Serialization.JsonPropertyName("BuyerId")] LongId BuyerId) : CodecMessage<RoadsideBuyerMessage>, ICodecProvider<RoadsideBuyerMessage>
{
    /// <inheritdoc />
    public static ICodec<RoadsideBuyerMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.VarInt,
            static message => message.SlotIndex,
            Codecs.LongId,
            static message => message.BuyerId,
            static (slotIndex, buyerId) => new RoadsideBuyerMessage(slotIndex, buyerId)
        ),
        trailingDataMessage: "The roadside buyer update has trailing data."
    );

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(RoadsideBuyerMessage);
    }
}
