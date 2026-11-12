using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Describes the native appearance and catch-window timings of one Derby bunny.</summary>
public sealed record DerbyBunnyTimeDefinition
{
    /// <summary>Gets or sets the interval between catch windows.</summary>
    /// <value>The wait in seconds between one catch window and the next (saved as <c>bunnyTimeCooldownSeconds</c>).</value>
    [JsonPropertyName("bunnyTimeCooldownSeconds")]
    public int CooldownSeconds { get; init; }

    /// <summary>Gets or sets the appearance time as a percentage of the event duration.</summary>
    /// <value>A percentage from 0 to 100 of the way through the derby at which this bunny appears.</value>
    [JsonPropertyName("spawnTimePercentage")]
    public int SpawnTimePercentage { get; init; }

    /// <summary>Gets or sets the delay before the first catch window.</summary>
    /// <value>Seconds after the bunny appears before its first catch window opens (saved as <c>bunnyTimeStartDelaySeconds</c>).</value>
    [JsonPropertyName("bunnyTimeStartDelaySeconds")]
    public int StartDelaySeconds { get; init; }

    /// <summary>Gets or sets the initial bunny points.</summary>
    /// <value>The points the bunny is worth when it appears.</value>
    [JsonPropertyName("startPoints")]
    public int StartPoints { get; init; }

    /// <summary>Gets or sets the catch-window duration.</summary>
    /// <value>Seconds each catch window stays open (saved as <c>bunnyTimeSeconds</c>).</value>
    [JsonPropertyName("bunnyTimeSeconds")]
    public int WindowSeconds { get; init; }
}
