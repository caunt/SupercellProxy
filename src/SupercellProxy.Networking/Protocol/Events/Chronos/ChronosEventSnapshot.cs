using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Events.Tasks;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>
/// Represents decoded <c language="csharp">ChronosEventSnapshot</c> home data.
/// </summary>
public sealed record ChronosEventSnapshot
{
    /// <summary>
    /// Gets the Birthday Tasks value.
    /// </summary>
    [JsonPropertyName("birthdayTasks")]
    public TaskEventSnapshot? BirthdayTasks { get; init; }

    /// Gets whether home-farm completion has already run for the event.
    [JsonPropertyName("finishedAtHomeFarm")]
    public bool CompletionProcessedAtHome { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">EndTime</c> value.
    /// </summary>
    [JsonPropertyName("endTime")]
    public long EndTime { get; init; }

    /// Gets the retained event identifier.
    [JsonPropertyName("id")]
    public int EventIdentifier { get; init; }

    /// Gets whether the event's main UI has been opened.
    [JsonPropertyName("eventUIOpened")]
    public bool EventUserInterfaceOpened { get; init; }

    /// Gets whether home-farm initialization has already completed for the event.
    [JsonPropertyName("initDoneAtHomeFarm")]
    public bool InitializationCompletedAtHome { get; init; }

    /// <summary>
    /// Gets the Leaderboard Button State value.
    /// </summary>
    [JsonPropertyName("leaderboardButtonState")]
    public int LeaderboardButtonState { get; init; } = 1;

    /// <summary>
    /// Gets the Seasonal Catalogue Gifts value.
    /// </summary>
    [JsonPropertyName("seasonalCataloguePlayerGifts")]
    public SeasonalCatalogueGiftSnapshot[] SeasonalCatalogueGifts { get; init; } = [];

    /// Gets the event's retained seasonal-currency data identifier.
    [JsonPropertyName("seasonalCurrency")]
    public int SeasonalCurrencyGlobalIdentifier { get; init; }

    /// <summary>
    /// Gets the Seasonal Gift Purchase Counts value.
    /// </summary>
    [JsonPropertyName("seasonalGiftPurchaseCounts")]
    public int[] SeasonalGiftPurchaseCounts { get; init; } = [];

    /// Gets whether this event has been marked seen.
    [JsonPropertyName("seen")]
    public bool Seen { get; init; }

    /// Gets whether the event-board viewing reward has been claimed.
    [JsonPropertyName("seenInEventBoard")]
    public bool SeenInEventBoard { get; init; }

    /// Gets whether the native event-start callback has already run.
    [JsonPropertyName("startCalled")]
    public bool StartCalled { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">StartTime</c> value.
    /// </summary>
    [JsonPropertyName("startTime")]
    public long StartTime { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Type</c> value.
    /// </summary>
    [JsonPropertyName("type")]
    public int Type { get; init; }

    /// Gets the retained event variant identifier.
    [JsonPropertyName("variantId")]
    public int VariantIdentifier { get; init; }
}
