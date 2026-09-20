using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Places the selected net on one fishing-area spot.</summary>
public sealed record PlaceFishingNetCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("FishingSpotGlobalId")] int FishingSpotGlobalIdentifier,
    [property: System.Text.Json.Serialization.JsonPropertyName("NetGlobalId")] int NetGlobalIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.PlaceFishingNetCommandType;

    /// <summary>Decodes a value from the supplied protocol payload.</summary>
    public static PlaceFishingNetCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int fishingSpotGlobalIdentifier = stream.ReadVariableInt();
        int netGlobalIdentifier = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new PlaceFishingNetCommand(fishingSpotGlobalIdentifier, netGlobalIdentifier, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(FishingSpotGlobalIdentifier);
        stream.WriteVariableInt(NetGlobalIdentifier);
        EncodeCommand(stream, environment);
    }
}
