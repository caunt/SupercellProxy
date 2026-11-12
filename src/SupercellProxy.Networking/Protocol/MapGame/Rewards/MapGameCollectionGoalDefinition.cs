using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Defines the season's shared collection goal.</summary>
public sealed record MapGameCollectionGoalDefinition
{
    /// <summary>Gets or sets the required shared count.</summary>
    /// <value>How many chickens or animals the Valley game must collect, from <c>goalNumber</c>; <see langword="null"/> when the definition names none.</value>
    [JsonPropertyName("goalNumber")]
    public int? Count { get; init; }

    /// <summary>Gets or sets the native collection-goal name.</summary>
    /// <value>The goal kind from <c>goalType</c>, such as <c>Collect Sanctuary Animals</c>; <see langword="null"/> when the definition names none.</value>
    [JsonPropertyName("goalType")]
    public string? Type { get; init; }
}
