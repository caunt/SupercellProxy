using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>One player's car on the Valley board, as saved with a home.</summary>
public sealed record MapGameHomePawnSnapshot : ExtensibleDocument
{
    /// <summary>Account of the player the car belongs to; null when not saved.</summary>
    [JsonPropertyName("AvatarId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MapGameAvatarId? AvatarId { get; init; }

    /// <summary>Board node the car stands on.</summary>
    [JsonPropertyName("CurrentNodeId")]
    public int CurrentNodeId { get; init; }

    /// <summary>The player's emblem; null when none is set.</summary>
    [JsonPropertyName("Emblem")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MapGameEmblemSnapshot? Emblem { get; init; }

    /// <summary>Empty nodes the car crossed since its last empty-node task.</summary>
    [JsonPropertyName("EmptyNodesTravelled")]
    public int EmptyNodesTravelled { get; init; }

    /// <summary>Gets the empty-node travel accumulated while carrying sanctuary animals; native member 0x4c.</summary>
    [JsonPropertyName("EmptyNodesTravelledDeliveringAnimals")]
    public int EmptyNodesTravelledDeliveringAnimals { get; init; }

    /// <summary>Gets the remaining empty-node immunity before a carried sanctuary animal can escape; native member 0x48.</summary>
    [JsonPropertyName("EmptyNodesTravelledDeliveringAnimalsImmunity")]
    public int EmptyNodesTravelledDeliveringAnimalsImmunity { get; init; }

    /// <summary>Farm level of the player the car belongs to (saved as <c>ExpLevel</c>).</summary>
    [JsonPropertyName("ExpLevel")]
    public int ExperienceLevel { get; init; }

    /// <summary>Name shown for the player; null when not saved.</summary>
    [JsonPropertyName("Name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; init; }

    /// <summary>Neighbourhood of the player; null when they are in none.</summary>
    [JsonPropertyName("NeighborhoodId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MapGameAvatarId? NeighborhoodId { get; init; }

    /// <summary>Valley profile options the player picked.</summary>
    [JsonPropertyName("SelectedOptions")]
    public int[] SelectedOptions { get; init; } = [];
}
