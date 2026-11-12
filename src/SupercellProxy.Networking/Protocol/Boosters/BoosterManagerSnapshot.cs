namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>Represents the decoded <c>BoosterManager</c> avatar-data document.</summary>
public sealed record BoosterManagerSnapshot
{
    /// <summary>Gets or sets the currently active Fresh Beats.</summary>
    /// <value>The running Fresh Beats with their countdowns, or <see langword="null"/> when the save has no <c>BeatList</c> object.</value>
    public BoosterListSnapshot? BeatList { get; init; }

    /// <summary>Gets or sets the Fresh Beats retained for later activation.</summary>
    /// <value>The stored Fresh Beat slots, or <see langword="null"/> when the save has no <c>BeatStorage</c> object.</value>
    public BoosterStorageSnapshot? BeatStorage { get; init; }

    /// <summary>Gets or sets the currently active boosters.</summary>
    /// <value>The running boosters with their countdowns, or <see langword="null"/> when the save has no <c>BoosterList</c> object.</value>
    public BoosterListSnapshot? BoosterList { get; init; }

    /// <summary>Gets or sets the boosters the player owns but has not activated.</summary>
    /// <value>The stored booster slots, or <see langword="null"/> when the save has no <c>BoosterStorage</c> object.</value>
    public BoosterStorageSnapshot? BoosterStorage { get; init; }

    /// <summary>Gets or sets the newly received boosters awaiting a storage decision.</summary>
    /// <value>The pending boosters the player must keep, swap or discard, or <see langword="null"/> when the save has no <c>PendingBoosters</c> object.</value>
    public PendingBoosterStorageSnapshot? PendingBoosters { get; init; }
}
