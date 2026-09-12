using SupercellProxy.Networking.Json;

using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Creatures;

/// <summary>
/// Represents decoded <c language="csharp">CreatureManagerSnapshot</c> home data.
/// </summary>
public sealed record CreatureManagerSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">DailyMaxSpawnResetTime</c> value.
    /// </summary>
    [JsonPropertyName("dailyMaxSpawnResetTime")]
    public int DailyMaximumSpawnResetTime { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">FarmVisitingCatchList</c> value.
    /// </summary>
    [JsonPropertyName("farmVisitingCatchList")]
    public EncodedDocumentValue[] FarmVisitingCatchList { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">LastKnownEventId</c> value.
    /// </summary>
    [JsonPropertyName("lastKnownEventId")]
    public int LastKnownEventIdentifier { get; init; }
}
