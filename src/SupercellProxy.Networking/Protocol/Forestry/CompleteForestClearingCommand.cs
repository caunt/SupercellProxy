using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>
/// Defines the Complete Forest Clearing Command contract.
/// </summary>
/// <summary>
/// Defines the Forest Global Id contract.
/// </summary>
public sealed record CompleteForestClearingCommand([property: System.Text.Json.Serialization.JsonPropertyName("ForestGlobalId")] int ForestGlobalId) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.CompleteForestClearingCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CompleteForestClearingCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int forest = stream.ReadVarInt();

        return new CompleteForestClearingCommand(forest);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(ForestGlobalId);
    }
}
