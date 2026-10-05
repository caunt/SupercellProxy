using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>Describes the native event-board presentation supplied by an event's UI section.</summary>
public sealed record EventUserInterfaceDefinition
{
    /// <summary>Gets the alternative event text.</summary>
    [JsonPropertyName("altText")]
    public string? AlternativeText { get; init; }

    /// <summary>Gets the end-notification text.</summary>
    [JsonPropertyName("endingNotificationText")]
    public string? EndingNotification { get; init; }

    /// <summary>Gets the end-notification lead time; absence retains the farm's configured default.</summary>
    [JsonPropertyName("endingNotificationTimeBeforeEnd")]
    public int? EndingNotificationSeconds { get; init; }

    /// <summary>Gets the downloadable event localization file.</summary>
    [JsonPropertyName("localizationFile")]
    public EventAssetDefinition? LocalizationFile { get; init; }

    /// <summary>Gets the event-start notification text.</summary>
    [JsonPropertyName("notification")]
    public string? Notification { get; init; }

    /// <summary>Gets an export in the game's event poster asset.</summary>
    [JsonPropertyName("posterCustomExportName")]
    public string? PosterExportName { get; init; }

    /// <summary>Gets a downloadable poster image.</summary>
    [JsonPropertyName("posterWebImage")]
    public EventAssetDefinition? PosterImage { get; init; }

    /// <summary>Gets the decoration chosen for the event-board reward preview.</summary>
    [JsonPropertyName("shownDecorationReward")]
    public string? PreviewDecoration { get; init; }

    /// <summary>Gets the preview's reward count; absence retains the farm's configured default.</summary>
    [JsonPropertyName("show_n_rewards")]
    public int? PreviewRewardCount { get; init; }

    /// <summary>Gets the reward-track name used for the event-board preview.</summary>
    [JsonPropertyName("shownRewardTrack")]
    public string? PreviewRewardTrack { get; init; }

    /// <summary>Gets the optional override of the event's action button.</summary>
    [JsonPropertyName("show_call_to_action_button")]
    public bool? ShowActionButton { get; init; }

    /// <summary>Gets the optional override of the reminder button.</summary>
    [JsonPropertyName("show_set_a_reminder_button")]
    public bool? ShowReminderButton { get; init; }

    /// <summary>Gets the event-board description.</summary>
    [JsonPropertyName("text")]
    public string? Text { get; init; }

    /// <summary>Gets the event-board title.</summary>
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    /// <summary>Gets the event-board text displayed after participation ends.</summary>
    [JsonPropertyName("visibleEndText")]
    public string? VisibleEndText { get; init; }

    /// <summary>Gets the event-board title displayed after participation ends.</summary>
    [JsonPropertyName("visibleEndTitle")]
    public string? VisibleEndTitle { get; init; }
}
