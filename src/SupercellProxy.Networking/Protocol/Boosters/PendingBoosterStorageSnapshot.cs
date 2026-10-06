namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>Newly received boosters awaiting a keep, swap, or discard decision.</summary>
public sealed record PendingBoosterStorageSnapshot
{
    /// <summary>Gets pending boosters in native insertion order.</summary>
    public PendingBoosterSnapshot[] PendingBoosterSlots { get; init; } = [];
}
