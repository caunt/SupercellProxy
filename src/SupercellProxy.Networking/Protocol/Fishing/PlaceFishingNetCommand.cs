using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Places the selected net on one fishing-area spot.</summary>
/// <param name="FishingSpotGlobalId">Game object global id of the fishing spot, written first as a VarInt.</param>
/// <param name="NetGlobalId">Global id of the net's row in the nets table, written as a VarInt after the spot id.</param>
public sealed record PlaceFishingNetCommand([property: JsonPropertyName("FishingSpotGlobalId")] int FishingSpotGlobalId,
    [property: JsonPropertyName("NetGlobalId")] int NetGlobalId) : CodecCommand<PlaceFishingNetCommand>, ICommandCodecProvider<PlaceFishingNetCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PlaceFishingNetCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.FishingSpotGlobalId,
        Codecs.VarInt,
        static command => command.NetGlobalId,
        static (fishingSpotGlobalId, netGlobalId) => new PlaceFishingNetCommand(fishingSpotGlobalId, netGlobalId)));
}
