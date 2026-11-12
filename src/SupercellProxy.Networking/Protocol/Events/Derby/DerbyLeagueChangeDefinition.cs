using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Describes an explicitly enabled override of ordinary league changes.</summary>
public sealed record DerbyLeagueChangeDefinition
{
    /// <summary>Gets or sets whether demotion is allowed when this override is enabled.</summary>
    /// <value><see langword="true"/> if neighbourhoods can drop a league after this derby; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("demoteEnabled")]
    public bool DemotionAllowed { get; init; }

    /// <summary>Gets or sets whether the event replaces the ordinary league rules.</summary>
    /// <value><see langword="true"/> if <see cref="PromotionAllowed"/> and <see cref="DemotionAllowed"/> apply (saved as <c>overrideEnabled</c>); otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("overrideEnabled")]
    public bool Enabled { get; init; }

    /// <summary>Gets or sets whether promotion is allowed when this override is enabled.</summary>
    /// <value><see langword="true"/> if neighbourhoods can move up a league after this derby; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("promoteEnabled")]
    public bool PromotionAllowed { get; init; }
}
