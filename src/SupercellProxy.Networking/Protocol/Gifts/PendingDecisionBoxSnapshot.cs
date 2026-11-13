using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>A saved reward box awaiting the player's choice.</summary>
public sealed record PendingDecisionBoxSnapshot
{
    /// <summary>Gets or sets the source's saved reward metadata.</summary>
    /// <value>Key and value pairs saved with the box under <c>details</c>; empty by default and for sources that attach none.</value>
    [JsonPropertyName("details")]
    public ImmutableArray<DecisionBoxSourceDetail> Details { get; init; } = [];

    /// <summary>Gets or sets the decision-box data id.</summary>
    /// <value>The global id of the box's row in the decision boxes table, which lists its reward options.</value>
    [JsonPropertyName("GlobalId")]
    public int GlobalId { get; init; }

    /// <summary>Gets or sets whether currency rewards use purchased rather than earned currency counters.</summary>
    /// <value><see langword="true"/> if a currency reward picked from this box counts as purchased currency; otherwise, <see langword="false"/>.</value>
    /// <remarks>The flag defaults to <see langword="false"/>, under which the reward counts as earned currency.</remarks>
    public bool Paid { get; init; }

    /// <summary>Gets or sets the native acquisition reason carried by this reward.</summary>
    /// <value>The numeric reward reason the picked reward is granted with, such as the order-track or chain-offer reason; 0 by default.</value>
    public int SourceTag { get; init; }
}
