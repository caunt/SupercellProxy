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
    [property: System.Text.Json.Serialization.JsonPropertyName("BuildingGlobalId")] int BuildingGlobalIdentifier,
    [property: System.Text.Json.Serialization.JsonPropertyName("ProductGlobalId")] int ProductGlobalIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
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
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new StartBuildingProductionCommand(stream.ReadVariableInt(), stream.ReadVariableInt(), fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteVariableInt(BuildingGlobalIdentifier);
        stream.WriteVariableInt(ProductGlobalIdentifier);
    }
}
