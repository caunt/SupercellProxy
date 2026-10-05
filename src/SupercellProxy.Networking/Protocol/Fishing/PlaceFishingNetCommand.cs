using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Places the selected net on one fishing-area spot.</summary>
public sealed record PlaceFishingNetCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("FishingSpotGlobalId")] int FishingSpotGlobalId,
    [property: System.Text.Json.Serialization.JsonPropertyName("NetGlobalId")] int NetGlobalId
) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.PlaceFishingNetCommandType;

    /// <summary>Decodes a value from the supplied protocol payload.</summary>
    public static PlaceFishingNetCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int fishingSpotGlobalId = stream.ReadVarInt();
        int netGlobalId = stream.ReadVarInt();

        return new PlaceFishingNetCommand(fishingSpotGlobalId, netGlobalId);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(FishingSpotGlobalId);
        stream.WriteVarInt(NetGlobalId);
    }
}
