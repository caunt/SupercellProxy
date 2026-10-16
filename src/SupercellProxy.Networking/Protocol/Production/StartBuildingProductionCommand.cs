using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Production;

/// <summary>
/// Serverbound command that queues one product in a production building, taking its ingredients from storage. Native first
/// collects one finished product from the building when there is one.
/// </summary>
/// <param name="BuildingGlobalId">Game object id of the production building.</param>
/// <param name="ProductGlobalId">Global id of the product to make, such as an entry of the bakery goods table.</param>
public sealed record StartBuildingProductionCommand(
    [property: JsonPropertyName("BuildingGlobalId")] int BuildingGlobalId,
    [property: JsonPropertyName("ProductGlobalId")] int ProductGlobalId
) : CodecCommand<StartBuildingProductionCommand>, ICommandCodecProvider<StartBuildingProductionCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StartBuildingProductionCommand> Codec { get; } = Codecs.Command(
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
