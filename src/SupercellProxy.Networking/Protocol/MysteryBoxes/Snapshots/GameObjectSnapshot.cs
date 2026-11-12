using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects;

public sealed partial record GameObjectSnapshot
{
    /// <summary>Gets or sets the saved mystery-box opening mode: diamonds, ads and diamonds, or ads only.</summary>
    /// <value>
    /// 0 when only diamonds open the box, 1 when an ad or diamonds can, 2 when only an ad can.
    /// <see langword="null"/> (left out of the JSON) on objects that are not mystery boxes and read as 0.
    /// </value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? BoxOpenType { get; init; }

    /// <summary>Gets or sets whether the mystery box was opened through an advertisement.</summary>
    /// <value>
    /// <see langword="true"/> if the box's ad was watched; otherwise,
    /// <see langword="false"/>. <see langword="null"/> (left out of the JSON) reads as <see langword="false"/>.
    /// </value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? WasOpenedWithAds { get; init; }
}
