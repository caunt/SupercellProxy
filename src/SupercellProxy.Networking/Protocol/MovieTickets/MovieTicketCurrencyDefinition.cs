using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>Represents the decoded MovieTicketCurrencyDefinition JSON contract.</summary>
public sealed record MovieTicketCurrencyDefinition
{
    /// <summary>Gets the Name value.</summary>
    [JsonPropertyName("SeasonalCurrency")]
    public string? Name { get; init; }

}
