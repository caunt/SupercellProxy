using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Collects one fish from a fishing spot.</summary>
public sealed record CollectFishingSpotCommand([property: System.Text.Json.Serialization.JsonPropertyName("FishingSpotGlobalId")] int FishingSpotGlobalId) : CodecCommand<CollectFishingSpotCommand>, ICommandCodecProvider<CollectFishingSpotCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectFishingSpotCommand> Codec { get; } = Codecs.Command(
        type: 109,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.FishingSpotGlobalId,
            static (fishingSpotGlobalId) => new CollectFishingSpotCommand(fishingSpotGlobalId)
        )
    );
}
