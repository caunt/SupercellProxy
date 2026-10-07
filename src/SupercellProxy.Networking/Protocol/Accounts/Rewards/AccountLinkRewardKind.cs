namespace SupercellProxy.Networking.Protocol.Accounts.Rewards;

/// <summary>The native one-time account-link reward selection.</summary>
public enum AccountLinkRewardKind
{
    /// <summary>The configured Supercell ID connection rewards.</summary>
    SupercellId = 1,
    /// <summary>The legacy Hay Day Pop promotion decoration.</summary>
    HayDayPop = 2,
}
