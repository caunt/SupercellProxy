using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Visitors;

/// <summary>A named resource and amount in a special visitor trade.</summary>
public sealed record FarmVisitorSpecialOfferResource
{
    /// <summary>Gets the native resource amount.</summary>
    public int Amount { get; init; }

    /// <summary>Gets the resource's data-table name.</summary>
    [JsonPropertyName("data")]
    public string DataName { get; init; } = string.Empty;
}
