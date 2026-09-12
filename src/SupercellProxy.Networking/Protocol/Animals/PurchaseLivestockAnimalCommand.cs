using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Animals;

/// Command 641 places one purchased livestock animal in an existing habitat.
public sealed record PurchaseLivestockAnimalCommand(
    bool Mirrored,
    int PositionY,
    [property: System.Text.Json.Serialization.JsonPropertyName("HabitatGlobalId")] int HabitatGlobalIdentifier,
    [property: System.Text.Json.Serialization.JsonPropertyName("AnimalDataGlobalId")] int AnimalDataGlobalIdentifier,
    int PositionX,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.PurchaseLivestockAnimalCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static PurchaseLivestockAnimalCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new PurchaseLivestockAnimalCommand(
            stream.ReadBoolean(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
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
        stream.WriteBoolean(Mirrored);
        stream.WriteVariableInt(PositionY);
        stream.WriteVariableInt(HabitatGlobalIdentifier);
        stream.WriteVariableInt(AnimalDataGlobalIdentifier);
        stream.WriteVariableInt(PositionX);
    }
}
