using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>GameObjectSnapshot</c> home data.
/// </summary>
public sealed record GameObjectSnapshot
{
    /// <summary>
    /// Gets or sets the <c>DataGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("ID")]
    public int DataGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the <c>X</c> value.
    /// </summary>
    public int? X { get; init; }

    /// <summary>
    /// Gets or sets the <c>Y</c> value.
    /// </summary>
    public int? Y { get; init; }

    /// <summary>
    /// Gets or sets the <c>AccurateX</c> value.
    /// </summary>
    public int? AccurateX { get; init; }

    /// <summary>
    /// Gets or sets the <c>AccurateY</c> value.
    /// </summary>
    public int? AccurateY { get; init; }

    /// <summary>
    /// Gets or sets the <c>Mirrored</c> value.
    /// </summary>
    public bool Mirrored { get; init; }

    /// <summary>
    /// Gets or sets the <c>BoosterList</c> value.
    /// </summary>
    public BoosterSnapshot[]? BoosterList { get; init; }

    /// <summary>
    /// Gets or sets the <c>Timer</c> value.
    /// </summary>
    public JsonElement Timer { get; init; }

    /// <summary>
    /// Gets or sets the <c>BeatsTimer</c> value.
    /// </summary>
    public int? BeatsTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c>State</c> value.
    /// </summary>
    public int State { get; init; }

    /// <summary>
    /// Gets or sets the <c>Rank</c> value.
    /// </summary>
    public int Rank { get; init; } = 1;

    /// <summary>
    /// Gets or sets the <c>MasteryGatherCount</c> value.
    /// </summary>
    [JsonPropertyName("Gather")]
    public int MasteryGatherCount { get; init; }

    /// <summary>
    /// Gets or sets the <c>BrokenParts</c> value.
    /// </summary>
    public bool[]? BrokenParts { get; init; }

    /// <summary>
    /// Gets or sets the <c>UpgradeTimer</c> value.
    /// </summary>
    public JsonElement UpgradeTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c>UpgradeReady</c> value.
    /// </summary>
    public bool UpgradeReady { get; init; }

    /// <summary>
    /// Gets or sets the <c>GathererNestIndex</c> value.
    /// </summary>
    public int GathererNestIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c>GathererMineIndex</c> value.
    /// </summary>
    public int GathererMineIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c>CarryingResources</c> value.
    /// </summary>
    public bool CarryingResources { get; init; }

    /// <summary>
    /// Gets or sets the <c>GathererAiState</c> value.
    /// </summary>
    [JsonPropertyName("AIState")]
    public int GathererAiState { get; init; }

    /// <summary>
    /// Gets or sets the <c>TravelTime</c> value.
    /// </summary>
    public int TravelTime { get; init; }

    /// <summary>
    /// Gets or sets the <c>TargetX</c> value.
    /// </summary>
    public int TargetX { get; init; }

    /// <summary>
    /// Gets or sets the <c>TargetY</c> value.
    /// </summary>
    public int TargetY { get; init; }

    /// <summary>
    /// Gets or sets the <c>FlashFruitIndex</c> value.
    /// </summary>
    public int FlashFruitIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c>ConstructionTimer</c> value.
    /// </summary>
    public JsonElement ConstructionTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c>TargetData</c> value.
    /// </summary>
    public int TargetData { get; init; }

    /// <summary>
    /// Gets or sets the <c>ItemGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("ItemID")]
    public int ItemGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the <c>Count</c> value.
    /// </summary>
    public int Count { get; init; }

    /// <summary>
    /// Gets or sets the <c>NextPoint</c> value.
    /// </summary>
    public int NextPoint { get; init; }

    /// <summary>
    /// Gets or sets the <c>LinkedGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("GlobalId")]
    public int LinkedGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the <c>GoodGlobalId</c> value.
    /// </summary>
    public int GoodGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the <c>GoodAmount</c> value.
    /// </summary>
    public int GoodAmount { get; init; }

    /// <summary>
    /// Gets or sets the <c>PaymentObjectAmount</c> value.
    /// </summary>
    public int PaymentObjectAmount { get; init; }

    /// <summary>
    /// Gets or sets the <c>PaymentObjectGlobalId</c> value.
    /// </summary>
    public int PaymentObjectGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the <c>EventId</c> value.
    /// </summary>
    [JsonPropertyName("EventID")]
    public int EventId { get; init; }

    /// <summary>
    /// Gets or sets the <c>GiftGid</c> value.
    /// </summary>
    public int GiftGid { get; init; }

    /// <summary>
    /// Gets or sets the <c>GiftAmount</c> value.
    /// </summary>
    public int GiftAmount { get; init; }

    /// <summary>
    /// Gets or sets the <c>SpawnedFromTutorial</c> value.
    /// </summary>
    public bool SpawnedFromTutorial { get; init; }

    /// <summary>
    /// Gets or sets the <c>SpawnedFromV2</c> value.
    /// </summary>
    public bool SpawnedFromV2 { get; init; }

    /// <summary>
    /// Gets or sets the <c>PeopleQuestV2</c> value.
    /// </summary>
    public JsonElement PeopleQuestV2 { get; init; }

    /// <summary>
    /// Gets or sets the <c>TutorialPeopleSpawned</c> value.
    /// </summary>
    public int TutorialPeopleSpawned { get; init; }

    /// <summary>
    /// Gets or sets the <c>SlotStates</c> value.
    /// </summary>
    public PeopleSpawnerSlotSnapshot[] SlotStates { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>DailyResetTime</c> value.
    /// </summary>
    public int DailyResetTime { get; init; }

    /// <summary>
    /// Gets or sets the <c>DailyVisitorsSpawned</c> value.
    /// </summary>
    public int DailyVisitorsSpawned { get; init; }

    /// <summary>
    /// Gets or sets the <c>Orders</c> value.
    /// </summary>
    public OrderSnapshot[] Orders { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>LastInitDayIndex</c> value.
    /// </summary>
    public int LastInitDayIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c>LastInitHourIndex</c> value.
    /// </summary>
    public int LastInitHourIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c>LastDailyResetHourIndex</c> value.
    /// </summary>
    public int LastDailyResetHourIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c>LastSpinDayIndex</c> value.
    /// </summary>
    public int LastSpinDayIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c>ConsecutiveSpinDays</c> value.
    /// </summary>
    public int ConsecutiveSpinDays { get; init; }

    /// <summary>
    /// Gets or sets the <c>NumSpins</c> value.
    /// </summary>
    public int NumSpins { get; init; }

    /// <summary>
    /// Gets or sets the <c>JackpotCount</c> value.
    /// </summary>
    public int JackpotCount { get; init; }

    /// <summary>
    /// Gets or sets the <c>PrizeType</c> value.
    /// </summary>
    public int PrizeType { get; init; }

    /// <summary>
    /// Gets or sets the <c>PrizeGlobalID</c> value.
    /// </summary>
    public int PrizeGlobalID { get; init; }

    /// <summary>
    /// Gets or sets the <c>PrizeCount</c> value.
    /// </summary>
    public int PrizeCount { get; init; }

    /// <summary>
    /// Gets or sets the <c>IsPrizeFromEvent</c> value.
    /// </summary>
    public bool IsPrizeFromEvent { get; init; }

    /// <summary>
    /// Gets or sets the <c>BoughtSpins</c> value.
    /// </summary>
    public int BoughtSpins { get; init; }

    /// <summary>
    /// Gets or sets the <c>BoughtSpinsDaily</c> value.
    /// </summary>
    public int BoughtSpinsDaily { get; init; }

    /// <summary>
    /// Gets or sets the <c>FarmPassSpins</c> value.
    /// </summary>
    public int FarmPassSpins { get; init; }

    /// <summary>
    /// Gets or sets the <c>WheelPrizes</c> value.
    /// </summary>
    public int[][] WheelPrizes { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>WheelAmounts</c> value.
    /// </summary>
    public int[][] WheelAmounts { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>RandomSeed</c> value.
    /// </summary>
    public int RandomSeed { get; init; }

    /// <summary>
    /// Gets or sets the <c>LastEventID</c> value.
    /// </summary>
    public int LastEventID { get; init; }

    /// <summary>
    /// Gets or sets the <c>UsedSpins</c> value.
    /// </summary>
    public int UsedSpins { get; init; }

    /// <summary>
    /// Gets or sets the <c>AdsSpins</c> value.
    /// </summary>
    public int AdsSpins { get; init; }

    /// <summary>
    /// Gets or sets the <c>HireTimer</c> value.
    /// </summary>
    public JsonElement HireTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c>CooldownTimer</c> value.
    /// </summary>
    public JsonElement CooldownTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c>OfferTimer</c> value.
    /// </summary>
    public JsonElement OfferTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c>IntervalOfferTimer</c> value.
    /// </summary>
    public JsonElement IntervalOfferTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c>HireEnded</c> value.
    /// </summary>
    public bool HireEnded { get; init; }

    /// <summary>
    /// Gets or sets the <c>IntervalOfferActive</c> value.
    /// </summary>
    public bool IntervalOfferActive { get; init; }

    /// <summary>
    /// Gets or sets the <c>FreeReEngagementAvailable</c> value.
    /// </summary>
    public bool FreeReEngagementAvailable { get; init; }

    /// <summary>
    /// Gets or sets the <c>Data</c> value.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement> Data { get; init; } =
        new Dictionary<string, JsonElement>(StringComparer.Ordinal);
}
