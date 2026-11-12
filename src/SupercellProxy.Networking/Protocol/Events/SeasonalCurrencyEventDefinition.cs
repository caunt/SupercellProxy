using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>Describes an event's seasonal currency and its eligibility requirements.</summary>
public sealed record SeasonalCurrencyEventDefinition
{
    /// <summary>Gets or sets the event's minimum player level.</summary>
    /// <value>The <c>requirements</c> object holding the level a player needs to take part, or <see langword="null"/> when the event sets no requirement.</value>
    [JsonPropertyName("requirements")]
    public EventLevelRequirements? Requirements { get; init; }

    /// <summary>Gets or sets the seasonal currency associated with the event.</summary>
    /// <value>The <c>SeasonalCurrency</c> object naming the currency row the event uses, or <see langword="null"/> when the event configures none.</value>
    [JsonPropertyName("SeasonalCurrency")]
    public EventSeasonalCurrencyDefinition? SeasonalCurrency { get; init; }
}
