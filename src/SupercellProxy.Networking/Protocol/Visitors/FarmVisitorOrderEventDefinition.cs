using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Visitors;

/// <summary>The farm-visitor payment percentages a running type-4 event sets.</summary>
public sealed record FarmVisitorOrderEventDefinition
{
    /// <summary>The live event type whose definitions scale what farm visitors pay.</summary>
    public const int EventType = 4;

    /// <summary>Gets the percentage of a visitor's coins paid; 100 leaves them unchanged.</summary>
    [JsonPropertyName("bonusCoins")]
    public int CoinPercentage { get; init; } = 100;

    /// <summary>Gets the percentage of a visitor's experience paid; 100 leaves it unchanged.</summary>
    [JsonPropertyName("bonusXP")]
    public int ExperiencePercentage { get; init; } = 100;
}
