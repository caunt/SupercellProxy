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
    /// Gets or sets the task progress when this is a birthday event; <see langword="null"/> for other event kinds.
    /// </summary>
    /// <value>The saved task list and per-task progress of the event's task board.</value>
    [JsonPropertyName("birthdayTasks")]
    public TaskEventSnapshot? BirthdayTasks { get; init; }
    /// <summary>Gets or sets whether home-farm completion has already run for the event.</summary>
    /// <value><see langword="true"/> if the client already ran the event's end handling on the player's own farm (saved as <c>finishedAtHomeFarm</c>); otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("finishedAtHomeFarm")]
    public bool CompletionProcessedAtHome { get; init; }

    /// <summary>Gets or sets the County Fair state associated with this event.</summary>
    /// <value>The fair's saved state, such as whether the player opened it, or <see langword="null"/> when the event has no County Fair.</value>
    [JsonPropertyName("countyFair")]
    public CountyFairSnapshot? CountyFair { get; init; }

    /// <summary>
    /// Gets or sets the Unix seconds when the event leaves the board.
    /// </summary>
    /// <value>The exclusive end of the event's run: the event counts as running while the server time is at least <see cref="StartTime"/> and below this value.</value>
    [JsonPropertyName("endTime")]
    public long EndTime { get; init; }

    /// <summary>Gets or sets the retained event id.</summary>
    /// <value>The id of the shop live event this board entry tracks (saved as <c>id</c>); commands and player data refer to the event by it.</value>
    [JsonPropertyName("id")]
    public int EventId { get; init; }

    /// <summary>Gets or sets whether the event's main UI has been opened.</summary>
    /// <value><see langword="true"/> if the player has opened the event's main screen; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("eventUIOpened")]
    public bool EventUserInterfaceOpened { get; init; }

    /// <summary>Gets or sets whether home-farm initialization has already completed for the event.</summary>
    /// <value><see langword="true"/> if the client already set the event up on the player's own farm (saved as <c>initDoneAtHomeFarm</c>); otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("initDoneAtHomeFarm")]
    public bool InitializationCompletedAtHome { get; init; }

    /// <summary>
    /// Gets or sets the display state of the event's leaderboard button, as native numbers it; 1 in captures.
    /// </summary>
    /// <value>0 once the notification is cleared, 1 when the event first gets a standing, and 2 after the standing drops; defaults to 1.</value>
    [JsonPropertyName("leaderboardButtonState")]
    public int LeaderboardButtonState { get; init; } = 1;

    /// <summary>
    /// Gets or sets the items the event's gift catalogue offers for its seasonal currency; empty for events without a catalogue.
    /// </summary>
    /// <value>The catalogue entries in their saved order; their positions are the gift indexes used by purchases and by <see cref="SeasonalGiftPurchaseCounts"/>.</value>
    [JsonPropertyName("seasonalCataloguePlayerGifts")]
    public SeasonalCatalogueGiftSnapshot[] SeasonalCatalogueGifts { get; init; } = [];

    /// <summary>Gets or sets the event's retained seasonal-currency data id.</summary>
    /// <value>The global id of the seasonal currency row the event uses, or 0 when the event has none.</value>
    [JsonPropertyName("seasonalCurrency")]
    public int SeasonalCurrencyGlobalId { get; init; }

    /// <summary>
    /// Gets or sets how many times the player bought each catalogue item, indexed like the catalogue.
    /// </summary>
    /// <value>One purchase count per catalogue item; empty by default.</value>
    [JsonPropertyName("seasonalGiftPurchaseCounts")]
    public int[] SeasonalGiftPurchaseCounts { get; init; } = [];

    /// <summary>Gets or sets whether this event has been marked seen.</summary>
    /// <value><see langword="true"/> if the event has been marked seen; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("seen")]
    public bool Seen { get; init; }

    /// <summary>Gets or sets whether the event-board viewing reward has been claimed.</summary>
    /// <value><see langword="true"/> if the player already received the cash (diamond) reward configured as <c>NewEventBoardEventSeenCashReward</c> for viewing the event on the board; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("seenInEventBoard")]
    public bool SeenInEventBoard { get; init; }

    /// <summary>Gets or sets whether the event's start has already been handled.</summary>
    /// <value><see langword="true"/> if the client has started the event (saved as <c>startCalled</c>); otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("startCalled")]
    public bool StartCalled { get; init; }

    /// <summary>
    /// Gets or sets the Unix seconds when the event begins.
    /// </summary>
    /// <value>The inclusive start of the event's run.</value>
    [JsonPropertyName("startTime")]
    public long StartTime { get; init; }

    /// <summary>
    /// Gets or sets the native event type number, which decides how the event behaves (for example 29 for a seasonal catalogue event).
    /// </summary>
    /// <value>The shop live-event type of the tracked event.</value>
    [JsonPropertyName("type")]
    public int Type { get; init; }

    /// <summary>Gets or sets the retained event variant id.</summary>
    /// <value>The variant of the live event the player was given; shop purchases and neighbourhood event commands carry it.</value>
    [JsonPropertyName("variantId")]
    public int VariantId { get; init; }
}
