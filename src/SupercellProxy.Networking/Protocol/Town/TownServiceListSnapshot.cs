using System.Collections.Immutable;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Saved town service-list state.</summary>
public sealed record TownServiceListSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets completed services whose rewards have not been collected.</summary>
    /// <value>Finished services waiting for collection, in the order collect commands index them. Empty by default.</value>
    public ImmutableArray<TownServiceEntrySnapshot> FinishedServices { get; init; } = [];

    /// <summary>Gets or sets saved service entries.</summary>
    /// <value>Services booked or running at the building, in slot order; the slot index is what start, cancel and speed-up commands refer to. Empty by default.</value>
    public ImmutableArray<TownServiceEntrySnapshot> Services { get; init; } = [];
}
