using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Describes an explicitly enabled override of ordinary league changes.</summary>
public sealed record DerbyLeagueChangeDefinition
{
    /// <summary>Gets whether demotion is allowed when this override is enabled.</summary>
    [JsonPropertyName("demoteEnabled")]
    public bool DemotionAllowed { get; init; }

    /// <summary>Gets whether the event replaces the ordinary league rules.</summary>
    [JsonPropertyName("overrideEnabled")]
    public bool Enabled { get; init; }

    /// <summary>Gets whether promotion is allowed when this override is enabled.</summary>
    [JsonPropertyName("promoteEnabled")]
    public bool PromotionAllowed { get; init; }
}
