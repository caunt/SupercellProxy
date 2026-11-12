using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.Production;

/// <summary>Retains workbench building levels and installed perks used by order rewards.</summary>
public sealed record WorkbenchSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the building mastery states.</summary>
    /// <value>One entry per production building type that has mastery progress; empty by default.</value>
    public WorkbenchBuildingSnapshot[] BuildingStates { get; init; } = [];
    /// <summary>Gets or sets the pending queue-perk migration flag.</summary>
    /// <value><see langword="true"/> if the save marks the queue time-cut perk migration as not done; otherwise, <see langword="false"/>.</value>
    /// <remarks>The flag is carried through unchanged.</remarks>
    public bool QueuePerkTimeCutNotDone { get; init; }
}
