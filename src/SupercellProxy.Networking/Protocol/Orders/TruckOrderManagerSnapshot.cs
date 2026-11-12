using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Retains the daily completed-order count used by the alternate truck generator.</summary>
public sealed record TruckOrderManagerSnapshot
{
    /// <summary>Gets or sets the number of orders dispatched since the daily reset.</summary>
    /// <value>Saved as <c>completed</c>; a daily count of truck deliveries.</value>
    [JsonPropertyName("completed")]
    public int Completed { get; init; }

    /// <summary>Gets or sets the timestamp at which the counter was reset.</summary>
    /// <value>Saved as <c>reset</c>; Unix time in seconds of the last reset.</value>
    [JsonPropertyName("reset")]
    public int ResetTimestamp { get; init; }
}
