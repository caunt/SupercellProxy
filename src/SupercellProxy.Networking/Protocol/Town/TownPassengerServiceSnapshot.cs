using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>One saved service requested by a town passenger.</summary>
public sealed record TownPassengerServiceSnapshot : ExtensibleDocument
{
    /// <summary>Gets whether this service was canceled.</summary>
    public bool Canceled { get; init; }

    /// <summary>Gets whether this service was completed.</summary>
    public bool Completed { get; init; }

    /// <summary>Gets the required goods by data id.</summary>
    public int[] RequiredGoods { get; init; } = [];

    /// <summary>Gets the quantity of each required good.</summary>
    public int[] RequiredGoodAmounts { get; init; } = [];

    /// <summary>Gets the service building's data id.</summary>
    public int ServiceBuildingData { get; init; }
}
