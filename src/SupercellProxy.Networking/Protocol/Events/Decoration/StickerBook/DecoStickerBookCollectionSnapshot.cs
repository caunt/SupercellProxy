using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.StickerBook;

/// <summary>The saved progress of one sticker-book collection.</summary>
public sealed record DecoStickerBookCollectionSnapshot
{
    /// <summary>Gets or sets the state of each collection slot: 0 not collected, 1 newly collected, 2 collected and acknowledged.</summary>
    /// <value>One entry per decoration the collection lists (saved as <c>decoState</c>); empty by default.</value>
    [JsonPropertyName("decoState")]
    public int[] DecorationStates { get; init; } = [];

    /// <summary>Gets or sets the event the collection runs in.</summary>
    /// <value>The live event id the collection is tied to, or 0 when it is not tied to one.</value>
    [JsonPropertyName("eventId")]
    public int EventId { get; init; }

    /// <summary>Gets or sets whether the collection's event has a notification the player has not acknowledged.</summary>
    /// <value><see langword="true"/> if the event notification is pending; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("eventNotification")]
    public bool EventNotification { get; init; }

    /// <summary>Gets or sets whether the collection's event is running.</summary>
    /// <value><see langword="true"/> if the client saved the collection's event as running; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("eventRunning")]
    public bool EventRunning { get; init; }

    /// <summary>Gets or sets the milestone rewards earned by collecting decorations, highest requirement first.</summary>
    /// <value>The milestones in saved order, whose positions are the reward indexes the milestone claim command uses; empty when the collection has none.</value>
    [JsonPropertyName("milestones")]
    public DecoStickerBookMilestoneSnapshot[] Milestones { get; init; } = [];

    /// <summary>Gets or sets the collection's notification state; 0 means nothing is waiting to be acknowledged.</summary>
    /// <value>A count of newly obtained decorations the player has not acknowledged yet.</value>
    [JsonPropertyName("notificationState")]
    public int NotificationState { get; init; }

    /// <summary>Gets or sets how many of the reward item completing the collection gives.</summary>
    /// <value>The quantity of <see cref="RewardDataGlobalId"/> granted by the completion claim.</value>
    [JsonPropertyName("rewardAmount")]
    public int RewardAmount { get; init; }

    /// <summary>Gets or sets whether the collection reward can be claimed.</summary>
    /// <value><see langword="true"/> if the save marks the completion reward as ready to claim; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("rewardClaimable")]
    public bool RewardClaimable { get; init; }

    /// <summary>Gets or sets whether the collection reward was claimed.</summary>
    /// <value><see langword="true"/> if the completion reward has been granted; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("rewardClaimed")]
    public bool RewardClaimed { get; init; }

    /// <summary>Gets or sets the data row of the collection reward.</summary>
    /// <value>The data-table global id of the completion reward (saved as <c>rewardData</c>).</value>
    [JsonPropertyName("rewardData")]
    public int RewardDataGlobalId { get; init; }

    /// <summary>Gets or sets the decoration saved in each collection slot.</summary>
    /// <value>One data-table global id per slot, parallel to <see cref="DecorationStates"/> (saved as <c>savedDecoGlobalID</c>).</value>
    [JsonPropertyName("savedDecoGlobalID")]
    public int[] SavedDecorationGlobalIds { get; init; } = [];

    /// <summary>Gets or sets the event variant the collection runs in.</summary>
    /// <value>The variant id of the event named by <see cref="EventId"/>, or 0 when there is none.</value>
    [JsonPropertyName("variantId")]
    public int VariantId { get; init; }
}
