using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.Production;

/// <summary>Retains workbench building levels and installed perks used by order rewards.</summary>
public sealed record WorkbenchSnapshot : ExtensibleDocument
{
    /// <summary>Gets the building mastery states.</summary>
    public WorkbenchBuildingSnapshot[] BuildingStates { get; init; } = [];
    /// <summary>Gets the pending queue-perk migration flag.</summary>
    public bool QueuePerkTimeCutNotDone { get; init; }
}
