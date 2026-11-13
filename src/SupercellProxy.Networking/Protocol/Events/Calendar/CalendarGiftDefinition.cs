using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Calendar;

/// <summary>Defines a calendar box's date, position, appearance and reward alternatives.</summary>
public sealed record CalendarGiftDefinition
{
    /// <summary>Gets or sets the calendar-box asset name.</summary>
    /// <value>The name of the box's row in the calendar event gift box table, read from <c>giftAsset</c>.</value>
    [JsonPropertyName("giftAsset")]
    public string? Asset { get; init; }

    /// <summary>Gets or sets the opening date in yyyyMMdd format.</summary>
    /// <value>The UTC day from which the box can be collected, such as <c>20261019</c>, read from <c>availableDate</c>.</value>
    [JsonPropertyName("availableDate")]
    public string? AvailableDate { get; init; }

    /// <summary>Gets or sets the box's label.</summary>
    /// <value>The display label read from <c>giftLabel</c>, or <see langword="null"/> when the gift has none.</value>
    [JsonPropertyName("giftLabel")]
    public string? Label { get; init; }

    /// <summary>Gets or sets the reward alternatives in native order.</summary>
    /// <value>The rewards read from <c>gift</c>; empty by default.</value>
    [JsonPropertyName("gift")]
    public ImmutableArray<CalendarGiftRewardDefinition> Rewards { get; init; } = [];

    /// <summary>Gets or sets the horizontal tile coordinate.</summary>
    /// <value>The tile column the box is placed at on Greg's farm, read from <c>tileX</c>.</value>
    [JsonPropertyName("tileX")]
    public int TileX { get; init; }

    /// <summary>Gets or sets the vertical tile coordinate.</summary>
    /// <value>The tile row the box is placed at on Greg's farm, read from <c>tileY</c>.</value>
    [JsonPropertyName("tileY")]
    public int TileY { get; init; }
}
