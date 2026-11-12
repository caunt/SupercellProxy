using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>A Farm Pass perk the player claimed, and whether and where it was put to use.</summary>
public sealed record FarmPassPerkSnapshot
{
    /// <summary>Gets or sets whether the player activated the perk.</summary>
    /// <value><see langword="true"/> if the perk activate command was applied to it; otherwise, <see langword="false"/>.</value>
    public bool Active { get; init; }

    /// <summary>Gets or sets what the perk was activated on, such as a building's global id; 0 for perks that apply to the whole farm.</summary>
    /// <value>The parameter global id the activate command sent, saved as <c>Param1</c>; for an extra-production-slot perk it is the processing building that gets the slots.</value>
    [JsonPropertyName("Param1")]
    public int Parameter1 { get; init; }

    /// <summary>Gets or sets the global id of the perk's table entry.</summary>
    /// <value>A row of the Farm Pass perks table, whose <c>Type</c> column decides what the perk does.</value>
    [JsonPropertyName("PerkDataId")]
    public int PerkDataId { get; init; }

    /// <summary>Gets or sets the multiplier saved with the perk; its use is not identified.</summary>
    /// <value>An integer carried through unchanged; defaults to 1 when the save omits it.</value>
    public int PremiumMultiple { get; init; } = 1;

    /// <summary>Gets or sets whether the perk is marked as coming from a subscription.</summary>
    /// <value><see langword="true"/> if the save marks the perk as a subscription perk; otherwise, <see langword="false"/>.</value>
    /// <remarks>The flag defaults to <see langword="false"/>.</remarks>
    public bool SubscriptionPerk { get; init; }
}
