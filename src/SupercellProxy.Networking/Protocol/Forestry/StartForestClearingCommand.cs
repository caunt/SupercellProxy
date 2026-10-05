using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>
/// Defines the Start Forest Clearing Command contract.
/// </summary>
/// <summary>
/// Defines the Forest Global Id contract.
/// </summary>
/// <summary>
/// Defines the Buy Missing Tool contract.
/// </summary>
public sealed record StartForestClearingCommand([property: System.Text.Json.Serialization.JsonPropertyName("ForestGlobalId")] int ForestGlobalId, bool BuyMissingTool) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.StartForestClearingCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static StartForestClearingCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int forest = stream.ReadVarInt();
        bool buyTool = stream.ReadBoolean();

        return new StartForestClearingCommand(forest, buyTool);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(ForestGlobalId);
        stream.WriteBoolean(BuyMissingTool);
    }
}
