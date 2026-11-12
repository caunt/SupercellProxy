using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>Retains the avatar's monotonically allocated help ids.</summary>
public sealed record HelpIdSequenceSnapshot
{
    /// <summary>Gets or sets the last allocated help id.</summary>
    /// <value>The highest help id handed out so far to a passenger or help request; 0 when no id was allocated yet.</value>
    [JsonPropertyName("LastId")]
    public int LastId { get; init; }
}
