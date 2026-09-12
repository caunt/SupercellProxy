using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Timing;
using SupercellProxy.Networking.Protocol.Boats;
using SupercellProxy.Networking.Protocol.Boosters;
using SupercellProxy.Networking.Protocol.Orders;
using SupercellProxy.Networking.Protocol.Visitors;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// Represents decoded <c language="csharp">GameObjectSnapshot</c> home data.
/// </summary>
public sealed record GameObjectSnapshot : ExtensibleDocument
{

    /// <summary>
    /// Gets or sets the <c language="csharp">AccurateX</c> value.
    /// </summary>
    public int? AccurateX { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AccurateY</c> value.
    /// </summary>
    public int? AccurateY { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AdsSpins</c> value.
    /// </summary>
    public int AdsSpins { get; init; }



    /// <summary>Gets the retained AnimalHabitatIndex value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AnimalHabitatIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">BeatsTimer</c> value.
    /// </summary>
    public int? BeatsTimer { get; init; }

    /// Gets the retained boat-order groups.
    [JsonPropertyName("boat_orders")]
    public BoatOrderSnapshot[] BoatOrders { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">BoosterList</c> value.
    /// </summary>
    public BoosterSnapshot[]? BoosterList { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">BoughtSpins</c> value.
    /// </summary>
    public int BoughtSpins { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">BoughtSpinsDaily</c> value.
    /// </summary>
    public int BoughtSpinsDaily { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">BrokenParts</c> value.
    /// </summary>
    public bool[]? BrokenParts { get; init; }

    /// <summary>Gets the retained Caretaker value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HelperHouseStateSnapshot? Caretaker { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CarryingResources</c> value.
    /// </summary>
    public bool CarryingResources { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ConsecutiveSpinDays</c> value.
    /// </summary>
    public int ConsecutiveSpinDays { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ConstructionTimer</c> value.
    /// </summary>
    public TimerSnapshot? ConstructionTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CooldownTimer</c> value.
    /// </summary>
    public TimerSnapshot? CooldownTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Count</c> value.
    /// </summary>
    public int Count { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">DailyResetTime</c> value.
    /// </summary>
    public int DailyResetTime { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">DailyVisitorsSpawned</c> value.
    /// </summary>
    public int DailyVisitorsSpawned { get; init; }
    /// <summary>
    /// Gets or sets the <c language="csharp">DataGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("ID")]
    public int DataGlobalIdentifier { get; init; }

    /// <summary>Gets the retained DeliveryRewards value.</summary>
    [JsonPropertyName("rewards")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TruckDeliveryRewardSnapshot[]? DeliveryRewards { get; init; }

    /// <summary>Gets the retained DiamondCost value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? DiamondCost { get; init; }

    /// <summary>Gets the retained DiamondCostToTake value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? DiamondCostToTake { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">EventId</c> value.
    /// </summary>
    [JsonPropertyName("EventID")]
    public int EventIdentifier { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">FarmPassSpins</c> value.
    /// </summary>
    public int FarmPassSpins { get; init; }

    /// <summary>Gets the retained Fed value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Fed { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">FlashFruitIndex</c> value.
    /// </summary>
    public int FlashFruitIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">FreeReEngagementAvailable</c> value.
    /// </summary>
    public bool FreeReEngagementAvailable { get; init; }

    /// <summary>Gets the retained FriendLastOpened value.</summary>
    [JsonPropertyName("LastOpenedFriend")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FriendLastOpened { get; init; }

    /// <summary>Gets the retained FriendOpenedBoxTimer value.</summary>
    [JsonPropertyName("OpenedBoxTimerFriend")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? FriendOpenedBoxTimer { get; init; }

    /// <summary>Gets the retained FriendSpawnTimer value.</summary>
    [JsonPropertyName("TimerFriend")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? FriendSpawnTimer { get; init; }

    /// <summary>Gets the retained Fruits value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool[]? Fruits { get; init; }

    /// <summary>
    /// Gets the Gathered Resource value.
    /// </summary>
    public int GatheredResource { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">GathererAiState</c> value.
    /// </summary>
    [JsonPropertyName("AIState")]
    public int GathererAiState { get; init; }

    /// <summary>
    /// Gets the Gatherer Habitat Index value.
    /// </summary>
    public int GathererHabitatIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">GathererMineIndex</c> value.
    /// </summary>
    public int GathererMineIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">GathererNestIndex</c> value.
    /// </summary>
    public int GathererNestIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">GiftAmount</c> value.
    /// </summary>
    public int GiftAmount { get; init; }

    /// <summary>
    /// Gets the number of visitor gifts already processed by this spawner during its current daily
    /// cycle.
    /// </summary>
    [JsonPropertyName("GiftCnt")]
    public int GiftCount { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">GiftGid</c> value.
    /// </summary>
    public int GiftGid { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">GoodAmount</c> value.
    /// </summary>
    public int GoodAmount { get; init; }

    /// <summary>Gets the retained GoodAmountToReward value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? GoodAmountToReward { get; init; }

    /// <summary>Gets the retained GoodAmounts value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int[]? GoodAmounts { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">GoodGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("GoodGlobalId")]
    public int GoodGlobalIdentifier { get; init; }

    /// Gets the reward-target sentinel used by mystery-box placement reconciliation.
    [JsonPropertyName("GoodGlobalIdToReward")]
    public int GoodGlobalIdentifierToReward { get; init; }

    /// <summary>Gets the retained GoodGlobalIdentifiers value.</summary>
    [JsonPropertyName("GoodGlobalIds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int[]? GoodGlobalIdentifiers { get; init; }

    /// <summary>Gets the retained GrowTimer value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? GrowTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">HireEnded</c> value.
    /// </summary>
    public bool HireEnded { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">HireTimer</c> value.
    /// </summary>
    public TimerSnapshot? HireTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">X</c> value.
    /// </summary>
    [JsonPropertyName("X")]
    public int? HorizontalCoordinate { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">IntervalOfferActive</c> value.
    /// </summary>
    public bool IntervalOfferActive { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">IntervalOfferTimer</c> value.
    /// </summary>
    public TimerSnapshot? IntervalOfferTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">IsPrizeFromEvent</c> value.
    /// </summary>
    public bool IsPrizeFromEvent { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ItemGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("ItemID")]
    public int ItemGlobalIdentifier { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">JackpotCount</c> value.
    /// </summary>
    public int JackpotCount { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LastDailyResetHourIndex</c> value.
    /// </summary>
    public int LastDailyResetHourIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LastEventID</c> value.
    /// </summary>
    [JsonPropertyName("LastEventID")]
    public int LastEventIdentifier { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LastInitDayIndex</c> value.
    /// </summary>
    [JsonPropertyName("LastInitDayIndex")]
    public int LastInitializationDayIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LastInitHourIndex</c> value.
    /// </summary>
    [JsonPropertyName("LastInitHourIndex")]
    public int LastInitializationHourIndex { get; init; }

    /// <summary>Gets the retained LastOpened value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? LastOpened { get; init; }

    /// <summary>Gets the retained LastSignpostMaterials value.</summary>
    [JsonPropertyName("LastSignpostMats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? LastSignpostMaterials { get; init; }

    /// <summary>Gets the retained LastSignpostUpgrade value.</summary>
    [JsonPropertyName("LastSignpostUpg")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? LastSignpostUpgrade { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LastSpinDayIndex</c> value.
    /// </summary>
    public int LastSpinDayIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LinkedGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("GlobalId")]
    public int LinkedGlobalIdentifier { get; init; }

    /// <summary>Gets the retained Locked value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Locked { get; init; }

    /// <summary>Gets the retained LockedChecked value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? LockedChecked { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">MasteryGatherCount</c> value.
    /// </summary>
    [JsonPropertyName("Gather")]
    public int MasteryGatherCount { get; init; }

    /// <summary>Gets the retained Miller value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HelperHouseStateSnapshot? Miller { get; init; }

    /// <summary>Gets the retained MineCount value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MineCount { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Mirrored</c> value.
    /// </summary>
    public bool Mirrored { get; init; }

    /// <summary>
    /// Gets the retained timestamp that controls the visitor spawner's next daily refresh
    /// boundary.
    /// </summary>
    [JsonPropertyName("TimeStamp")]
    public int NextDailyRefreshTimestamp { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">NextPoint</c> value.
    /// </summary>
    public int NextPoint { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">NumSpins</c> value.
    /// </summary>
    [JsonPropertyName("NumSpins")]
    public int NumberSpins { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">OfferTimer</c> value.
    /// </summary>
    public TimerSnapshot? OfferTimer { get; init; }

    /// <summary>Gets the retained OpenedBoxTimer value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? OpenedBoxTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Rank</c> value.
    /// </summary>
    public int Rank { get; init; } = 1;

    /// <summary>
    /// Gets or sets the <c language="csharp">PaymentObjectAmount</c> value.
    /// </summary>
    public int PaymentObjectAmount { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PaymentObjectGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("PaymentObjectGlobalId")]
    public int PaymentObjectGlobalIdentifier { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PeopleQuestV2</c> value.
    /// </summary>
    public EncodedDocumentValue? PeopleQuestV2 { get; init; }

    /// <summary>Gets the retained PetTimer value.</summary>
    [JsonPropertyName("T")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? PetTimer { get; init; }

    /// <summary>Gets the retained Phase value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Phase { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PrizeCount</c> value.
    /// </summary>
    public int PrizeCount { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PrizeGlobalID</c> value.
    /// </summary>
    [JsonPropertyName("PrizeGlobalID")]
    public int PrizeGlobalIdentifier { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PrizeType</c> value.
    /// </summary>
    public int PrizeType { get; init; }

    /// <summary>Gets the retained ProductionList value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProductionListSnapshot? ProductionList { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">RandomSeed</c> value.
    /// </summary>
    public int RandomSeed { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">SlotStates</c> value.
    /// </summary>
    public PeopleSpawnerSlotSnapshot[] SlotStates { get; init; } = [];

    /// <summary>Gets the retained Seed value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Seed { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Orders</c> value.
    /// </summary>
    public OrderSnapshot[] Orders { get; init; } = [];

    /// <summary>Gets the retained SpawnedBoxCount value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SpawnedBoxCount { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">SpawnedFromTutorial</c> value.
    /// </summary>
    public bool SpawnedFromTutorial { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">SpawnedFromV2</c> value.
    /// </summary>
    public bool SpawnedFromV2 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">State</c> value.
    /// </summary>
    public int State { get; init; }

    /// Gets the retained state deadline used by stateful home objects.
    public int StateTimer { get; init; }

    /// Gets the retained duration of the boat's current dock or travel state.
    public int StayTime { get; init; }

    /// <summary>Gets the retained SubState value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SubState { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TargetData</c> value.
    /// </summary>
    public int TargetData { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TargetX</c> value.
    /// </summary>
    public int TargetX { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TargetY</c> value.
    /// </summary>
    public int TargetY { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Timer</c> value.
    /// </summary>
    public TimerValue Timer { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TravelTime</c> value.
    /// </summary>
    public int TravelTime { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TutorialPeopleSpawned</c> value.
    /// </summary>
    public int TutorialPeopleSpawned { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UpgradeReady</c> value.
    /// </summary>
    public bool UpgradeReady { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UpgradeTimer</c> value.
    /// </summary>
    public TimerSnapshot? UpgradeTimer { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UsedSpins</c> value.
    /// </summary>
    public int UsedSpins { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Y</c> value.
    /// </summary>
    [JsonPropertyName("Y")]
    public int? VerticalCoordinate { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">WheelPrizes</c> value.
    /// </summary>
    public int[][] WheelPrizes { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">WheelAmounts</c> value.
    /// </summary>
    public int[][] WheelAmounts { get; init; } = [];
}
