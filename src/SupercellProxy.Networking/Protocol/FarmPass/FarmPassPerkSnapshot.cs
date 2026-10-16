using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>A Farm Pass perk the player claimed, and whether and where it was put to use.</summary>
public sealed record FarmPassPerkSnapshot
{
    /// <summary>Whether the player activated the perk; claimed perks start inactive.</summary>
    public bool Active { get; init; }

    /// <summary>What the perk was activated on, such as a building's global id; 0 for perks that apply to the whole farm.</summary>
    [JsonPropertyName("Param1")]
    public int Parameter1 { get; init; }

    /// <summary>Global id of the perk's table entry.</summary>
    [JsonPropertyName("PerkDataId")]
    public int PerkDataId { get; init; }

    /// <summary>Multiplier saved with the perk; perks claimed from the pass save 0, and its use is not identified.</summary>
    public int PremiumMultiple { get; init; } = 1;

    /// <summary>Whether the perk is marked as coming from a subscription; the client logic does not read it.</summary>
    public bool SubscriptionPerk { get; init; }
}
