using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Calendar;

/// <summary>Defines a calendar box's date, position, appearance and reward alternatives.</summary>
public sealed record CalendarGiftDefinition
{
    /// <summary>Gets the calendar-box asset name.</summary>
    [JsonPropertyName("giftAsset")]
    public string? Asset { get; init; }
    /// <summary>Gets the opening date in yyyyMMdd format.</summary>
    [JsonPropertyName("availableDate")]
    public string? AvailableDate { get; init; }
    /// <summary>Gets the box's label.</summary>
    [JsonPropertyName("giftLabel")]
    public string? Label { get; init; }
    /// <summary>Gets reward alternatives in native fallback order.</summary>
    [JsonPropertyName("gift")]
    public CalendarGiftRewardDefinition[] Rewards { get; init; } = [];
    /// <summary>Gets the horizontal tile coordinate.</summary>
    [JsonPropertyName("tileX")]
    public int TileX { get; init; }
    /// <summary>Gets the vertical tile coordinate.</summary>
    [JsonPropertyName("tileY")]
    public int TileY { get; init; }
}
