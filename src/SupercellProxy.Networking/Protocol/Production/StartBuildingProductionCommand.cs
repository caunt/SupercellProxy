using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Production;

/// <summary>
/// Defines the Start Building Production Command contract.
/// </summary>
/// <summary>
/// Defines the Building Global Id contract.
/// </summary>
/// <summary>
/// Defines the Product Global Id contract.
/// </summary>
public sealed record StartBuildingProductionCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("BuildingGlobalId")] int BuildingGlobalId,
    [property: System.Text.Json.Serialization.JsonPropertyName("ProductGlobalId")] int ProductGlobalId
) : CodecCommand<StartBuildingProductionCommand>, ICommandCodecProvider<StartBuildingProductionCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StartBuildingProductionCommand> Codec { get; } = Codecs.Command(
        type: 606,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.BuildingGlobalId,
            Codecs.VarInt,
            static command => command.ProductGlobalId,
            static (buildingGlobalId, productGlobalId) => new StartBuildingProductionCommand(buildingGlobalId, productGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
