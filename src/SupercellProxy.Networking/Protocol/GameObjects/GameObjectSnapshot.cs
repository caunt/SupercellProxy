using SupercellProxy.Networking.Protocol.Boats;
using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Timing;
using SupercellProxy.Networking.Protocol.Boosters;
using SupercellProxy.Networking.Protocol.Orders;
using SupercellProxy.Networking.Protocol.Visitors;
using SupercellProxy.Networking.Protocol.Town;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// Represents decoded <c language="csharp">GameObjectSnapshot</c> home data.
/// </summary>
public sealed partial record GameObjectSnapshot : ExtensibleDocument
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
    /// <summary>Gets per-stand roadside advertisement timers, in native timer ticks.</summary>
    [JsonPropertyName("AdTimers")]
    public int[]? AdvertisementTimers { get; init; }
    /// <summary>Gets the fishing helper's saved activity and orders.</summary>
    [JsonPropertyName("Helper")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HelperHouseStateSnapshot? AngusHelper { get; init; }

    /// <summary>Gets the uncollected pearls stored in Angus's bucket.</summary>
    [JsonPropertyName("PearlCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AngusPearlCount { get; init; }

    /// <summary>Gets the passenger's sanctuary-visit bonus row.</summary>
    [JsonPropertyName("AnimalBonusID")]
    public int? AnimalBonusId { get; init; }
    /// <summary>Gets the retained AnimalHabitatIndex value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AnimalHabitatIndex { get; init; }

    /// <summary>Gets the retained adult-pet animation index, read only when pet animation indices are part of logic.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AnimationIndex { get; init; }

    /// <summary>Gets per-stand automatic-buyer timers for a roadside shop.</summary>
    [JsonPropertyName("AITimer")]
    public int[] AutomaticBuyerTimers { get; init; } = [];
    /// <summary>Gets the saved Balloon height counter.</summary>
    [JsonPropertyName("Height")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? BalloonHeight { get; init; }



    /// <summary>Gets the Balloon reward row selected when it was popped.</summary>
    [JsonPropertyName("rewardIndex")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? BalloonRewardIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">BeatsTimer</c> value.
    /// </summary>
    public int? BeatsTimer { get; init; }
    /// Gets the retained boat-order groups.
    [JsonPropertyName("boat_orders")]
    public BoatOrderSnapshot[] BoatOrders { get; init; } = [];

    /// <summary>Gets whether the current boat has been seen.</summary>
    [JsonPropertyName("boat_seen")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? BoatSeen { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">BoosterList</c> value.
    /// </summary>
    public BoosterListSnapshot? BoosterList { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">BoughtSpins</c> value.
    /// </summary>
    public int BoughtSpins { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">BoughtSpinsDaily</c> value.
    /// </summary>
    public int BoughtSpinsDaily { get; init; }

    /// <summary>Gets saved bowl values, represented as food counts or boolean states by object type.</summary>
    [JsonPropertyName("Bowls")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BowlValueSnapshot[]? BowlValues { get; init; }

    /// <summary>Gets the Boy's pending interval-offer state.</summary>
    public int BoyOffer { get; init; }

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

    /// <summary>Gets the town service coin-bonus upgrade level.</summary>
    public int? CoinBonus { get; init; }

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

    /// <summary>Gets whether the creature has been collected.</summary>
    [JsonPropertyName("Collected")]
    public bool? CreatureCollected { get; init; }

    /// <summary>Gets the creature's spawning-rule id.</summary>
    [JsonPropertyName("SpawningRuleID")]
    public int? CreatureSpawningRuleId { get; init; }

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
    public int DataGlobalId { get; init; }

    /// <summary>Gets the retained DeliveryRewards value.</summary>
    [JsonPropertyName("rewards")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TruckDeliveryRewardSnapshot[]? DeliveryRewards { get; init; }

    /// <summary>Gets the sanctuary animal requested by the passenger.</summary>
    [JsonPropertyName("DesiredAnimalID")]
    public int? DesiredAnimalId { get; init; }

    /// <summary>Gets whether a tree or bush has started its native clearing sequence.</summary>
    public bool DestructionStarted { get; init; }

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
    public int EventId { get; init; }

    /// <summary>Gets the town service experience and reputation upgrade level.</summary>
    [JsonPropertyName("ExpBonus")]
    public int? ExpAndRepBonus { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">FarmPassSpins</c> value.
    /// </summary>
    public int FarmPassSpins { get; init; }

    /// <summary>Gets the retained Fed value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Fed { get; init; }

    /// <summary>Gets the last finished town-passenger service index when saved.</summary>
    public int? FinishedServiceIndex { get; init; }

    /// <summary>Gets the duck's retained random value selected when processing starts.</summary>
    [JsonPropertyName("ARand")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? FishingAnimalRandomValue { get; init; }

    /// <summary>Gets the fishing-spot instance index; an omitted value denotes index zero.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? FishingSpotIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">FlashFruitIndex</c> value.
    /// </summary>
    public int FlashFruitIndex { get; init; }

    /// <summary>Gets the quantities generated for the Boy's current offers.</summary>
    public int[] FoundItemAmounts { get; init; } = [];

    /// <summary>Gets the good selected for the Boy's current search.</summary>
    [JsonPropertyName("FoundItemID")]
    public int FoundItemGlobalId { get; init; }

    /// <summary>Gets the prices generated for the Boy's current offers.</summary>
    public int[] FoundItemPrices { get; init; } = [];

    /// <summary>Gets the roadside free-advertisement cooldown.</summary>
    [JsonPropertyName("FreeAdTimer")]
    public TimerSnapshot? FreeAdvertisementTimer { get; init; }

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

    /// <summary>Gets the retained time-limited gift-mailbox slot.</summary>
    [JsonPropertyName("slot4")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public GiftMailboxSlotSnapshot? GiftMailboxSlot4 { get; init; }

    /// <summary>Gets the native acquisition reason assigned to this gift.</summary>
    [JsonPropertyName("CustomTag")]
    public int GiftRewardReason { get; init; }

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
    public int GoodGlobalId { get; init; }

    /// Gets the reward-target sentinel used by mystery-box placement reconciliation.
    [JsonPropertyName("GoodGlobalIdToReward")]
    public int GoodGlobalIdToReward { get; init; }

    /// <summary>Gets the retained GoodGlobalIds value.</summary>
    [JsonPropertyName("GoodGlobalIds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int[]? GoodGlobalIds { get; init; }

    /// <summary>Gets the retained GrowTimer value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? GrowTimer { get; init; }

    /// <summary>Gets whether the saved object explicitly supplied its random seed.</summary>
    [JsonIgnore]
    public bool HasRandomSeed { get; private set; }

    /// <summary>Gets the passenger's id in the avatar's help sequence.</summary>
    [JsonPropertyName("HID")]
    public int? HelpId { get; init; }

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
    public int ItemGlobalId { get; init; }

    /// <summary>Gets the retained collection-area item list.</summary>
    [JsonPropertyName("ItemList")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ItemQuantityListSnapshot? ItemList { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">JackpotCount</c> value.
    /// </summary>
    public int JackpotCount { get; init; }

    /// <summary>Gets the last roadside advertisement timestamp.</summary>
    public long LastAdvertisementTimestamp { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LastDailyResetHourIndex</c> value.
    /// </summary>
    public int LastDailyResetHourIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LastEventID</c> value.
    /// </summary>
    [JsonPropertyName("LastEventID")]
    public int LastEventId { get; init; }

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
    public int LinkedGlobalId { get; init; }

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

    /// <summary>Gets the passenger's displayed name index.</summary>
    public int? NameIndex { get; init; }

    /// <summary>Gets a fishing spot's retained net or trap and drops.</summary>
    public Fishing.FishingNetSnapshot? Net { get; init; }

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

    /// <summary>Gets the passenger's completion-bonus quantity.</summary>
    [JsonPropertyName("BonusAmount")]
    public int? PassengerBonusAmount { get; init; }

    /// <summary>Gets whether the passenger's completion bonus belongs to an event.</summary>
    [JsonPropertyName("BonusEvent")]
    public bool? PassengerBonusEvent { get; init; }

    /// <summary>Gets the legacy passenger completion-bonus row.</summary>
    [JsonPropertyName("BonusID")]
    public int? PassengerBonusId { get; init; }

    /// <summary>Gets the passenger's completion-bonus resource.</summary>
    [JsonPropertyName("BonusID2")]
    public int? PassengerBonusResourceId { get; init; }

    /// <summary>Gets the high part of the passenger's pickup-origin home id.</summary>
    [JsonPropertyName("PH")]
    public int? PassengerOriginHigh { get; init; }

    /// <summary>Gets the low part of the passenger's pickup-origin home id.</summary>
    [JsonPropertyName("PL")]
    public int? PassengerOriginLow { get; init; }

    /// <summary>Gets the number of personal-train pickups for this passenger.</summary>
    [JsonPropertyName("PC")]
    public int? PassengerPickupCount { get; init; }

    /// <summary>Gets the passenger spawner's private service-request RNG state.</summary>
    [JsonPropertyName("ServiceRandomizer")]
    public int? PassengerServiceRandomState { get; init; }

    /// <summary>Gets saved town-passenger service requests.</summary>
    public TownPassengerServiceSnapshot[]? PassengerServices { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PaymentObjectAmount</c> value.
    /// </summary>
    public int PaymentObjectAmount { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PaymentObjectGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("PaymentObjectGlobalId")]
    public int PaymentObjectGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PeopleQuestV2</c> value.
    /// </summary>
    public EncodedDocumentValue? PeopleQuestV2 { get; init; }

    /// <summary>Gets the retained index of an adult pet's habitat among the home's pet habitats.</summary>
    [JsonPropertyName("Habitat")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? PetHabitatIndex { get; init; }

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
    public int PrizeGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PrizeType</c> value.
    /// </summary>
    public int PrizeType { get; init; }

    /// <summary>Gets the retained ProductionList value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProductionListSnapshot? ProductionList { get; init; }

    /// <summary>Gets accumulated production minutes used by legacy building mastery.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ProductionTime { get; init; }
    /// <summary>
    /// Gets or sets the <c language="csharp">RandomSeed</c> value.
    /// </summary>
    public int RandomSeed
    {
        get;
        init
        {
            field = value;
            HasRandomSeed = true;
        }
    }
    /// <summary>Gets the fishing spot's saved random-seed migration version.</summary>
    public int RandomSeedFix { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">SlotStates</c> value.
    /// </summary>
    public PeopleSpawnerSlotSnapshot[] SlotStates { get; init; } = [];

    /// <summary>Gets the retained Seed value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Seed { get; init; }

    /// <summary>Gets the selected boat order; old saves omit this field and select zero.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SelectedBoatOrderIndex { get; init; }

    /// <summary>Gets the selected index in the Boy's current offer.</summary>
    [JsonPropertyName("SelectedOfferIndex")]
    public int? SelectedBoyOfferIndex { get; init; }

    /// <summary>Gets the current town-passenger service index when saved.</summary>
    public int? ServiceIndex { get; init; }

    /// <summary>Gets saved town service entries.</summary>
    public TownServiceListSnapshot? ServiceList { get; init; }

    /// <summary>Gets the town service-slot upgrade level.</summary>
    public int? ServiceSlots { get; init; }

    /// <summary>Gets a lobster or duck's slot in its processing facility.</summary>
    public int Slot { get; init; }

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

    /// <summary>Gets the number of ordinary passengers spawned for the current town train.</summary>
    [JsonPropertyName("Spawn")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SpawnedTrainPassengerCount { get; init; }

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

    /// <summary>Gets the passenger's retained walking destination.</summary>
    public int? TargetAccurateX { get; init; }

    /// <summary>Gets the passenger's retained walking destination.</summary>
    public int? TargetAccurateY { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TargetData</c> value.
    /// </summary>
    public int TargetData { get; init; }

    /// <summary>Gets the passenger request currently associated with a service building.</summary>
    public int? TargetPassengerServiceIndex { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TargetX</c> value.
    /// </summary>
    public int TargetX { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TargetY</c> value.
    /// </summary>
    public int TargetY { get; init; }

    /// <summary>Gets the town service time-reduction upgrade level.</summary>
    public int? TimeBonus { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Timer</c> value.
    /// </summary>
    public TimerValue Timer { get; init; }

    /// <summary>Gets the number of passengers assigned to a saved town train.</summary>
    [JsonPropertyName("Passengers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TrainPassengerCount { get; init; }

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

    /// <summary>Gets whether the passenger spawner retains the town tutorial's wandering region.</summary>
    [JsonPropertyName("TutorialWalkRandom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? UseTownTutorialWandering { get; init; }

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
