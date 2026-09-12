using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Defines the Farm Object Snapshot contract.
/// </summary>
public sealed record FarmObjectSnapshot
{
    /// <summary>
    /// Gets the Data Global Id value.
    /// </summary>
    [JsonPropertyName("ID")]
    public int DataGlobalIdentifier { get; init; }

    /// <summary>
    /// Gets the Expansion Locked value.
    /// </summary>
    public bool ExpansionLocked { get; init; }
}
