using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>Identifies one pending booster.</summary>
public sealed record PendingBoosterSnapshot
{
    /// <summary>Gets or sets the booster's data id.</summary>
    /// <value>The global id of the received booster's row in the boosters table, saved as <c>id</c>.</value>
    [JsonPropertyName("id")]
    public int BoosterDataGlobalId { get; init; }
}
