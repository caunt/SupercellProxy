using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>Represents the decoded MovieTicketRewardValue JSON contract.</summary>
public sealed record MovieTicketRewardValue
{
    /// <summary>Gets the Amount value.</summary>
    [JsonPropertyName("Amount")]
    public int Amount { get; init; }

    /// <summary>Gets the Data value.</summary>
    [JsonPropertyName("data")]
    public string? Data { get; init; }

    /// <summary>Gets the Probability value.</summary>
    [JsonPropertyName("Probability")]
    public int Probability { get; init; }

}
