using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>One possible movie ticket reward; <see cref="Type"/> says which of the reward fields is filled.</summary>
public sealed record MovieTicketRewardDefinition
{
    /// <summary>Decoration reward, used when <see cref="Type"/> is 1: a decoration, fence, ditch or modular decoration.</summary>
    [JsonPropertyName("Decorations")]
    public MovieTicketRewardValue? Decorations { get; init; }

    /// <summary>Coin or diamond reward, used when <see cref="Type"/> is 0; coins and diamonds are granted as their free variants.</summary>
    [JsonPropertyName("Money")]
    public MovieTicketRewardValue? Money { get; init; }

    /// <summary>Goods reward, used when <see cref="Type"/> is 2.</summary>
    [JsonPropertyName("Products")]
    public MovieTicketRewardValue? Products { get; init; }

    /// <summary>Seasonal currency reward, used when <see cref="Type"/> is 4.</summary>
    [JsonPropertyName("SeasonalCurrency")]
    public MovieTicketRewardValue? SeasonalCurrency { get; init; }

    /// <summary>Which of the reward kinds applies, as text: 0 money, 1 decorations, 2 goods, 4 seasonal currency.</summary>
    [JsonPropertyName("Type")]
    public string? Type { get; init; }
}
