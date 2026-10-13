using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Workbench;

/// <summary>Starts a workbench building-mastery job.</summary>
/// <param name="JobKind">The job kind.</param>
/// <param name="BuildingGlobalId">The building data id.</param>
/// <param name="WorkbenchObjectGlobalId">The workbench object id.</param>
public sealed record StartBuildingMasteryJobCommand(int JobKind, int BuildingGlobalId, int WorkbenchObjectGlobalId) : CodecCommand<StartBuildingMasteryJobCommand>, ICommandCodecProvider<StartBuildingMasteryJobCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StartBuildingMasteryJobCommand> Codec { get; } = Codecs.Command(
        type: 521,
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
