using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>Contains the data names selected for a player's event bonuses.</summary>
public sealed record ChronosEventBonuses
{
    /// <summary>Gets the selected data names in their native order.</summary>
    [JsonPropertyName("bonuses")]
    public string[] Bonuses { get; init; } = [];
}
