using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Tutorials;

/// <summary>
/// Saved tutorial state: which tutorial group the farm started in and which of its steps are done.
/// </summary>
public sealed record TutorialManagerSnapshot
{
    /// <summary>
    /// Tutorial group the farm was created with (saved as <c>tutorialGroupId</c>); it picks the farm's starter amounts.
    /// </summary>
    [JsonPropertyName("tutorialGroupId")]
    public int GroupGlobalId { get; init; }

    /// <summary>
    /// Bit set of tutorial milestones reached, such as the truck being unlocked and the first-load setup being done (saved as
    /// <c>tutorialProgressFlags</c>).
    /// </summary>
    [JsonPropertyName("tutorialProgressFlags")]
    public int ProgressFlags { get; init; }
}
