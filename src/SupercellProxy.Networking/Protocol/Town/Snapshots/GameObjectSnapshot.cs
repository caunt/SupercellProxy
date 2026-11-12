using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects;

public sealed partial record GameObjectSnapshot
{
    /// <summary>Gets or sets the passenger spawner's Farm Pass service-help requests on that day.</summary>
    /// <value>How many Farm Pass help requests (help kind 3) the town made on the day in <see cref="LastHelpRequestDayIndex"/>; <see langword="null"/> (read as 0) when the save omits it.</value>
    [JsonPropertyName("LastFarmPassHelpRequestCount")]
    public int? LastFarmPassHelpRequestCount { get; init; }

    /// <summary>Gets or sets the passenger spawner's day index of its last service-help request.</summary>
    /// <value>Whole days since the Unix epoch of the server time when help was last requested; the three request counts belong to this day; <see langword="null"/> when the save omits it.</value>
    [JsonPropertyName("LastHelpRequestDayIndex")]
    public int? LastHelpRequestDayIndex { get; init; }

    /// <summary>Gets or sets the passenger spawner's neighbourhood service-help requests on that day.</summary>
    /// <value>How many neighbourhood help requests (help kind 2) the town made on the day in <see cref="LastHelpRequestDayIndex"/>; <see langword="null"/> (read as 0) when the save omits it.</value>
    [JsonPropertyName("LastNeighborhoodHelpRequestCount")]
    public int? LastNeighborhoodHelpRequestCount { get; init; }

    /// <summary>Gets or sets the passenger spawner's town-visitor service-help requests on that day.</summary>
    /// <value>How many town-visitor help requests (help kind 1) the town made on the day in <see cref="LastHelpRequestDayIndex"/>; <see langword="null"/> (read as 0) when the save omits it.</value>
    [JsonPropertyName("LastVisitorHelpRequestCount")]
    public int? LastVisitorHelpRequestCount { get; init; }

    /// <summary>Gets or sets the personal train's zero-based passenger-capacity upgrade.</summary>
    /// <value>Upgrade level saved under <c>Capacity</c> on the personal train object, used as the row index into the train's <c>Capacity</c> column; <see langword="null"/> (level 0) before any upgrade.</value>
    [JsonPropertyName("Capacity")]
    public int? PersonalTrainCapacityLevel { get; init; }

    /// <summary>Gets or sets the personal train's zero-based cooldown upgrade.</summary>
    /// <value>Upgrade level saved under <c>Cooldown</c>, used as the row index into the train's <c>CooldownIntervalSeconds</c> column to shorten the wait between trips; <see langword="null"/> (level 0) before any upgrade.</value>
    [JsonPropertyName("Cooldown")]
    public int? PersonalTrainCooldownLevel { get; init; }
}
