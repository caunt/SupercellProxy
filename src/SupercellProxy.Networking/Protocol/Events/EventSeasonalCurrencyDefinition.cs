using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>Identifies the seasonal currency configured by an event.</summary>
public sealed record EventSeasonalCurrencyDefinition
{
    /// <summary>Gets the Name value.</summary>
    [JsonPropertyName("SeasonalCurrency")]
    public string? Name { get; init; }
}
