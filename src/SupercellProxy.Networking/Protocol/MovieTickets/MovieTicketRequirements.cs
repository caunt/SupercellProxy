using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>Represents the decoded MovieTicketRequirements JSON contract.</summary>
public sealed record MovieTicketRequirements
{
    /// <summary>Gets the MinimumLevel value.</summary>
    [JsonPropertyName("minLevel")]
    public int MinimumLevel { get; init; }

}
