using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>Defines a global experience-reward event.</summary>
public sealed record ExperienceRewardEventDefinition
{
    /// <summary>Gets the native shop-event type for the global experience modifier.</summary>
    public const int EventType = 35;

    /// <summary>Gets the experience percentage, including the ordinary reward.</summary>
    [JsonPropertyName("bonusXP")]
    public int Percentage { get; init; } = 100;
}
