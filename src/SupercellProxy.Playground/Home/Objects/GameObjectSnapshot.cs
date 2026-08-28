using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">GameObjectSnapshot</c> home data.
/// </summary>
internal sealed record GameObjectSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">DataGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("ID")]
    public int DataGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">X</c> value.
    /// </summary>
    public int? X { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Y</c> value.
    /// </summary>
    public int? Y { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AccurateX</c> value.
    /// </summary>
    public int? AccurateX { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AccurateY</c> value.
    /// </summary>
    public int? AccurateY { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Mirrored</c> value.
    /// </summary>
    public bool Mirrored { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">BoosterList</c> value.
    /// </summary>
    public BoosterSnapshot[]? BoosterList { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Timer</c> value.
    /// </summary>
    public JsonElement Timer { get; init; }

    /// <summary>
    /// Gets the number of visitor gifts already processed by this spawner during its current daily
    /// cycle.
    /// </summary>
    [JsonPropertyName("GiftCnt")]
    public int GiftCount { get; init; }

    /// <summary>
    /// Gets the retained timestamp that controls the visitor spawner's next daily refresh
    /// boundary.
    /// </summary>
    [JsonPropertyName("TimeStamp")]
    public int NextDailyRefreshTimestamp { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">BeatsTimer</c> value.
    /// </summary>
    public int? BeatsTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">State</c> value.
    /// </summary>
    public int State { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Rank</c> value.
    /// </summary>
    public int Rank { get; init; } = 1;

    /// <summary>
    /// Gets or sets the <c language="csharp">MasteryGatherCount</c> value.
    /// </summary>
    [JsonPropertyName("Gather")]
    public int MasteryGatherCount { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">BrokenParts</c> value.
    /// </summary>
    public bool[]? BrokenParts { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UpgradeTimer</c> value.
    /// </summary>
    public JsonElement UpgradeTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UpgradeReady</c> value.
    /// </summary>
    public bool UpgradeReady { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">GathererNestIndex</c> value.
    /// </summary>
    public int GathererNestIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">GathererMineIndex</c> value.
    /// </summary>
    public int GathererMineIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CarryingResources</c> value.
    /// </summary>
    public bool CarryingResources { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">GathererAiState</c> value.
    /// </summary>
    [JsonPropertyName("AIState")]
    public int GathererAiState { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TravelTime</c> value.
    /// </summary>
    public int TravelTime { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TargetX</c> value.
    /// </summary>
    public int TargetX { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TargetY</c> value.
    /// </summary>
    public int TargetY { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">FlashFruitIndex</c> value.
    /// </summary>
    public int FlashFruitIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ConstructionTimer</c> value.
    /// </summary>
    public JsonElement ConstructionTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TargetData</c> value.
    /// </summary>
    public int TargetData { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ItemGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("ItemID")]
    public int ItemGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Count</c> value.
    /// </summary>
    public int Count { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">NextPoint</c> value.
    /// </summary>
    public int NextPoint { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LinkedGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("GlobalId")]
    public int LinkedGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">GoodGlobalId</c> value.
    /// </summary>
    public int GoodGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">GoodAmount</c> value.
    /// </summary>
    public int GoodAmount { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PaymentObjectAmount</c> value.
    /// </summary>
    public int PaymentObjectAmount { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PaymentObjectGlobalId</c> value.
    /// </summary>
    public int PaymentObjectGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">EventId</c> value.
    /// </summary>
    [JsonPropertyName("EventID")]
    public int EventId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">GiftGid</c> value.
    /// </summary>
    public int GiftGid { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">GiftAmount</c> value.
    /// </summary>
    public int GiftAmount { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">SpawnedFromTutorial</c> value.
    /// </summary>
    public bool SpawnedFromTutorial { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">SpawnedFromV2</c> value.
    /// </summary>
    public bool SpawnedFromV2 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PeopleQuestV2</c> value.
    /// </summary>
    public JsonElement PeopleQuestV2 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TutorialPeopleSpawned</c> value.
    /// </summary>
    public int TutorialPeopleSpawned { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">SlotStates</c> value.
    /// </summary>
    public PeopleSpawnerSlotSnapshot[] SlotStates { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">DailyResetTime</c> value.
    /// </summary>
    public int DailyResetTime { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">DailyVisitorsSpawned</c> value.
    /// </summary>
    public int DailyVisitorsSpawned { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Orders</c> value.
    /// </summary>
    public OrderSnapshot[] Orders { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">LastInitDayIndex</c> value.
    /// </summary>
    public int LastInitDayIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LastInitHourIndex</c> value.
    /// </summary>
    public int LastInitHourIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LastDailyResetHourIndex</c> value.
    /// </summary>
    public int LastDailyResetHourIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LastSpinDayIndex</c> value.
    /// </summary>
    public int LastSpinDayIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ConsecutiveSpinDays</c> value.
    /// </summary>
    public int ConsecutiveSpinDays { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">NumSpins</c> value.
    /// </summary>
    public int NumSpins { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">JackpotCount</c> value.
    /// </summary>
    public int JackpotCount { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PrizeType</c> value.
    /// </summary>
    public int PrizeType { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PrizeGlobalID</c> value.
    /// </summary>
    public int PrizeGlobalID { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PrizeCount</c> value.
    /// </summary>
    public int PrizeCount { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">IsPrizeFromEvent</c> value.
    /// </summary>
    public bool IsPrizeFromEvent { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">BoughtSpins</c> value.
    /// </summary>
    public int BoughtSpins { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">BoughtSpinsDaily</c> value.
    /// </summary>
    public int BoughtSpinsDaily { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">FarmPassSpins</c> value.
    /// </summary>
    public int FarmPassSpins { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">WheelPrizes</c> value.
    /// </summary>
    public int[][] WheelPrizes { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">WheelAmounts</c> value.
    /// </summary>
    public int[][] WheelAmounts { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">RandomSeed</c> value.
    /// </summary>
    public int RandomSeed { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LastEventID</c> value.
    /// </summary>
    public int LastEventID { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UsedSpins</c> value.
    /// </summary>
    public int UsedSpins { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AdsSpins</c> value.
    /// </summary>
    public int AdsSpins { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">HireTimer</c> value.
    /// </summary>
    public JsonElement HireTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CooldownTimer</c> value.
    /// </summary>
    public JsonElement CooldownTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">OfferTimer</c> value.
    /// </summary>
    public JsonElement OfferTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">IntervalOfferTimer</c> value.
    /// </summary>
    public JsonElement IntervalOfferTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">HireEnded</c> value.
    /// </summary>
    public bool HireEnded { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">IntervalOfferActive</c> value.
    /// </summary>
    public bool IntervalOfferActive { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">FreeReEngagementAvailable</c> value.
    /// </summary>
    public bool FreeReEngagementAvailable { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Data</c> value.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement> Data { get; init; } =
        new Dictionary<string, JsonElement>(StringComparer.Ordinal);
}
