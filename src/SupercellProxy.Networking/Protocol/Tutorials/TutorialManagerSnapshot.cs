using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Tutorials;

/// <summary>
/// Saved tutorial state: which tutorial group the farm started in and which of its steps are done.
/// </summary>
public sealed record TutorialManagerSnapshot
{
    /// <summary>
    /// Gets or sets the tutorial group the farm was created with (saved as <c>tutorialGroupId</c>).
    /// </summary>
    /// <value>Data global id of a tutorial groups row; 0 when the save omits it.</value>
    [JsonPropertyName("tutorialGroupId")]
    public int GroupGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the bit set of tutorial milestones reached, such as the truck being unlocked and the first-load setup being done (saved as
    /// <c>tutorialProgressFlags</c>).
    /// </summary>
    /// <value>Bit 5 (32) marks the one-time starter setup as done and bit 6 (64) the truck as unlocked; other bits mark further milestones. 0 when the save omits it.</value>
    [JsonPropertyName("tutorialProgressFlags")]
    public int ProgressFlags { get; init; }
}
