using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Retains the two native identifier words of an order's helper.</summary>
public sealed record TruckOrderHelperSnapshot
{
    /// <summary>Gets the high identifier word.</summary>
    [JsonPropertyName("hi")]
    public int High { get; init; }

    /// <summary>Gets the low identifier word.</summary>
    [JsonPropertyName("lo")]
    public int Low { get; init; }
}
