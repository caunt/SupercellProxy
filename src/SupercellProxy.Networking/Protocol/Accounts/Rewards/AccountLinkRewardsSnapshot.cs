using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Rewards;

/// <summary>The account-link reward flags saved by the native offer manager.</summary>
public sealed record AccountLinkRewardsSnapshot
{
    /// <summary>Gets or sets whether the Hay Day Pop decoration was already claimed.</summary>
    /// <value><see langword="true"/> if the save has the <c>PopPromoStatueClaimed</c> flag and it is set, so the <see cref="AccountLinkRewardKind.HayDayPop"/> reward was already claimed; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("PopPromoStatueClaimed")]
    public bool HayDayPopRewardClaimed { get; init; }

    /// <summary>Gets or sets whether the Supercell ID connection rewards were already claimed.</summary>
    /// <value><see langword="true"/> if the save has the <c>SCIDStatueClaimed</c> flag and it is set, so the <see cref="AccountLinkRewardKind.SupercellId"/> rewards were already given; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("SCIDStatueClaimed")]
    public bool SupercellIdRewardClaimed { get; init; }
}
