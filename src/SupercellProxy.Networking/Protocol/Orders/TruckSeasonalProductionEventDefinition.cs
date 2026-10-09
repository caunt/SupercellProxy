using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Defines the seasonal production gate used by the truck goods pool.</summary>
public sealed record TruckSeasonalProductionEventDefinition
{
    /// <summary>Gets the seasonal building data name.</summary>
    [JsonPropertyName("SeasonalObject")]
    public string? Building { get; init; }

    /// <summary>Gets how long before event end seasonal orders stop being generated.</summary>
    public int StopSeasonalObjectMinutesBeforeEventEnd { get; init; }
}
