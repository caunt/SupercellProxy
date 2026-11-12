using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>What one movie ticket reward gives and how likely it is to be drawn.</summary>
public sealed record MovieTicketRewardValue
{
    /// <summary>Gets or sets how many of the reward are given.</summary>
    /// <value>The quantity granted when the video is watched, such as a coin amount or a number of goods.</value>
    [JsonPropertyName("Amount")]
    public int Amount { get; init; }

    /// <summary>Gets or sets the name of the rewarded table entry, such as a goods or decoration entry (saved as <c>data</c>).</summary>
    /// <value>A row name in the table that the reward's type names, or <see langword="null"/> when absent.</value>
    [JsonPropertyName("data")]
    public string? Data { get; init; }

    /// <summary>Gets or sets the weight of this reward when rewards are drawn at random from a pool.</summary>
    /// <value>A relative weight, not a percentage, saved as <c>Probability</c>.</value>
    [JsonPropertyName("Probability")]
    public int Probability { get; init; }
}
