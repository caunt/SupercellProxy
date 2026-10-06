using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>Identifies one pending booster.</summary>
public sealed record PendingBoosterSnapshot
{
    /// <summary>Gets the booster's data id.</summary>
    [JsonPropertyName("id")]
    public int BoosterDataGlobalId { get; init; }
}
