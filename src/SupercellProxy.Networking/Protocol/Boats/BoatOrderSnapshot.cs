using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>
/// Defines the Boat Order Snapshot contract.
/// </summary>
public sealed record BoatOrderSnapshot
{

    /// <summary>
    /// Gets the Cargo Entries value.
    /// </summary>
    [JsonPropertyName("crate_array")]
    public BoatCargoSnapshot[] CargoEntries { get; init; } = [];
    /// <summary>
    /// Gets the Experience Level value.
    /// </summary>
    [JsonPropertyName("expLevel")]
    public int ExperienceLevel { get; init; }
}
