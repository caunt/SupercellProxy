using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Defines the season's shared collection goal.</summary>
public sealed record MapGameCollectionGoalDefinition
{
    /// <summary>Gets the required shared count.</summary>
    [JsonPropertyName("goalNumber")]
    public int? Count { get; init; }

    /// <summary>Gets the native collection-goal name.</summary>
    [JsonPropertyName("goalType")]
    public string? Type { get; init; }
}
