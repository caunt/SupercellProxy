using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Rewards;

/// <summary>The account-link reward flags saved by the native offer manager.</summary>
public sealed record AccountLinkRewardsSnapshot
{
    /// <summary>Gets whether the Hay Day Pop decoration was already claimed.</summary>
    [JsonPropertyName("PopPromoStatueClaimed")]
    public bool HayDayPopRewardClaimed { get; init; }

    /// <summary>Gets whether the Supercell ID connection rewards were already claimed.</summary>
    [JsonPropertyName("SCIDStatueClaimed")]
    public bool SupercellIdRewardClaimed { get; init; }
}
