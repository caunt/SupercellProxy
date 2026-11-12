using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.CountyFair;

/// <summary>Retains the County Fair's opening acknowledgement independently of ordinary task events.</summary>
public sealed record CountyFairSnapshot
{
    /// <summary>Gets or sets whether the player has opened this County Fair.</summary>
    /// <value><see langword="true"/> if the fair was marked opened for the event; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("openedByPlayer")]
    public bool OpenedByPlayer { get; init; }
}
