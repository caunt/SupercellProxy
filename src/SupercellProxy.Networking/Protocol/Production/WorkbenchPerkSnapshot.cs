using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.Production;

/// <summary>Retains the level, perk data row, and native activation flags.</summary>
public sealed record WorkbenchPerkSnapshot : ExtensibleDocument
{
    /// <summary>Gets whether the perk is active.</summary>
    public bool Active { get; init; }
    /// <summary>Gets the workbench perk data id.</summary>
    [JsonPropertyName("GlobalId")]
    public int DataGlobalId { get; init; }
    /// <summary>Gets whether the perk came from legacy mastery.</summary>
    public bool Legacy { get; init; }
    /// <summary>Gets the building level containing the perk.</summary>
    public int Level { get; init; }
}
