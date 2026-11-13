using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.Production;

/// <summary>Retains the level, perk data row, and native activation flags.</summary>
public sealed record WorkbenchPerkSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets whether the perk is active.</summary>
    /// <value><see langword="true"/> if the perk currently gives its bonus, such as faster production or extra truck or boat coins and experience; otherwise, <see langword="false"/>.</value>
    public bool Active { get; init; }

    /// <summary>Gets or sets the workbench perk data id.</summary>
    /// <value>Saved as <c>GlobalId</c>; the global id of the perk row, such as <c>Speed</c>, <c>Truck_Coins</c> or <c>Boat_XP</c>.</value>
    [JsonPropertyName("GlobalId")]
    public int DataGlobalId { get; init; }

    /// <summary>Gets or sets whether the perk came from legacy mastery.</summary>
    /// <value><see langword="true"/> if the perk was carried over from the older building mastery system; otherwise, <see langword="false"/>.</value>
    public bool Legacy { get; init; }

    /// <summary>Gets or sets the building level containing the perk.</summary>
    /// <value>The mastery level of the building at which the perk is installed.</value>
    public int Level { get; init; }
}
