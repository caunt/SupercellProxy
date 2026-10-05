using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>Retains the avatar's monotonically allocated help identifiers.</summary>
public sealed record HelpIdentifierSequenceSnapshot
{
    /// <summary>Gets the last allocated help identifier.</summary>
    [JsonPropertyName("LastId")]
    public int LastIdentifier { get; init; }
}
