using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Retains the two native identifier words of an order's helper.</summary>
public sealed record TruckOrderHelperSnapshot
{
    /// <summary>Gets or sets the high identifier word.</summary>
    /// <value>Saved as <c>hi</c>; the high 32 bits of the helper's account id.</value>
    [JsonPropertyName("hi")]
    public int High { get; init; }

    /// <summary>Gets or sets the low identifier word.</summary>
    /// <value>Saved as <c>lo</c>; the low 32 bits of the helper's account id.</value>
    [JsonPropertyName("lo")]
    public int Low { get; init; }
}
