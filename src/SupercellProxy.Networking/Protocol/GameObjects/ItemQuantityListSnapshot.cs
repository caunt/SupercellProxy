using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>Represents item ids and quantities stored in a collection area.</summary>
public sealed record ItemQuantityListSnapshot
{
    /// <summary>Gets or sets the global ids of the items waiting in the list, saved under the <c>Donations</c> key.</summary>
    /// <value>One item global id per entry, paired by index with <see cref="Amounts"/>; empty by default and when nothing is waiting.</value>
    [JsonPropertyName("Donations")]
    public ImmutableArray<int> ItemGlobalIds { get; init; } = [];

    /// <summary>Gets or sets how many of each listed item are waiting.</summary>
    /// <value>One positive count per entry of <see cref="ItemGlobalIds"/>, in the same order; the arrays must have equal length. Empty by default.</value>
    public ImmutableArray<int> Amounts { get; init; } = [];
}
