using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Retains the daily completed-order count used by the alternate truck generator.</summary>
public sealed record TruckOrderManagerSnapshot
{
    /// <summary>Gets the number of orders dispatched since the daily reset.</summary>
    [JsonPropertyName("completed")]
    public int Completed { get; init; }

    /// <summary>Gets the timestamp at which the counter was reset.</summary>
    [JsonPropertyName("reset")]
    public int ResetTimestamp { get; init; }
}
