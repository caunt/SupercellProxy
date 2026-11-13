using System.Collections.Immutable;
using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.Production;

/// <summary>Retains mastery for a building data row, shared by its instances.</summary>
public sealed record WorkbenchBuildingSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the building data id.</summary>
    /// <value>Saved as <c>GlobalId</c>; the global id of the production building's data row, so every copy of that building shares this mastery.</value>
    [JsonPropertyName("GlobalId")]
    public int DataGlobalId { get; init; }

    /// <summary>Gets or sets the workbench building level.</summary>
    /// <value>The mastery level reached for the building; each level can hold an installed perk, listed in <see cref="PerkStates"/> with that level.</value>
    public int Level { get; init; }

    /// <summary>Gets or sets the purchased perk states.</summary>
    /// <value>One entry per perk installed at one of the building's mastery levels; empty by default.</value>
    public ImmutableArray<WorkbenchPerkSnapshot> PerkStates { get; init; } = [];
}
