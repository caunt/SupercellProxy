using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats.Snapshots;

/// <summary>Retains one difficulty's daily completed-order count.</summary>
public sealed record BoatDifficultyCompletionSnapshot
{
    /// <summary>Gets the difficulty data id.</summary>
    [JsonPropertyName("ID")]
    public int Id { get; init; }
    /// <summary>Gets the completed-order count.</summary>
    public int Value { get; init; }
}
