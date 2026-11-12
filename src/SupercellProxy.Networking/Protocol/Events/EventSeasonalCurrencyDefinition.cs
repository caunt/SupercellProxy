using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>Identifies the seasonal currency configured by an event.</summary>
public sealed record EventSeasonalCurrencyDefinition
{
    /// <summary>Gets or sets the seasonal currencies table entry the event pays and charges in.</summary>
    /// <value>The row name from the seasonal currencies table, read from the <c>SeasonalCurrency</c> key, or <see langword="null"/> when the event has no seasonal currency.</value>
    [JsonPropertyName("SeasonalCurrency")]
    public string? Name { get; init; }
}
