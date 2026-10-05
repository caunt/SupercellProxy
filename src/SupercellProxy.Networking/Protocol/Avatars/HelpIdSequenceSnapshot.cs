using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>Retains the avatar's monotonically allocated help ids.</summary>
public sealed record HelpIdSequenceSnapshot
{
    /// <summary>Gets the last allocated help id.</summary>
    [JsonPropertyName("LastId")]
    public int LastId { get; init; }
}
