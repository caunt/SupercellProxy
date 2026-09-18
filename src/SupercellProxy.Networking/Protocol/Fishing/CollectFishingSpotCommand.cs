using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Collects one fish from a fishing spot.</summary>
public sealed record CollectFishingSpotCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("FishingSpotGlobalId")] int FishingSpotGlobalIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CollectFishingSpotCommandType;

    /// <summary>Decodes a value from the supplied protocol payload.</summary>
    public static CollectFishingSpotCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int fishingSpotGlobalIdentifier = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new CollectFishingSpotCommand(fishingSpotGlobalIdentifier, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(FishingSpotGlobalIdentifier);
        EncodeCommand(stream, environment);
    }
}
