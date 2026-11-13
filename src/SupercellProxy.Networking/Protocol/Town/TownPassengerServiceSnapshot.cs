using System.Collections.Immutable;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>One saved service requested by a town passenger.</summary>
public sealed record TownPassengerServiceSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets whether this service was canceled.</summary>
    /// <value><see langword="true"/> if the booking was canceled and the passenger walked off without it; otherwise, <see langword="false"/>.</value>
    public bool Canceled { get; init; }

    /// <summary>Gets or sets whether this service was completed.</summary>
    /// <value><see langword="true"/> if the building finished this service for the passenger; otherwise, <see langword="false"/>.</value>
    /// <remarks>A fresh request starts as <see langword="false"/>.</remarks>
    public bool Completed { get; init; }

    /// <summary>Gets or sets the quantity of each required good.</summary>
    /// <value>Amount of the good at the same index in <see cref="RequiredGoods"/>; empty by default.</value>
    public ImmutableArray<int> RequiredGoodAmounts { get; init; } = [];

    /// <summary>Gets or sets the required goods by data id.</summary>
    /// <value>Data global ids of the goods the service needs, in the same order as <see cref="RequiredGoodAmounts"/>; empty by default.</value>
    public ImmutableArray<int> RequiredGoods { get; init; } = [];

    /// <summary>Gets or sets the service building's data id.</summary>
    /// <value>Data global id of the service building type that performs this request; 0 is the default.</value>
    public int ServiceBuildingData { get; init; }
}
