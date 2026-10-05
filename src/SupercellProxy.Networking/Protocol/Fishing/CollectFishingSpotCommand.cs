using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Collects one fish from a fishing spot.</summary>
public sealed record CollectFishingSpotCommand([property: System.Text.Json.Serialization.JsonPropertyName("FishingSpotGlobalId")] int FishingSpotGlobalId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CollectFishingSpotCommandType;

    /// <summary>Decodes a value from the supplied protocol payload.</summary>
    public static CollectFishingSpotCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int fishingSpotGlobalId = stream.ReadVarInt();

        return new CollectFishingSpotCommand(fishingSpotGlobalId);
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(FishingSpotGlobalId);
    }
}
