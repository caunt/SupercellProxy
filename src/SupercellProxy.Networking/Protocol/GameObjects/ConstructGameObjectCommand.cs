using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// Defines the Construct Game Object Command contract.
/// </summary>
/// <summary>
/// Defines the Position X contract.
/// </summary>
/// <summary>
/// Defines the Variant contract.
/// </summary>
/// <summary>
/// Defines the Position Y contract.
/// </summary>
/// <summary>
/// Defines the Layout Mode contract.
/// </summary>
/// <summary>
/// Defines the Replaced Object Global Id contract.
/// </summary>
/// <summary>
/// Defines the Target Data Global Id contract.
/// </summary>
/// <summary>
/// Defines the Mirrored contract.
/// </summary>
public sealed record ConstructGameObjectCommand(
    int PositionX,
    int Variant,
    int PositionY,
    bool LayoutMode,
    [property: System.Text.Json.Serialization.JsonPropertyName("ReplacedObjectGlobalId")] int ReplacedObjectGlobalIdentifier,
    [property: System.Text.Json.Serialization.JsonPropertyName("TargetDataGlobalId")] int TargetDataGlobalIdentifier,
    bool Mirrored,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.ConstructGameObjectCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static ConstructGameObjectCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new ConstructGameObjectCommand(
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadBoolean(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadBoolean(),
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
        stream.WriteVariableInt(PositionX);
        stream.WriteVariableInt(Variant);
        stream.WriteVariableInt(PositionY);
        stream.WriteBoolean(LayoutMode);
        stream.WriteVariableInt(ReplacedObjectGlobalIdentifier);
        stream.WriteVariableInt(TargetDataGlobalIdentifier);
        stream.WriteBoolean(Mirrored);
    }
}
