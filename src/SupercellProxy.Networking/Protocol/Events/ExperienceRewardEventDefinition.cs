using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>Defines a global experience-reward event.</summary>
public sealed record ExperienceRewardEventDefinition
{
    /// <summary>Gets the native shop-event type for the global experience modifier.</summary>
    public const int EventType = 35;

    /// <summary>Gets or sets the experience percentage, including the ordinary reward.</summary>
    /// <value>The <c>bonusXP</c> percentage of the normal experience granted while the event runs, so 150 pays one and a half times the usual amount; 100 by default, which leaves experience unchanged.</value>
    [JsonPropertyName("bonusXP")]
    public int Percentage { get; init; } = 100;
}
