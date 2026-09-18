namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>Represents the decoded <c>BoosterManager</c> avatar-data document.</summary>
public sealed record BoosterManagerSnapshot
{
    /// <summary>Gets the currently active boosters.</summary>
    public BoosterListSnapshot? BoosterList { get; init; }

    /// <summary>Gets the boosters the player owns but has not activated.</summary>
    public BoosterStorageSnapshot? BoosterStorage { get; init; }
}
