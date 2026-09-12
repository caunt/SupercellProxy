using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>Represents the decoded MovieTicketEventDefinition JSON contract.</summary>
public sealed record MovieTicketEventDefinition
{
    /// <summary>Gets the AdPlacement value.</summary>
    [JsonPropertyName("AdPlacement")]
    public string? AdPlacement { get; init; }

    /// <summary>Gets the DailyAds value.</summary>
    [JsonPropertyName("DailyAds")]
    public int DailyAds { get; init; }

    /// <summary>Gets the ForcedNonSpenderRewards value.</summary>
    [JsonPropertyName("RewardSetForcedNonSpender")]
    public MovieTicketRewardDefinition[] ForcedNonSpenderRewards { get; init; } = [];

    /// <summary>Gets the MoviesCycle value.</summary>
    [JsonPropertyName("MoviesCycle")]
    public MovieTicketCycleDefinition[] MoviesCycle { get; init; } = [];

    /// <summary>Gets the RandomNonSpenderRewards value.</summary>
    [JsonPropertyName("RewardRandomSetNonSpender")]
    public MovieTicketRewardDefinition[] RandomNonSpenderRewards { get; init; } = [];

    /// <summary>Gets the Requirements value.</summary>
    [JsonPropertyName("requirements")]
    public MovieTicketRequirements? Requirements { get; init; }

    /// <summary>Gets the SeasonalCurrency value.</summary>
    [JsonPropertyName("SeasonalCurrency")]
    public MovieTicketCurrencyDefinition? SeasonalCurrency { get; init; }

    /// <summary>Gets the UseSameCycleForSpender value.</summary>
    [JsonPropertyName("useSameCycleForSpender")]
    public bool UseSameCycleForSpender { get; init; }

    /// <summary>Gets the UseSameRewardSetForSpenders value.</summary>
    [JsonPropertyName("useSameRewardSetForSpenders")]
    public bool UseSameRewardSetForSpenders { get; init; }

}
