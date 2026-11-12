namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>Newly received boosters awaiting a keep, swap, or discard decision.</summary>
public sealed record PendingBoosterStorageSnapshot
{
    /// <summary>Gets or sets the pending boosters in native insertion order.</summary>
    /// <value>The received boosters still waiting for a keep, swap or discard decision, oldest first; empty by default.</value>
    public PendingBoosterSnapshot[] PendingBoosterSlots { get; init; } = [];
}
