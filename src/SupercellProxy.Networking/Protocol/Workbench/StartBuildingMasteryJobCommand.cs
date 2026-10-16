using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Workbench;

/// <summary>Starts a workbench building-mastery job.</summary>
/// <param name="JobKind">Kind of mastery job, as native numbers them.</param>
/// <param name="BuildingGlobalId">Global id of the building type being mastered.</param>
/// <param name="WorkbenchObjectGlobalId">Game object id of the workbench doing the job.</param>
public sealed record StartBuildingMasteryJobCommand(int JobKind, int BuildingGlobalId, int WorkbenchObjectGlobalId) : CodecCommand<StartBuildingMasteryJobCommand>, ICommandCodecProvider<StartBuildingMasteryJobCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StartBuildingMasteryJobCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.JobKind,
            Codecs.VarInt,
            static command => command.BuildingGlobalId,
            Codecs.VarInt,
            static command => command.WorkbenchObjectGlobalId,
            static (jobKind, buildingGlobalId, workbenchObjectGlobalId) => new StartBuildingMasteryJobCommand(jobKind, buildingGlobalId, workbenchObjectGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
