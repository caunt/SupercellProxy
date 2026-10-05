using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>
/// Defines the Buy Seasonal Catalogue Gift Command contract.
/// </summary>
/// <summary>
/// Defines the Event Id contract.
/// </summary>
/// <summary>
/// Defines the Gift Index contract.
/// </summary>
public sealed record BuySeasonalCatalogueGiftCommand([property: System.Text.Json.Serialization.JsonPropertyName("EventId")] int EventId, int GiftIndex) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.BuySeasonalCatalogueGiftCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static BuySeasonalCatalogueGiftCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int eventId = stream.ReadVarInt();
        int giftIndex = stream.ReadVarInt();

        return new BuySeasonalCatalogueGiftCommand(eventId, giftIndex);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(EventId);
        stream.WriteVarInt(GiftIndex);
    }
}
