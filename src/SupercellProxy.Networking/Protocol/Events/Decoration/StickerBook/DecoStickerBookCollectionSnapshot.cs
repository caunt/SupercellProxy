using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.StickerBook;

/// <summary>The saved progress of one sticker-book collection.</summary>
public sealed record DecoStickerBookCollectionSnapshot
{
    /// <summary>Gets the state of each collection slot: 0 not collected, 1 newly collected, 2 collected and acknowledged.</summary>
    [JsonPropertyName("decoState")]
    public int[] DecorationStates { get; init; } = [];

    /// <summary>Gets the event the collection runs in.</summary>
    [JsonPropertyName("eventId")]
    public int EventId { get; init; }

    /// <summary>Gets whether the collection's event has a notification the player has not acknowledged.</summary>
    [JsonPropertyName("eventNotification")]
    public bool EventNotification { get; init; }

    /// <summary>Gets whether the collection's event is running.</summary>
    [JsonPropertyName("eventRunning")]
    public bool EventRunning { get; init; }

    /// <summary>Gets the milestone rewards earned by collecting decorations, highest requirement first.</summary>
    [JsonPropertyName("milestones")]
    public DecoStickerBookMilestoneSnapshot[] Milestones { get; init; } = [];

    /// <summary>Gets the collection's notification state; 0 means nothing is waiting to be acknowledged.</summary>
    [JsonPropertyName("notificationState")]
    public int NotificationState { get; init; }

    /// <summary>Gets how many of the reward item completing the collection gives.</summary>
    [JsonPropertyName("rewardAmount")]
    public int RewardAmount { get; init; }

    /// <summary>Gets whether the collection reward can be claimed.</summary>
    [JsonPropertyName("rewardClaimable")]
    public bool RewardClaimable { get; init; }

    /// <summary>Gets whether the collection reward was claimed.</summary>
    [JsonPropertyName("rewardClaimed")]
    public bool RewardClaimed { get; init; }

    /// <summary>Gets the data row of the collection reward.</summary>
    [JsonPropertyName("rewardData")]
    public int RewardDataGlobalId { get; init; }

    /// <summary>Gets the decoration saved in each collection slot.</summary>
    [JsonPropertyName("savedDecoGlobalID")]
    public int[] SavedDecorationGlobalIds { get; init; } = [];

    /// <summary>Gets the event variant the collection runs in.</summary>
    [JsonPropertyName("variantId")]
    public int VariantId { get; init; }
}
