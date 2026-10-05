using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Production;

/// <summary>
/// Defines the Start Building Production Command contract.
/// </summary>
/// <summary>
/// Defines the Building Global Id contract.
/// </summary>
/// <summary>
/// Defines the Product Global Id contract.
/// </summary>
public sealed record StartBuildingProductionCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("BuildingGlobalId")] int BuildingGlobalId,
    [property: System.Text.Json.Serialization.JsonPropertyName("ProductGlobalId")] int ProductGlobalId
) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.StartBuildingProductionCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static StartBuildingProductionCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new StartBuildingProductionCommand(stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(BuildingGlobalId);
        stream.WriteVarInt(ProductGlobalId);
    }
}
