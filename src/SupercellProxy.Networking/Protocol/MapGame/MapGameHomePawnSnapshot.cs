using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>One player's car on the Valley board, as saved with a home.</summary>
public sealed record MapGameHomePawnSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the account of the player the car belongs to; <see langword="null"/> when not saved.</summary>
    /// <value>The player's account id in its saved two-word form, which identifies the car on the board; left out of the JSON when <see langword="null"/>.</value>
    [JsonPropertyName("AvatarId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MapGameAvatarId? AvatarId { get; init; }

    /// <summary>Gets or sets the board node the car stands on.</summary>
    /// <value>The id of a node of the season's Valley map; 0 by default.</value>
    [JsonPropertyName("CurrentNodeId")]
    public int CurrentNodeId { get; init; }

    /// <summary>Gets or sets the player's emblem; <see langword="null"/> when none is set.</summary>
    /// <value>The background, pattern and symbol the player chose, left out of the JSON when <see langword="null"/>.</value>
    [JsonPropertyName("Emblem")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MapGameEmblemSnapshot? Emblem { get; init; }

    /// <summary>Gets or sets the empty nodes the car crossed since its last empty-node task.</summary>
    /// <value>A non-negative count.</value>
    [JsonPropertyName("EmptyNodesTravelled")]
    public int EmptyNodesTravelled { get; init; }

    /// <summary>Gets or sets the empty-node travel accumulated while carrying sanctuary animals.</summary>
    /// <value>Empty nodes crossed with animals on board.</value>
    [JsonPropertyName("EmptyNodesTravelledDeliveringAnimals")]
    public int EmptyNodesTravelledDeliveringAnimals { get; init; }

    /// <summary>Gets or sets the remaining empty-node immunity before a carried sanctuary animal can escape.</summary>
    /// <value>Empty nodes the car may still cross safely; 0 when the car carries none.</value>
    [JsonPropertyName("EmptyNodesTravelledDeliveringAnimalsImmunity")]
    public int EmptyNodesTravelledDeliveringAnimalsImmunity { get; init; }

    /// <summary>Gets or sets the farm level of the player the car belongs to (saved as <c>ExpLevel</c>).</summary>
    /// <value>The player's experience level at the time the board was saved; 0 by default.</value>
    [JsonPropertyName("ExpLevel")]
    public int ExperienceLevel { get; init; }

    /// <summary>Gets or sets the name shown for the player; <see langword="null"/> when not saved.</summary>
    /// <value>The player's display name as other Valley players see it, left out of the JSON when <see langword="null"/>.</value>
    [JsonPropertyName("Name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; init; }

    /// <summary>Gets or sets the neighbourhood of the player; <see langword="null"/> when they are in none.</summary>
    /// <value>The neighbourhood's id in its saved two-word form, left out of the JSON when <see langword="null"/>.</value>
    [JsonPropertyName("NeighborhoodId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MapGameAvatarId? NeighborhoodId { get; init; }

    /// <summary>Gets or sets the Valley profile options the player picked.</summary>
    /// <value>The option numbers; empty by default.</value>
    [JsonPropertyName("SelectedOptions")]
    public int[] SelectedOptions { get; init; } = [];
}
