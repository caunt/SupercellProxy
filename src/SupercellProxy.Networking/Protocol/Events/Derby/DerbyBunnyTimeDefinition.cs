using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Describes the native appearance and catch-window timings of one Derby bunny.</summary>
public sealed record DerbyBunnyTimeDefinition
{
    /// <summary>Gets the interval between catch windows.</summary>
    [JsonPropertyName("bunnyTimeCooldownSeconds")]
    public int CooldownSeconds { get; init; }

    /// <summary>Gets the appearance time as a percentage of the event duration.</summary>
    [JsonPropertyName("spawnTimePercentage")]
    public int SpawnTimePercentage { get; init; }

    /// <summary>Gets the delay before the first catch window.</summary>
    [JsonPropertyName("bunnyTimeStartDelaySeconds")]
    public int StartDelaySeconds { get; init; }

    /// <summary>Gets the initial bunny points.</summary>
    [JsonPropertyName("startPoints")]
    public int StartPoints { get; init; }

    /// <summary>Gets the catch-window duration.</summary>
    [JsonPropertyName("bunnyTimeSeconds")]
    public int WindowSeconds { get; init; }
}
