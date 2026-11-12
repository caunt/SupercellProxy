using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>Represents the decoded DecorationEventDefinition JSON contract.</summary>
public sealed record DecorationEventDefinition
{
    /// <summary>Gets or sets the length of the phase in which players build and submit their designs; <see langword="null"/> when the event defines none.</summary>
    /// <value>Seconds from the event's start time until the decoration phase ends and voting begins.</value>
    [JsonPropertyName("decorationPhaseDuration")]
    public int? DecorationPhaseDuration { get; init; }

    /// <summary>Gets or sets the number of votes in one voting batch.</summary>
    /// <value>The vote quota of one batch, read from <c>voteBatchSize</c>, or <see langword="null"/> when the definition omits it.</value>
    [JsonPropertyName("voteBatchSize")]
    public int? VoteBatchSize { get; init; }

    /// <summary>Gets or sets the cooldown in seconds after exhausting a voting batch.</summary>
    /// <value>A duration in seconds (saved as <c>votingCoolDown</c>), or <see langword="null"/> when omitted.</value>
    [JsonPropertyName("votingCoolDown")]
    public int? VotingCooldownSeconds { get; init; }
}
