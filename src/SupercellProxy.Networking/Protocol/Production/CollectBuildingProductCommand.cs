using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Production;

/// <summary>
/// Defines the Collect Building Product Command contract.
/// </summary>
/// <summary>
/// Defines the Building Global Id contract.
/// </summary>
public sealed record CollectBuildingProductCommand([property: System.Text.Json.Serialization.JsonPropertyName("BuildingGlobalId")] int BuildingGlobalId) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.CollectBuildingProductCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CollectBuildingProductCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new CollectBuildingProductCommand(stream.ReadVarInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(BuildingGlobalId);
    }
}
