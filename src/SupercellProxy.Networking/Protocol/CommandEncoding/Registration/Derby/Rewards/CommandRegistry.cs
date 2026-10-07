namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

public static partial class CommandRegistry
{
    /// <summary>Acknowledges the derby leaderboard and start presentation.</summary>
    public const int AcknowledgeDerbyEntriesCommandType = 176;
    /// <summary>Claims the previous derby's podium rewards.</summary>
    public const int ClaimDerbyPlacementRewardsCommandType = 508;
    /// <summary>Dismisses unavailable rewards from the previous derby.</summary>
    public const int ResetDerbyRewardsCommandType = 698;
}
