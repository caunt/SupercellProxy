using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>One possible movie ticket reward; <see cref="Type"/> says which of the reward fields is filled.</summary>
public sealed record MovieTicketRewardDefinition
{
    /// <summary>Gets or sets the decoration reward, used when <see cref="Type"/> is 1: a decoration, fence, ditch or modular decoration.</summary>
    /// <value>The decoration's row name, amount and weight; <see langword="null"/> for other types.</value>
    [JsonPropertyName("Decorations")]
    public MovieTicketRewardValue? Decorations { get; init; }

    /// <summary>Gets or sets the coin or diamond reward, used when <see cref="Type"/> is 0.</summary>
    /// <value>A money row name such as <c>Cash</c> or <c>Diamonds</c>, with its amount and weight; <see langword="null"/> for other types.</value>
    [JsonPropertyName("Money")]
    public MovieTicketRewardValue? Money { get; init; }

    /// <summary>Gets or sets the goods reward, used when <see cref="Type"/> is 2.</summary>
    /// <value>The goods row name, amount and weight; <see langword="null"/> for other types.</value>
    [JsonPropertyName("Products")]
    public MovieTicketRewardValue? Products { get; init; }

    /// <summary>Gets or sets the seasonal currency reward, used when <see cref="Type"/> is 4.</summary>
    /// <value>The seasonal currency row name, amount and weight; <see langword="null"/> for other types.</value>
    [JsonPropertyName("SeasonalCurrency")]
    public MovieTicketRewardValue? SeasonalCurrency { get; init; }

    /// <summary>Gets or sets which of the reward kinds applies, as text: 0 money, 1 decorations, 2 goods, 4 seasonal currency.</summary>
    /// <value>One of the strings <c>"0"</c>, <c>"1"</c>, <c>"2"</c> or <c>"4"</c>; the meaning of any other value, including <see langword="null"/>, is not yet identified.</value>
    [JsonPropertyName("Type")]
    public string? Type { get; init; }
}
