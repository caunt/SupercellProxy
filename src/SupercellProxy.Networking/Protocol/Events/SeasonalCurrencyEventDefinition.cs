using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>Describes an event's seasonal currency and its eligibility requirements.</summary>
public sealed record SeasonalCurrencyEventDefinition
{
    /// <summary>Gets the event's minimum player level.</summary>
    [JsonPropertyName("requirements")]
    public EventLevelRequirements? Requirements { get; init; }

    /// <summary>Gets the seasonal currency associated with the event.</summary>
    [JsonPropertyName("SeasonalCurrency")]
    public EventSeasonalCurrencyDefinition? SeasonalCurrency { get; init; }
}
