using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>What one movie ticket reward gives and how likely it is to be drawn.</summary>
public sealed record MovieTicketRewardValue
{
    /// <summary>How many of the reward are given.</summary>
    [JsonPropertyName("Amount")]
    public int Amount { get; init; }

    /// <summary>Name of the rewarded table entry, such as a goods or decoration entry (saved as <c>data</c>).</summary>
    [JsonPropertyName("data")]
    public string? Data { get; init; }

    /// <summary>Weight of this reward when rewards are drawn at random from a pool.</summary>
    [JsonPropertyName("Probability")]
    public int Probability { get; init; }
}
