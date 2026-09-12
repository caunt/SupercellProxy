using SupercellProxy.Networking.Json;

using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>
/// Defines the Decision Box Manager Snapshot contract.
/// </summary>
public sealed record DecisionBoxManagerSnapshot
{

    /// <summary>
    /// Gets the Pending Boxes value.
    /// </summary>
    [JsonPropertyName("PendingBoxes")]
    public EncodedDocumentValue[] PendingBoxes { get; init; } = [];
    /// <summary>
    /// Gets the Rand Seed value.
    /// </summary>
    public int RandSeed { get; init; }
}
