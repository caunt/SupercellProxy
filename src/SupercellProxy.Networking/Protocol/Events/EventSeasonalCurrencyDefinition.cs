using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>Identifies the seasonal currency configured by an event.</summary>
public sealed record EventSeasonalCurrencyDefinition
{
    /// <summary>Seasonal currencies table entry the event pays and charges in (saved as <c>SeasonalCurrency</c>); null when it has none.</summary>
    [JsonPropertyName("SeasonalCurrency")]
    public string? Name { get; init; }
}
