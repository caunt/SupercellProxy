using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Events;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Defines the seasonal production gate used by the truck goods pool.</summary>
public sealed record TruckSeasonalProductionEventDefinition
{
    /// <summary>Gets the seasonal-object type in the serialized event collection.</summary>
    public const int EventType = 29;

    /// <summary>Gets or sets the seasonal building data name.</summary>
    /// <value>Saved as <c>SeasonalObject</c>; the processing building row name whose seasonal goods may appear in truck orders while this event runs, or <see langword="null"/> when unset.</value>
    [JsonPropertyName("SeasonalObject")]
    public string? Building { get; init; }

    /// <summary>Gets or sets the participation level required by the seasonal object event.</summary>
    /// <value>Saved as <c>requirements</c>; holds the minimum experience level, or is <see langword="null"/> when the event sets none.</value>
    [JsonPropertyName("requirements")]
    public EventLevelRequirements? Requirements { get; init; }

    /// <summary>Gets or sets how long before event end seasonal orders stop being generated.</summary>
    /// <value>Whole minutes before the event's end time after which the building's seasonal goods are no longer added to new truck orders; 0 keeps them until the end.</value>
    public int StopSeasonalObjectMinutesBeforeEventEnd { get; init; }
}
