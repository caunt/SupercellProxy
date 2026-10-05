using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Calendar;

/// <summary>Defines one resource alternative inside a calendar gift.</summary>
public sealed record CalendarGiftRewardDefinition
{
    /// <summary>Gets the reward quantity.</summary>
    [JsonPropertyName("amount")]
    public int Amount { get; init; }
    /// <summary>Gets the resource's asset name.</summary>
    [JsonPropertyName("gift")]
    public string? Name { get; init; }
}
