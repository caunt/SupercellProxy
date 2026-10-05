using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Saved town service-list state.</summary>
public sealed record TownServiceListSnapshot : ExtensibleDocument
{
    /// <summary>Gets saved service entries.</summary>
    public TownServiceEntrySnapshot[] Services { get; init; } = [];

    /// <summary>Gets completed services whose rewards have not been collected.</summary>
    public TownServiceEntrySnapshot[] FinishedServices { get; init; } = [];
}
