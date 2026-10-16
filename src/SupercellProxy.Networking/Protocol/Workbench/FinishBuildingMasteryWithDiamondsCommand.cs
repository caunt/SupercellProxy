using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Workbench;

/// <summary>Pays diamonds to finish a building mastery job.</summary>
/// <param name="BuildingGlobalId">The building data id.</param>
public sealed record FinishBuildingMasteryWithDiamondsCommand(int BuildingGlobalId) : CodecCommand<FinishBuildingMasteryWithDiamondsCommand>, ICommandCodecProvider<FinishBuildingMasteryWithDiamondsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<FinishBuildingMasteryWithDiamondsCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.BuildingGlobalId,
            static buildingGlobalId => new FinishBuildingMasteryWithDiamondsCommand(buildingGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
