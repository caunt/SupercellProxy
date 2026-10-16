using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>Represents the decoded DecorationEventDefinition JSON contract.</summary>
public sealed record DecorationEventDefinition
{
    /// <summary>Length of the phase in which players build and submit their designs; null when the event defines none.</summary>
    [JsonPropertyName("decorationPhaseDuration")]
    public int? DecorationPhaseDuration { get; init; }

    /// <summary>Gets the number of votes in one voting batch.</summary>
    [JsonPropertyName("voteBatchSize")]
    public int? VoteBatchSize { get; init; }

    /// <summary>Gets the cooldown in seconds after exhausting a voting batch.</summary>
    [JsonPropertyName("votingCoolDown")]
    public int? VotingCooldownSeconds { get; init; }
}
