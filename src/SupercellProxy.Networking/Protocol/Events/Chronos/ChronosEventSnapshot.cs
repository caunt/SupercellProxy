using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Events.CountyFair;

using SupercellProxy.Networking.Protocol.Events.Tasks;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>
/// Represents decoded <c language="csharp">ChronosEventSnapshot</c> home data.
/// </summary>
public sealed record ChronosEventSnapshot
{
    /// <summary>
    /// Task progress when this is a birthday event; null for other event kinds.
    /// </summary>
    [JsonPropertyName("birthdayTasks")]
    public TaskEventSnapshot? BirthdayTasks { get; init; }
    /// <summary>Gets whether home-farm completion has already run for the event.</summary>
    [JsonPropertyName("finishedAtHomeFarm")]
    public bool CompletionProcessedAtHome { get; init; }

    /// <summary>Gets the County Fair state associated with this event.</summary>
    [JsonPropertyName("countyFair")]
    public CountyFairSnapshot? CountyFair { get; init; }

    /// <summary>
    /// Unix seconds when the event leaves the board.
    /// </summary>
    [JsonPropertyName("endTime")]
    public long EndTime { get; init; }

    /// <summary>Gets the retained event id.</summary>
    [JsonPropertyName("id")]
    public int EventId { get; init; }

    /// <summary>Gets whether the event's main UI has been opened.</summary>
    [JsonPropertyName("eventUIOpened")]
    public bool EventUserInterfaceOpened { get; init; }

    /// <summary>Gets whether home-farm initialization has already completed for the event.</summary>
    [JsonPropertyName("initDoneAtHomeFarm")]
    public bool InitializationCompletedAtHome { get; init; }

    /// <summary>
    /// Display state of the event's leaderboard button, as native numbers it; 1 in captures.
    /// </summary>
    [JsonPropertyName("leaderboardButtonState")]
    public int LeaderboardButtonState { get; init; } = 1;

    /// <summary>
    /// Items the event's gift catalogue offers for its seasonal currency; empty for events without a catalogue.
    /// </summary>
    [JsonPropertyName("seasonalCataloguePlayerGifts")]
    public SeasonalCatalogueGiftSnapshot[] SeasonalCatalogueGifts { get; init; } = [];

    /// <summary>Gets the event's retained seasonal-currency data id.</summary>
    [JsonPropertyName("seasonalCurrency")]
    public int SeasonalCurrencyGlobalId { get; init; }

    /// <summary>
    /// How many times the player bought each catalogue item, indexed like the catalogue; compared with each item's purchase limit.
    /// </summary>
    [JsonPropertyName("seasonalGiftPurchaseCounts")]
    public int[] SeasonalGiftPurchaseCounts { get; init; } = [];

    /// <summary>Gets whether this event has been marked seen.</summary>
    [JsonPropertyName("seen")]
    public bool Seen { get; init; }

    /// <summary>Gets whether the event-board viewing reward has been claimed.</summary>
    [JsonPropertyName("seenInEventBoard")]
    public bool SeenInEventBoard { get; init; }

    /// <summary>Gets whether the native event-start callback has already run.</summary>
    [JsonPropertyName("startCalled")]
    public bool StartCalled { get; init; }

    /// <summary>
    /// Unix seconds when the event begins.
    /// </summary>
    [JsonPropertyName("startTime")]
    public long StartTime { get; init; }

    /// <summary>
    /// Native event type number, which decides how the event behaves (for example 29 for a seasonal catalogue event).
    /// </summary>
    [JsonPropertyName("type")]
    public int Type { get; init; }

    /// <summary>Gets the retained event variant id.</summary>
    [JsonPropertyName("variantId")]
    public int VariantId { get; init; }
}
