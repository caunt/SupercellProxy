using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events;

/// <summary>Describes the native event-board presentation supplied by an event's UI section.</summary>
public sealed record EventUserInterfaceDefinition
{
    /// <summary>Gets or sets the alternative event text.</summary>
    /// <value>The <c>altText</c> string of the event's UI section, or <see langword="null"/> when the event omits it.</value>
    [JsonPropertyName("altText")]
    public string? AlternativeText { get; init; }

    /// <summary>Gets or sets the end-notification text.</summary>
    /// <value>The <c>endingNotificationText</c> shown in the notification sent shortly before the event ends, or <see langword="null"/> when the event sends no such notification text.</value>
    [JsonPropertyName("endingNotificationText")]
    public string? EndingNotification { get; init; }

    /// <summary>Gets or sets the end-notification lead time; absence retains the farm's configured default.</summary>
    /// <value>The number of seconds before the event's end at which <see cref="EndingNotification"/> fires, read from <c>endingNotificationTimeBeforeEnd</c>, or <see langword="null"/> to keep the default lead time.</value>
    [JsonPropertyName("endingNotificationTimeBeforeEnd")]
    public int? EndingNotificationSeconds { get; init; }

    /// <summary>Gets or sets the downloadable event localization file.</summary>
    /// <value>The path and SHA-1 fingerprint of the event's own localization file, or <see langword="null"/> when the event ships none.</value>
    [JsonPropertyName("localizationFile")]
    public EventAssetDefinition? LocalizationFile { get; init; }

    /// <summary>Gets or sets the event-start notification text.</summary>
    /// <value>The <c>notification</c> text sent when the event begins, or <see langword="null"/> when the event sends no start notification.</value>
    [JsonPropertyName("notification")]
    public string? Notification { get; init; }

    /// <summary>Gets or sets an export in the game's event poster asset.</summary>
    /// <value>The <c>posterCustomExportName</c> naming which export of the built-in poster asset to draw, or <see langword="null"/> when the event uses <see cref="PosterImage"/> or no poster.</value>
    [JsonPropertyName("posterCustomExportName")]
    public string? PosterExportName { get; init; }

    /// <summary>Gets or sets a downloadable poster image.</summary>
    /// <value>The path and SHA-1 fingerprint of the <c>posterWebImage</c> to download, or <see langword="null"/> when the event has no web poster.</value>
    [JsonPropertyName("posterWebImage")]
    public EventAssetDefinition? PosterImage { get; init; }

    /// <summary>Gets or sets the decoration chosen for the event-board reward preview.</summary>
    /// <value>The <c>shownDecorationReward</c> decoration row name featured in the preview, or <see langword="null"/> when no decoration is featured.</value>
    [JsonPropertyName("shownDecorationReward")]
    public string? PreviewDecoration { get; init; }

    /// <summary>Gets or sets the preview's reward count; absence retains the farm's configured default.</summary>
    /// <value>How many rewards the event-board preview lists, read from <c>show_n_rewards</c>, or <see langword="null"/> to keep the default count.</value>
    [JsonPropertyName("show_n_rewards")]
    public int? PreviewRewardCount { get; init; }

    /// <summary>Gets or sets the reward-track name used for the event-board preview.</summary>
    /// <value>The <c>shownRewardTrack</c> name of the reward track whose rewards the preview shows, or <see langword="null"/> when the event names none.</value>
    [JsonPropertyName("shownRewardTrack")]
    public string? PreviewRewardTrack { get; init; }

    /// <summary>Gets or sets the optional override of the event's action button.</summary>
    /// <value><see langword="true"/> if the event forces the call-to-action button on; otherwise, <see langword="false"/> to force it off, or <see langword="null"/> when <c>show_call_to_action_button</c> is absent and the board's default applies.</value>
    [JsonPropertyName("show_call_to_action_button")]
    public bool? ShowActionButton { get; init; }

    /// <summary>Gets or sets the optional override of the reminder button.</summary>
    /// <value><see langword="true"/> if the event forces the set-a-reminder button on; otherwise, <see langword="false"/> to force it off, or <see langword="null"/> when <c>show_set_a_reminder_button</c> is absent and the board's default applies.</value>
    [JsonPropertyName("show_set_a_reminder_button")]
    public bool? ShowReminderButton { get; init; }

    /// <summary>Gets or sets the event-board description.</summary>
    /// <value>The <c>text</c> body shown under the title on the event board while the event runs, or <see langword="null"/> when the event omits it.</value>
    [JsonPropertyName("text")]
    public string? Text { get; init; }

    /// <summary>Gets or sets the event-board title.</summary>
    /// <value>The <c>title</c> heading of the event's board entry while the event runs, or <see langword="null"/> when the event omits it.</value>
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    /// <summary>Gets or sets the event-board text displayed after participation ends.</summary>
    /// <value>The <c>visibleEndText</c> that replaces <see cref="Text"/> once players can no longer take part but the event is still listed, or <see langword="null"/> when the event omits it.</value>
    [JsonPropertyName("visibleEndText")]
    public string? VisibleEndText { get; init; }

    /// <summary>Gets or sets the event-board title displayed after participation ends.</summary>
    /// <value>The <c>visibleEndTitle</c> that replaces <see cref="Title"/> in that same after-participation period, or <see langword="null"/> when the event omits it.</value>
    [JsonPropertyName("visibleEndTitle")]
    public string? VisibleEndTitle { get; init; }
}
