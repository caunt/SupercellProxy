using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>Contains the data names selected for a player's event bonuses.</summary>
public sealed record ChronosEventBonuses
{
    /// <summary>Gets or sets the selected data names in their native order.</summary>
    /// <value>Row names from the event's bonus table picked for this player; empty by default.</value>
    [JsonPropertyName("bonuses")]
    public ImmutableArray<string> Bonuses { get; init; } = [];
}
