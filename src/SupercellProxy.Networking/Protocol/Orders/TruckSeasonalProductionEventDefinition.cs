using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Events;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Defines the seasonal production gate used by the truck goods pool.</summary>
public sealed record TruckSeasonalProductionEventDefinition
{
    /// <summary>Gets the seasonal-object type in the serialized event collection.</summary>
    public const int EventType = 29;

    /// <summary>Gets the seasonal building data name.</summary>
    [JsonPropertyName("SeasonalObject")]
    public string? Building { get; init; }

    /// <summary>Gets the participation level required by the seasonal object event.</summary>
    [JsonPropertyName("requirements")]
    public EventLevelRequirements? Requirements { get; init; }

    /// <summary>Gets how long before event end seasonal orders stop being generated.</summary>
    public int StopSeasonalObjectMinutesBeforeEventEnd { get; init; }
}
