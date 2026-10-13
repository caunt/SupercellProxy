using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Places the selected net on one fishing-area spot.</summary>
public sealed record PlaceFishingNetCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("FishingSpotGlobalId")] int FishingSpotGlobalId,
    [property: System.Text.Json.Serialization.JsonPropertyName("NetGlobalId")] int NetGlobalId
) : CodecCommand<PlaceFishingNetCommand>, ICommandCodecProvider<PlaceFishingNetCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PlaceFishingNetCommand> Codec { get; } = Codecs.Command(
        type: 118,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.FishingSpotGlobalId,
            Codecs.VarInt,
            static command => command.NetGlobalId,
            static (fishingSpotGlobalId, netGlobalId) => new PlaceFishingNetCommand(fishingSpotGlobalId, netGlobalId)
        )
    );
}
