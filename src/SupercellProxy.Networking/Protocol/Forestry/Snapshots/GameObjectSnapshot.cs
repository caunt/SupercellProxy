using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects;

public sealed partial record GameObjectSnapshot
{
    /// <summary>Gets or sets the saved fruit-source revival state.</summary>
    /// <value>Saved as <c>RState</c>: 1 while the exhausted plant waits for a visitor to revive it, 2 once a helper has revived it; <see langword="null"/> (read as 0) when the plant has no revival in progress.</value>
    [JsonPropertyName("RState")]
    public int? PlantRevivalState { get; init; }

    /// <summary>Gets or sets the high component of the farmer who revived this source.</summary>
    /// <value>The high 32 bits of the helper's home id, saved as <c>RH</c>; <see langword="null"/> (read as 0) until someone revives the plant.</value>
    [JsonPropertyName("RH")]
    public int? PlantReviverHigh { get; init; }

    /// <summary>Gets or sets the low component of the farmer who revived this source.</summary>
    /// <value>The low 32 bits of the helper's home id, saved as <c>RL</c>; <see langword="null"/> (read as 0) until someone revives the plant.</value>
    [JsonPropertyName("RL")]
    public int? PlantReviverLow { get; init; }
}
