using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">ChronosEventSnapshot</c> home data.
/// </summary>
internal sealed record ChronosEventSnapshot
{
    /// Gets the retained event identifier.
    [JsonPropertyName("id")]
    public int EventId { get; init; }

    /// Gets the retained event variant identifier.
    [JsonPropertyName("variantId")]
    public int VariantId { get; init; }

    /// Gets whether this event has been seen in the event board.
    [JsonPropertyName("seen")]
    public bool SeenInEventBoard { get; init; }

    /// Gets whether the native event-start callback has already run.
    [JsonPropertyName("startCalled")]
    public bool StartCalled { get; init; }

    /// Gets whether home-farm initialization has already completed for the event.
    [JsonPropertyName("initDoneAtHomeFarm")]
    public bool InitializationCompletedAtHome { get; init; }

    /// Gets whether home-farm completion has already run for the event.
    [JsonPropertyName("finishedAtHomeFarm")]
    public bool CompletionProcessedAtHome { get; init; }

    /// Gets the event's retained seasonal-currency data identifier.
    [JsonPropertyName("seasonalCurrency")]
    public int SeasonalCurrencyGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Type</c> value.
    /// </summary>
    [JsonPropertyName("type")]
    public int Type { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">StartTime</c> value.
    /// </summary>
    [JsonPropertyName("startTime")]
    public long StartTime { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">EndTime</c> value.
    /// </summary>
    [JsonPropertyName("endTime")]
    public long EndTime { get; init; }
}
