using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects;

public sealed partial record GameObjectSnapshot
{
    /// <summary>Gets the saved fruit-source revival state.</summary>
    [JsonPropertyName("RState")]
    public int? PlantRevivalState { get; init; }

    /// <summary>Gets the high component of the farmer who revived this source.</summary>
    [JsonPropertyName("RH")]
    public int? PlantReviverHigh { get; init; }

    /// <summary>Gets the low component of the farmer who revived this source.</summary>
    [JsonPropertyName("RL")]
    public int? PlantReviverLow { get; init; }
}
