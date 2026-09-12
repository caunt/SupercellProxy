using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>Represents the decoded MovieTicketRewardDefinition JSON contract.</summary>
public sealed record MovieTicketRewardDefinition
{
    /// <summary>Gets the Money value.</summary>
    [JsonPropertyName("Money")]
    public MovieTicketRewardValue? Money { get; init; }

    /// <summary>Gets the Products value.</summary>
    [JsonPropertyName("Products")]
    public MovieTicketRewardValue? Products { get; init; }

    /// <summary>Gets the SeasonalCurrency value.</summary>
    [JsonPropertyName("SeasonalCurrency")]
    public MovieTicketRewardValue? SeasonalCurrency { get; init; }

    /// <summary>Gets the Type value.</summary>
    [JsonPropertyName("Type")]
    public string? Type { get; init; }

}
