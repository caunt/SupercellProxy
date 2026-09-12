using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Tutorials;

/// <summary>
/// Defines the Tutorial Manager Snapshot contract.
/// </summary>
public sealed record TutorialManagerSnapshot
{
    /// <summary>
    /// Gets the Group Global Id value.
    /// </summary>
    [JsonPropertyName("tutorialGroupId")]
    public int GroupGlobalIdentifier { get; init; }

    /// <summary>
    /// Gets the Progress Flags value.
    /// </summary>
    [JsonPropertyName("tutorialProgressFlags")]
    public int ProgressFlags { get; init; }
}
