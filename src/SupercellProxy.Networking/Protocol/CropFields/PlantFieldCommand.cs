using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CropFields;

/// <summary>
/// Defines the Plant Field Command contract.
/// </summary>
/// <summary>
/// Defines the Crop Global Id contract.
/// </summary>
/// <summary>
/// Defines the Buy Seeds contract.
/// </summary>
/// <summary>
/// Defines the Field Global Id contract.
/// </summary>
public sealed record PlantFieldCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("CropGlobalId")] int CropGlobalIdentifier,
    bool BuySeeds,
    [property: System.Text.Json.Serialization.JsonPropertyName("FieldGlobalId")] int FieldGlobalIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.PlantFieldCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static PlantFieldCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new PlantFieldCommand(
            stream.ReadVariableInt(),
            stream.ReadBoolean(),
            stream.ReadVariableInt(),
            fields.ExecutionPhaseCounter,
            fields.DebugData0,
            fields.DebugData1
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteVariableInt(CropGlobalIdentifier);
        stream.WriteBoolean(BuySeeds);
        stream.WriteVariableInt(FieldGlobalIdentifier);
    }
}
