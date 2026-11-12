using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Visitors;

/// <summary>The farm-visitor payment percentages a running type-4 event sets.</summary>
public sealed record FarmVisitorOrderEventDefinition
{
    /// <summary>The live event type whose definitions scale what farm visitors pay.</summary>
    public const int EventType = 4;

    /// <summary>Gets or sets the percentage of a visitor's coins paid; 100 leaves them unchanged.</summary>
    /// <value>Percent of the visitor's coin payment that is paid in total, saved as <c>bonusCoins</c>, so 150 adds half again on top of the base payment; 100 is the default.</value>
    [JsonPropertyName("bonusCoins")]
    public int CoinPercentage { get; init; } = 100;

    /// <summary>Gets or sets the percentage of a visitor's experience paid; 100 leaves it unchanged.</summary>
    /// <value>Percent of the visitor's experience reward granted in total, saved as <c>bonusXP</c>; 100 is the default.</value>
    [JsonPropertyName("bonusXP")]
    public int ExperiencePercentage { get; init; } = 100;
}
