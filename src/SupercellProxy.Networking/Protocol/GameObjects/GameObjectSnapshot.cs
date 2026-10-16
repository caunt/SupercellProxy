using SupercellProxy.Networking.Protocol.Boats;
using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Protocol.Fishing;
using SupercellProxy.Networking.Protocol.Timing;
using SupercellProxy.Networking.Protocol.Boosters;
using SupercellProxy.Networking.Protocol.Orders;
using SupercellProxy.Networking.Protocol.Visitors;
using SupercellProxy.Networking.Protocol.Town;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>
/// Saved state of one object in a home, such as a building, field, tree, animal, character or decoration; which fields are set
/// depends on the kind of object.
/// </summary>
public sealed partial record GameObjectSnapshot : ExtensibleDocument
{
    /// <summary>Exact horizontal map position in map units (512 per tile); null when only the tile position is saved.</summary>
    public int? AccurateX { get; init; }

    /// <summary>Exact vertical map position in map units (512 per tile); null when only the tile position is saved.</summary>
    public int? AccurateY { get; init; }

    /// <summary>Wheel spins earned by watching videos and not used yet.</summary>
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

    /// <summary>Production seconds of the product a processing-building booster sped up; null or 0 when none did.</summary>
    public int? BeatsTimer { get; init; }
    /// <summary>Gets the retained boat-order groups.</summary>
    [JsonPropertyName("boat_orders")]
    public BoatOrderSnapshot[] BoatOrders { get; init; } = [];

    /// <summary>Gets whether the current boat has been seen.</summary>
    [JsonPropertyName("boat_seen")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? BoatSeen { get; init; }

    /// <summary>Boosters attached to the building; null when it has none.</summary>
    public BoosterListSnapshot? BoosterList { get; init; }

    /// <summary>Wheel spins bought with diamonds and not used yet.</summary>
    public int BoughtSpins { get; init; }

    /// <summary>Wheel spins bought since the wheel car last arrived.</summary>
    public int BoughtSpinsDaily { get; init; }

    /// <summary>Gets saved bowl values, represented as food counts or boolean states by object type.</summary>
    [JsonPropertyName("Bowls")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BowlValueSnapshot[]? BowlValues { get; init; }

    /// <summary>Gets the Boy's pending interval-offer state.</summary>
    public int BoyOffer { get; init; }

    /// <summary>Which parts of the object are broken, one flag per part; null for objects without parts.</summary>
    public bool[]? BrokenParts { get; init; }

    /// <summary>Gets the retained Caretaker value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HelperHouseStateSnapshot? Caretaker { get; init; }

    /// <summary>Whether the gatherer is flying back to its nest with gathered goods.</summary>
    public bool CarryingResources { get; init; }

    /// <summary>Gets the town service coin-bonus upgrade level.</summary>
    public int? CoinBonus { get; init; }

    /// <summary>Days in a row the wheel was spun; a gap of three days or more starts the count over at 1.</summary>
    public int ConsecutiveSpinDays { get; init; }

    /// <summary>Countdown of a construction site until its building is finished.</summary>
    public TimerSnapshot? ConstructionTimer { get; init; }

    /// <summary>Cooldown countdown, such as the Boy's rest or a fishing spot's wait before it can be used again.</summary>
    public TimerSnapshot? CooldownTimer { get; init; }

    /// <summary>How many items the object holds, such as the goods in the Boy's box.</summary>
    public int Count { get; init; }

    /// <summary>Gets whether the creature has been collected.</summary>
    [JsonPropertyName("Collected")]
    public bool? CreatureCollected { get; init; }

    /// <summary>Gets the creature's spawning-rule id.</summary>
    [JsonPropertyName("SpawningRuleID")]
    public int? CreatureSpawningRuleId { get; init; }

    /// <summary>Gets the player-selected pet or sanctuary animal name.</summary>
    [JsonPropertyName("Name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CustomName { get; init; }

    /// <summary>Time of the object's daily reset as saved; the client logic does not read it.</summary>
    public int DailyResetTime { get; init; }

    /// <summary>Visitors a spawner spawned today as saved; the client logic does not read it.</summary>
    public int DailyVisitorsSpawned { get; init; }

    /// <summary>Global id of the object's table entry (saved as <c>ID</c>).</summary>
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

    /// <summary>Gets the remaining timer of a wild obstacle that is being cleared.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? DestructionTimer { get; init; }

    /// <summary>Gets the retained DiamondCost value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? DiamondCost { get; init; }

    /// <summary>Gets the retained DiamondCostToTake value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? DiamondCostToTake { get; init; }

    /// <summary>Live event the object is tied to (saved as <c>EventID</c>); 0 when none.</summary>
    [JsonPropertyName("EventID")]
    public int EventId { get; init; }

    /// <summary>Gets the town service experience and reputation upgrade level.</summary>
    [JsonPropertyName("ExpBonus")]
    public int? ExpAndRepBonus { get; init; }

    /// <summary>Wheel spins from the Farm Pass extra-spin perk and not used yet.</summary>
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

    /// <summary>Fruit the gatherer is flying to; -1 when none.</summary>
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

    /// <summary>Whether the Boy offers a free re-hire; hiring him clears it.</summary>
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

    /// <summary>Goods the gatherers brought back to the nest and the player has not collected yet.</summary>
    public int GatheredResource { get; init; }

    /// <summary>Step the gatherer is in, such as 2 taking off or 6 flying back (saved as <c>AIState</c>).</summary>
    [JsonPropertyName("AIState")]
    public int GathererAiState { get; init; }

    /// <summary>Position of the nest's habitat among the habitats of its kind.</summary>
    public int GathererHabitatIndex { get; init; }

    /// <summary>Mine the gatherer works at; -1 when none.</summary>
    public int GathererMineIndex { get; init; }

    /// <summary>Nest the gatherer belongs to.</summary>
    public int GathererNestIndex { get; init; }

    /// <summary>How many of the gift's reward the gift gives.</summary>
    public int GiftAmount { get; init; }

    /// <summary>
    /// Gets the number of visitor gifts already processed by this spawner during its current daily
    /// cycle.
    /// </summary>
    [JsonPropertyName("GiftCnt")]
    public int GiftCount { get; init; }

    /// <summary>Global id of the gift's reward.</summary>
    public int GiftGid { get; init; }

    /// <summary>Gets the retained time-limited gift-mailbox slot.</summary>
    [JsonPropertyName("slot4")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public GiftMailboxSlotSnapshot? GiftMailboxSlot4 { get; init; }

    /// <summary>Gets the native acquisition reason assigned to this gift.</summary>
    [JsonPropertyName("CustomTag")]
    public int GiftRewardReason { get; init; }

    /// <summary>How many goods the visitor asks for.</summary>
    public int GoodAmount { get; init; }

    /// <summary>Gets the retained GoodAmountToReward value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? GoodAmountToReward { get; init; }

    /// <summary>Gets the retained GoodAmounts value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int[]? GoodAmounts { get; init; }

    /// <summary>Global id of the goods the visitor asks for.</summary>
    [JsonPropertyName("GoodGlobalId")]
    public int GoodGlobalId { get; init; }

    /// <summary>Gets the reward-target sentinel used by mystery-box placement reconciliation.</summary>
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

    /// <summary>Whether the Boy's hire ran out; hiring him again clears it.</summary>
    public bool HireEnded { get; init; }

    /// <summary>Time left on the Boy's hire.</summary>
    public TimerSnapshot? HireTimer { get; init; }

    /// <summary>Horizontal tile position (saved as <c>X</c>); <see cref="AccurateX"/> wins when it is saved.</summary>
    [JsonPropertyName("X")]
    public int? HorizontalCoordinate { get; init; }

    /// <summary>Whether the Boy's interval offer is running.</summary>
    public bool IntervalOfferActive { get; init; }

    /// <summary>Countdown until the Boy's next interval offer.</summary>
    public TimerSnapshot? IntervalOfferTimer { get; init; }

    /// <summary>Whether the wheel's current prize comes from an event.</summary>
    public bool IsPrizeFromEvent { get; init; }

    /// <summary>Global id of the item the object holds (saved as <c>ItemID</c>), such as the goods in the Boy's box.</summary>
    [JsonPropertyName("ItemID")]
    public int ItemGlobalId { get; init; }

    /// <summary>Gets the retained collection-area item list.</summary>
    [JsonPropertyName("ItemList")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ItemQuantityListSnapshot? ItemList { get; init; }

    /// <summary>Jackpot tier of the wheel: each jackpot raises it, and a higher tier multiplies the next jackpot's coins.</summary>
    public int JackpotCount { get; init; }

    /// <summary>Gets the last roadside advertisement timestamp.</summary>
    public long LastAdvertisementTimestamp { get; init; }

    /// <summary>Unix hour of the wheel's last daily reset.</summary>
    public int LastDailyResetHourIndex { get; init; }

    /// <summary>Last live event the wheel used (saved as <c>LastEventID</c>); 0 when none.</summary>
    [JsonPropertyName("LastEventID")]
    public int LastEventId { get; init; }

    /// <summary>Unix day the wheel was last set up (saved as <c>LastInitDayIndex</c>).</summary>
    [JsonPropertyName("LastInitDayIndex")]
    public int LastInitializationDayIndex { get; init; }

    /// <summary>Unix hour the wheel was last set up (saved as <c>LastInitHourIndex</c>).</summary>
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

    /// <summary>Unix day of the wheel's last spin.</summary>
    public int LastSpinDayIndex { get; init; }

    /// <summary>Global id of another object this one is linked to (saved as <c>GlobalId</c>); 0 when none.</summary>
    [JsonPropertyName("GlobalId")]
    public int LinkedGlobalId { get; init; }

    /// <summary>Gets the retained Locked value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Locked { get; init; }
    /// <summary>Gets the retained LockedChecked value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? LockedChecked { get; init; }

    /// <summary>Gatherings counted on a gatherer habitat (saved as <c>Gather</c>).</summary>
    [JsonPropertyName("Gather")]
    public int MasteryGatherCount { get; init; }

    /// <summary>Gets the retained Miller value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HelperHouseStateSnapshot? Miller { get; init; }

    /// <summary>Gets the retained MineCount value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MineCount { get; init; }

    /// <summary>Whether the object is placed mirrored.</summary>
    public bool Mirrored { get; init; }

    /// <summary>Gets the passenger's displayed name index.</summary>
    public int? NameIndex { get; init; }

    /// <summary>Gets a fishing spot's retained net or trap and drops.</summary>
    public FishingNetSnapshot? Net { get; init; }

    /// <summary>
    /// Gets the retained timestamp that controls the visitor spawner's next daily refresh
    /// boundary.
    /// </summary>
    [JsonPropertyName("TimeStamp")]
    public int NextDailyRefreshTimestamp { get; init; }

    /// <summary>Next point of the photographer's route.</summary>
    public int NextPoint { get; init; }

    /// <summary>Free daily wheel spins not used yet (saved as <c>NumSpins</c>).</summary>
    [JsonPropertyName("NumSpins")]
    public int NumberSpins { get; init; }

    /// <summary>Time left on the Boy's current offer.</summary>
    public TimerSnapshot? OfferTimer { get; init; }

    /// <summary>Gets the retained OpenedBoxTimer value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? OpenedBoxTimer { get; init; }

    /// <summary>Upgrade rank of the object; 1 for an object that was never upgraded.</summary>
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

    /// <summary>How much the visitor pays for the goods.</summary>
    public int PaymentObjectAmount { get; init; }

    /// <summary>Global id of what the visitor pays with, such as coins.</summary>
    [JsonPropertyName("PaymentObjectGlobalId")]
    public int PaymentObjectGlobalId { get; init; }

    /// <summary>Quest the visitor brings, kept as received; null when the visitor has none.</summary>
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

    /// <summary>How many of the wheel's current prize the spin gives.</summary>
    public int PrizeCount { get; init; }

    /// <summary>Global id of the wheel's current prize (saved as <c>PrizeGlobalID</c>).</summary>
    [JsonPropertyName("PrizeGlobalID")]
    public int PrizeGlobalId { get; init; }

    /// <summary>Kind of the wheel's current prize: 1 an item, 2 a free spin, 3 the jackpot.</summary>
    public int PrizeType { get; init; }

    /// <summary>Gets the retained ProductionList value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProductionListSnapshot? ProductionList { get; init; }

    /// <summary>Gets accumulated production minutes used by legacy building mastery.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ProductionTime { get; init; }

    /// <summary>Seed of the object's own random numbers, for objects that draw their own.</summary>
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

    /// <summary>Slots of a visitor spawner, one per visitor it can have.</summary>
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

    /// <summary>Truck orders on the order board.</summary>
    public OrderSnapshot[] Orders { get; init; } = [];

    /// <summary>Gets the retained SpawnedBoxCount value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SpawnedBoxCount { get; init; }

    /// <summary>Whether the visitor was spawned by the tutorial instead of the visitor spawner.</summary>
    public bool SpawnedFromTutorial { get; init; }

    /// <summary>Whether the visitor came from the second version of the visitor spawner.</summary>
    public bool SpawnedFromV2 { get; init; }

    /// <summary>Gets the number of ordinary passengers spawned for the current town train.</summary>
    [JsonPropertyName("Spawn")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SpawnedTrainPassengerCount { get; init; }

    /// <summary>State number of the object; what each value means depends on the kind of object.</summary>
    public int State { get; init; }

    /// <summary>Gets the retained state deadline used by stateful home objects.</summary>
    public int StateTimer { get; init; }

    /// <summary>Gets the retained duration of the boat's current dock or travel state.</summary>
    public int StayTime { get; init; }

    /// <summary>Gets the retained SubState value.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SubState { get; init; }

    /// <summary>Gets the passenger's retained walking destination.</summary>
    public int? TargetAccurateX { get; init; }

    /// <summary>Gets the passenger's retained walking destination.</summary>
    public int? TargetAccurateY { get; init; }

    /// <summary>Global id of the building a construction site turns into.</summary>
    public int TargetData { get; init; }

    /// <summary>Gets the passenger request currently associated with a service building.</summary>
    public int? TargetPassengerServiceIndex { get; init; }

    /// <summary>Horizontal flight target of a gatherer, relative to the object it belongs to.</summary>
    public int TargetX { get; init; }

    /// <summary>Vertical flight target of a gatherer, relative to the object it belongs to.</summary>
    public int TargetY { get; init; }

    /// <summary>Gets the town service time-reduction upgrade level.</summary>
    public int? TimeBonus { get; init; }

    /// <summary>Main countdown of the object; what it times depends on the kind of object.</summary>
    public TimerValue Timer { get; init; }

    /// <summary>Gets the number of passengers assigned to a saved town train.</summary>
    [JsonPropertyName("Passengers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TrainPassengerCount { get; init; }

    /// <summary>Flight time of the gatherer's current trip, in updates.</summary>
    public int TravelTime { get; init; }

    /// <summary>Tutorial visitors the visitor spawner has spawned.</summary>
    public int TutorialPeopleSpawned { get; init; }

    /// <summary>Gets upgrade materials already paid for the current building rank.</summary>
    public int[] UpgradeCollectionToolReadyDatas { get; init; } = [];

    /// <summary>Whether the building's upgrade is ready; the client logic only carries it along.</summary>
    public bool UpgradeReady { get; init; }

    /// <summary>Countdown of a fishing-area building's upgrade; the building cannot be upgraded again before it ends.</summary>
    public TimerSnapshot? UpgradeTimer { get; init; }

    /// <summary>Gets whether the passenger spawner retains the town tutorial's wandering region.</summary>
    [JsonPropertyName("TutorialWalkRandom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? UseTownTutorialWandering { get; init; }

    /// <summary>Wheel spins started today; the daily spin limit counts them.</summary>
    public int UsedSpins { get; init; }

    /// <summary>Vertical tile position (saved as <c>Y</c>); <see cref="AccurateY"/> wins when it is saved.</summary>
    [JsonPropertyName("Y")]
    public int? VerticalCoordinate { get; init; }

    /// <summary>Global ids of the prizes on the wheel's slots, matching <see cref="WheelAmounts"/> by position.</summary>
    public int[][] WheelPrizes { get; init; } = [];

    /// <summary>Amounts of the prizes on the wheel's slots, matching <see cref="WheelPrizes"/> by position.</summary>
    public int[][] WheelAmounts { get; init; } = [];
}
