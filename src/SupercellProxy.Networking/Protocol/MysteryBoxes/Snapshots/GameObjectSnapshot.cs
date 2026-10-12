using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects;

public sealed partial record GameObjectSnapshot
{
    /// <summary>Gets the saved mystery-box opening mode: diamonds, ads and diamonds, or ads only.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? BoxOpenType { get; init; }

    /// <summary>Gets whether the mystery box was opened through an advertisement.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? WasOpenedWithAds { get; init; }
}
