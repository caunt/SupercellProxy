using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Production;

/// <summary>
/// Serverbound command that takes the first finished product out of a production building into storage.
/// </summary>
/// <param name="BuildingGlobalId">Game object id of the production building.</param>
public sealed record CollectBuildingProductCommand([property: JsonPropertyName("BuildingGlobalId")] int BuildingGlobalId) : CodecCommand<CollectBuildingProductCommand>, ICommandCodecProvider<CollectBuildingProductCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectBuildingProductCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.BuildingGlobalId,
            static buildingGlobalId => new CollectBuildingProductCommand(buildingGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
