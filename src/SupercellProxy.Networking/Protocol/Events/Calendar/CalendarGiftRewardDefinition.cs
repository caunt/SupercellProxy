using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Calendar;

/// <summary>Defines one resource alternative inside a calendar gift.</summary>
public sealed record CalendarGiftRewardDefinition
{
    /// <summary>Gets or sets the reward quantity.</summary>
    /// <value>How many units of <see cref="Name"/> the gift grants, read from <c>amount</c>.</value>
    [JsonPropertyName("amount")]
    public int Amount { get; init; }
    /// <summary>Gets or sets the resource's asset name.</summary>
    /// <value>The data-table name of the rewarded resource, read from <c>gift</c>.</value>
    [JsonPropertyName("gift")]
    public string? Name { get; init; }
}
