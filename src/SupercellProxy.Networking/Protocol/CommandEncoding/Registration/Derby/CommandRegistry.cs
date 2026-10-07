namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

public static partial class CommandRegistry
{
    /// <summary>Claims selected derby threshold rewards.</summary>
    public const int ClaimDerbyRewardsCommandType = 173;
    /// <summary>Clears an active farm-side derby task.</summary>
    public const int ClearActiveDerbyTaskCommandType = 615;
    /// <summary>Records seen derby reward thresholds.</summary>
    public const int MarkDerbyRewardsSeenCommandType = 178;
    /// <summary>Records how many flying derby bunnies have been shown.</summary>
    public const int MarkFlyingDerbyBunniesSeenCommandType = 326;
    /// <summary>Rerolls selected derby reward choices for diamonds.</summary>
    public const int RerollDerbyRewardsCommandType = 172;
    /// <summary>Records leaderboard credit or submits the player's completed derby task.</summary>
    public const int ResolveCompletedDerbyTaskCommandType = 169;
    /// <summary>Reports completion or cancellation of a derby task.</summary>
    public const int ResolveDerbyTaskServerCommandType = 171;
    /// <summary>Reports acceptance or rejection of a derby task.</summary>
    public const int StartDerbyTaskServerCommandType = 170;
}
