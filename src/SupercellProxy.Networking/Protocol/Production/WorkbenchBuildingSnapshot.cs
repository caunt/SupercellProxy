using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.Production;

/// <summary>Retains mastery for a building data row, shared by its instances.</summary>
public sealed record WorkbenchBuildingSnapshot : ExtensibleDocument
{
    /// <summary>Gets the building data id.</summary>
    [JsonPropertyName("GlobalId")]
    public int DataGlobalId { get; init; }
    /// <summary>Gets the workbench building level.</summary>
    public int Level { get; init; }
    /// <summary>Gets purchased perk states.</summary>
    public WorkbenchPerkSnapshot[] PerkStates { get; init; } = [];
}
