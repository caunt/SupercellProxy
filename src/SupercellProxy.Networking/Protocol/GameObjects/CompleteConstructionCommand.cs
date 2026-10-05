using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// Defines the Complete Construction Command contract.
/// </summary>
/// <summary>
/// Defines the Construction Global Id contract.
/// </summary>
public sealed record CompleteConstructionCommand([property: System.Text.Json.Serialization.JsonPropertyName("ConstructionGlobalId")] int ConstructionGlobalId) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.CompleteConstructionCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CompleteConstructionCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int id = stream.ReadVarInt();

        return new CompleteConstructionCommand(id);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(ConstructionGlobalId);
    }
}
