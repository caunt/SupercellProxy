using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Production;

/// <summary>
/// Defines the Collect Building Product Command contract.
/// </summary>
/// <summary>
/// Defines the Building Global Id contract.
/// </summary>
public sealed record CollectBuildingProductCommand([property: System.Text.Json.Serialization.JsonPropertyName("BuildingGlobalId")] int BuildingGlobalId) : CodecCommand<CollectBuildingProductCommand>, ICommandCodecProvider<CollectBuildingProductCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectBuildingProductCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.BuildingGlobalId,
            static (buildingGlobalId) => new CollectBuildingProductCommand(buildingGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
