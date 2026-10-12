using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects;

public sealed partial record GameObjectSnapshot
{
    /// <summary>Gets the personal train's zero-based passenger-capacity upgrade.</summary>
    [JsonPropertyName("Capacity")]
    public int? PersonalTrainCapacityLevel { get; init; }

    /// <summary>Gets the personal train's zero-based cooldown upgrade.</summary>
    [JsonPropertyName("Cooldown")]
    public int? PersonalTrainCooldownLevel { get; init; }
}
