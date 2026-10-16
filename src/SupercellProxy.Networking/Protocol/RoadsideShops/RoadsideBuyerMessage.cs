using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>Tells a seller on their own farm that someone bought one of their roadside stands; the stand records the buyer.</summary>
/// <param name="SlotIndex">Position of the bought stand in the seller's shop.</param>
/// <param name="BuyerId">Home of the player who bought it.</param>
public sealed record RoadsideBuyerMessage(int SlotIndex, [property: JsonPropertyName("BuyerId")] LongId BuyerId) : CodecMessage<RoadsideBuyerMessage>, ICodecProvider<RoadsideBuyerMessage>
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

    /// <summary>Returns only the message name, without the stand or buyer.</summary>
    public override string ToString()
    {
        return nameof(RoadsideBuyerMessage);
    }
}
