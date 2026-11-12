using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Visitors;

/// <summary>A named resource and amount in a special visitor trade.</summary>
public sealed record FarmVisitorSpecialOfferResource
{
    /// <summary>Gets or sets the native resource amount.</summary>
    /// <value>How many units of <see cref="DataName"/> are asked for or paid; 0 by default.</value>
    public int Amount { get; init; }

    /// <summary>Gets or sets the resource's data-table name.</summary>
    /// <value>Row name of the good or money resource (saved as <c>data</c>), resolved against the game's data tables; empty by default.</value>
    [JsonPropertyName("data")]
    public string DataName { get; init; } = string.Empty;
}
