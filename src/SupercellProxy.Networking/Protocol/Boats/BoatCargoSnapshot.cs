using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// Defines the Boat Cargo Snapshot contract.
/// </summary>
public sealed record BoatCargoSnapshot
{

    /// <summary>
    /// Gets the Amount value.
    /// </summary>
    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    /// <summary>
    /// Gets the Boosted value.
    /// </summary>
    [JsonPropertyName("boosted")]
    public bool Boosted { get; init; }
    /// <summary>
    /// Gets the Completed value.
    /// </summary>
    [JsonPropertyName("completed")]
    public bool Completed { get; init; }

    /// <summary>
    /// Gets the Data Global Id value.
    /// </summary>
    [JsonPropertyName("data_global_id")]
    public int DataGlobalIdentifier { get; init; }

    /// <summary>
    /// Gets the Gifted value.
    /// </summary>
    [JsonPropertyName("gifted")]
    public bool Gifted { get; init; }
}
