using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Workbench;

/// <summary>Buys a building mastery level with coins.</summary>
/// <param name="BuildingGlobalId">The building data id.</param>
public sealed record BuyBuildingMasteryCommand(int BuildingGlobalId) : CodecCommand<BuyBuildingMasteryCommand>, ICommandCodecProvider<BuyBuildingMasteryCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuyBuildingMasteryCommand> Codec { get; } = Codecs.Command(
        type: 563,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.BuildingGlobalId,
            static (buildingGlobalId) => new BuyBuildingMasteryCommand(buildingGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
