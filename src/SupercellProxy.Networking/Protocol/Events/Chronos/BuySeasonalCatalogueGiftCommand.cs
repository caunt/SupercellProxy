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
public sealed record BuySeasonalCatalogueGiftCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("EventId")] int EventIdentifier,
    int GiftIndex,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
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
        int eventIdentifier = stream.ReadVariableInt();
        int giftIndex = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new BuySeasonalCatalogueGiftCommand(eventIdentifier, giftIndex, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(EventIdentifier);
        stream.WriteVariableInt(GiftIndex);
        EncodeCommand(stream, environment);
    }
}
