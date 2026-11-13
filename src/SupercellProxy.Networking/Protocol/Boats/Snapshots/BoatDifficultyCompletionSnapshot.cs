using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats.Snapshots;

/// <summary>Retains one difficulty's daily completed-order count.</summary>
public sealed record BoatDifficultyCompletionSnapshot
{
    /// <summary>Gets or sets the difficulty data id.</summary>
    /// <value>The global id of the boat difficulty row the count belongs to, saved as <c>ID</c>.</value>
    [JsonPropertyName("ID")]
    public int Id { get; init; }

    /// <summary>Gets or sets the completed-order count.</summary>
    /// <value>The number of orders of this difficulty completed since the last daily reset.</value>
    public int Value { get; init; }
}
