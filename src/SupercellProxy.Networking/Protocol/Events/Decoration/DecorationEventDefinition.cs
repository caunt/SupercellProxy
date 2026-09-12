using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>Represents the decoded DecorationEventDefinition JSON contract.</summary>
public sealed record DecorationEventDefinition
{
    /// <summary>Gets the DecorationPhaseDuration value.</summary>
    [JsonPropertyName("decorationPhaseDuration")]
    public int? DecorationPhaseDuration { get; init; }

}
