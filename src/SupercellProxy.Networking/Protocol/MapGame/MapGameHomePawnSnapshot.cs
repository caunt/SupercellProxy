using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>Represents decoded MapGameHomePawnSnapshot state.</summary>
public sealed record MapGameHomePawnSnapshot : ExtensibleDocument
{
    /// <summary>Gets the AvatarIdentifier value.</summary>
    [JsonPropertyName("AvatarId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MapGameAvatarIdentifier? AvatarIdentifier { get; init; }

    /// <summary>Gets the CurrentNodeIdentifier value.</summary>
    [JsonPropertyName("CurrentNodeId")]
    public int CurrentNodeIdentifier { get; init; }

    /// <summary>Gets the Emblem value.</summary>
    [JsonPropertyName("Emblem")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MapGameEmblemSnapshot? Emblem { get; init; }

    /// <summary>Gets the EmptyNodesTravelled value.</summary>
    [JsonPropertyName("EmptyNodesTravelled")]
    public int EmptyNodesTravelled { get; init; }

    /// <summary>Gets the EmptyNodesTravelledDeliveringAnimals value.</summary>
    [JsonPropertyName("EmptyNodesTravelledDeliveringAnimals")]
    public int EmptyNodesTravelledDeliveringAnimals { get; init; }

    /// <summary>Gets the EmptyNodesTravelledDeliveringAnimalsImmunity value.</summary>
    [JsonPropertyName("EmptyNodesTravelledDeliveringAnimalsImmunity")]
    public int EmptyNodesTravelledDeliveringAnimalsImmunity { get; init; }

    /// <summary>Gets the ExperienceLevel value.</summary>
    [JsonPropertyName("ExpLevel")]
    public int ExperienceLevel { get; init; }

    /// <summary>Gets the Name value.</summary>
    [JsonPropertyName("Name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; init; }

    /// <summary>Gets the NeighborhoodIdentifier value.</summary>
    [JsonPropertyName("NeighborhoodId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MapGameAvatarIdentifier? NeighborhoodIdentifier { get; init; }

    /// <summary>Gets the SelectedOptions value.</summary>
    [JsonPropertyName("SelectedOptions")]
    public int[] SelectedOptions { get; init; } = [];

}
