using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


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
public sealed record RoadsideBuyerMessage(int SlotIndex, [property: System.Text.Json.Serialization.JsonPropertyName("BuyerId")] LongId BuyerId) : IMessage
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static RoadsideBuyerMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        RoadsideBuyerMessage result = new(stream.ReadVarInt(), stream.ReadLongId());

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The roadside buyer update has trailing data.")
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
    }

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(RoadsideBuyerMessage);
    }
}
