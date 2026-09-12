using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Represents decoded CompletedNeighborhoodTaskSnapshot state.</summary>
public sealed record CompletedNeighborhoodTaskSnapshot : ExtensibleDocument
{
    /// <summary>Gets the Complete value.</summary>
    [JsonPropertyName("Complete")]
    public bool Complete { get; init; }

    /// <summary>Gets the PlayerLevelAtTaskStart value.</summary>
    [JsonPropertyName("PlayerLevelAtTaskStart")]
    public int PlayerLevelAtTaskStart { get; init; }

    /// <summary>Gets the Progress value.</summary>
    [JsonPropertyName("Progress")]
    public int Progress { get; init; }

    /// <summary>Gets the Seen value.</summary>
    [JsonPropertyName("Seen")]
    public bool Seen { get; init; }

    /// <summary>Gets the TaskDataIdentifier value.</summary>
    [JsonPropertyName("TaskDataId")]
    public int TaskDataIdentifier { get; init; }

}
