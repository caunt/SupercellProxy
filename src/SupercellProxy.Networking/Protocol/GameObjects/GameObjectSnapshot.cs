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
    /// <summary>Gets or sets the exact horizontal map position in map units (512 per tile); <see langword="null"/> when only the tile position is saved.</summary>
    /// <value>A map-unit coordinate, where a tile's center is its tile index times 512 plus 256; it takes precedence over <see cref="HorizontalCoordinate"/>.</value>
    public int? AccurateX { get; init; }

    /// <summary>Gets or sets the exact vertical map position in map units (512 per tile); <see langword="null"/> when only the tile position is saved.</summary>
    /// <value>A map-unit coordinate, where a tile's center is its tile index times 512 plus 256; it takes precedence over <see cref="VerticalCoordinate"/>.</value>
    public int? AccurateY { get; init; }

    /// <summary>Gets or sets the wheel spins earned by watching videos and not used yet.</summary>
    /// <value>A spin count.</value>
    public int AdsSpins { get; init; }
    /// <summary>Gets or sets the per-stand roadside advertisement timers, in native timer ticks.</summary>
    /// <value>One countdown per roadside stand slot, at 15 ticks per second, saved as <c>AdTimers</c>, of the time left on the stand's advertisement; <see langword="null"/> when not saved.</value>
    [JsonPropertyName("AdTimers")]
    public int[]? AdvertisementTimers { get; init; }
    /// <summary>Gets or sets the fishing helper's saved activity and orders.</summary>
    /// <value>Angus's hire state, offers and tackle orders, saved as <c>Helper</c> on his object; <see langword="null"/> loads as a helper that was never hired.</value>
    [JsonPropertyName("Helper")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HelperHouseStateSnapshot? AngusHelper { get; init; }

    /// <summary>Gets or sets the uncollected pearls stored in Angus's bucket.</summary>
    /// <value>A pearl count saved as <c>PearlCount</c>; <see langword="null"/> (read as 0) when the bucket is empty or the object is not Angus.</value>
    [JsonPropertyName("PearlCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AngusPearlCount { get; init; }

    /// <summary>Gets or sets the passenger's sanctuary-visit bonus row.</summary>
    /// <value>Global id of the bonus row a town passenger earns by visiting a sanctuary animal, saved as <c>AnimalBonusID</c>; 0 or <see langword="null"/> when the passenger has none.</value>
    [JsonPropertyName("AnimalBonusID")]
    public int? AnimalBonusId { get; init; }
    /// <summary>Gets or sets the position of a livestock animal's habitat among all animal habitats of the home.</summary>
    /// <value>A zero-based index counted over every animal habitat in home order; <see langword="null"/> is read as 0.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AnimalHabitatIndex { get; init; }

    /// <summary>Gets or sets the retained adult-pet animation index.</summary>
    /// <value>Which idle animation the pet plays; <see langword="null"/> is read as 0.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AnimationIndex { get; init; }

    /// <summary>Gets or sets the per-stand automatic-buyer timers for a roadside shop.</summary>
    /// <value>One countdown per stand slot, at 15 ticks per second, for the stand's automatic buyer; -1 when no automatic buyer is scheduled, and empty for objects other than the roadside shop.</value>
    [JsonPropertyName("AITimer")]
    public int[] AutomaticBuyerTimers { get; init; } = [];
    /// <summary>Gets or sets the saved Balloon height counter.</summary>
    /// <value>The floating balloon's current height, saved as <c>Height</c>; <see langword="null"/> is read as 0.</value>
    [JsonPropertyName("Height")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? BalloonHeight { get; init; }



    /// <summary>Gets or sets the Balloon reward row selected when it was popped.</summary>
    /// <value>A zero-based index into the balloon's reward rows, saved as <c>rewardIndex</c>; <see langword="null"/> is read as 0.</value>
    [JsonPropertyName("rewardIndex")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? BalloonRewardIndex { get; init; }

    /// <summary>Gets or sets the production seconds of the product a processing-building booster sped up; <see langword="null"/> or 0 when none did.</summary>
    /// <value>A number of seconds.</value>
    public int? BeatsTimer { get; init; }
    /// <summary>Gets or sets the retained boat-order groups.</summary>
    /// <value>The boat's crate orders in slot order, saved as <c>boat_orders</c>; empty for objects other than the boat.</value>
    [JsonPropertyName("boat_orders")]
    public BoatOrderSnapshot[] BoatOrders { get; init; } = [];

    /// <summary>Gets or sets whether the current boat has been seen.</summary>
    /// <value><see langword="true"/> if the player has looked at the boat now docked, saved as <c>boat_seen</c>; otherwise, <see langword="false"/>, or <see langword="null"/> (read as <see langword="false"/>) when not saved.</value>
    [JsonPropertyName("boat_seen")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? BoatSeen { get; init; }

    /// <summary>Gets or sets the boosters attached to the building; <see langword="null"/> when it has none.</summary>
    /// <value>The building's attached boosters with their timers.</value>
    public BoosterListSnapshot? BoosterList { get; init; }

    /// <summary>Gets or sets the wheel spins bought with diamonds and not used yet.</summary>
    /// <value>A spin count.</value>
    public int BoughtSpins { get; init; }

    /// <summary>Gets or sets the wheel spins bought since the wheel car last arrived.</summary>
    /// <value>A purchase count.</value>
    public int BoughtSpinsDaily { get; init; }

    /// <summary>Gets or sets the saved bowl values, represented as food counts or boolean states by object type.</summary>
    /// <value>One entry per bowl of a pet or sanctuary habitat, in bowl order, saved as <c>Bowls</c>; pet habitats store each as a food count, sanctuary habitats as filled or empty. <see langword="null"/> when the object has no bowls.</value>
    [JsonPropertyName("Bowls")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BowlValueSnapshot[]? BowlValues { get; init; }

    /// <summary>Gets or sets the Boy's pending interval-offer state.</summary>
    /// <value>A Boy state code remembered from before he was hired, such as 2 (standing available) or 14 (showing a re-hire offer); 0 when nothing is pending.</value>
    public int BoyOffer { get; init; }

    /// <summary>Gets or sets which parts of the object are broken, one flag per part; <see langword="null"/> for objects without parts.</summary>
    /// <value>One flag per part in the object's part order, where <see langword="true"/> marks a part that is still broken.</value>
    public bool[]? BrokenParts { get; init; }

    /// <summary>Gets or sets the caretaker's saved hire state, offers and feeding orders.</summary>
    /// <value>The caretaker part of the helper house, or <see langword="null"/> when the save does not carry it and the caretaker is absent.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HelperHouseStateSnapshot? Caretaker { get; init; }

    /// <summary>Gets or sets whether the gatherer is flying back to its nest with gathered goods.</summary>
    /// <value><see langword="true"/> if the gatherer finished a harvest and is on its way home; otherwise, <see langword="false"/>.</value>
    public bool CarryingResources { get; init; }

    /// <summary>Gets or sets the town service coin-bonus upgrade level.</summary>
    /// <value>A zero-based index into the service building's <c>CoinBonus</c> column, which gives the coin bonus percentage; <see langword="null"/> is read as level 0.</value>
    public int? CoinBonus { get; init; }

    /// <summary>Gets or sets the days in a row the wheel was spun.</summary>
    /// <value>A day count.</value>
    public int ConsecutiveSpinDays { get; init; }

    /// <summary>Gets or sets the countdown of a construction site until its building is finished.</summary>
    /// <value>The site's countdown at 15 ticks per second; <see langword="null"/> loads as an elapsed timer.</value>
    public TimerSnapshot? ConstructionTimer { get; init; }

    /// <summary>Gets or sets the cooldown countdown, such as the Boy's rest or a fishing spot's wait before it can be used again.</summary>
    /// <value>A countdown at 15 ticks per second; <see langword="null"/> loads as an elapsed timer.</value>
    public TimerSnapshot? CooldownTimer { get; init; }

    /// <summary>Gets or sets how many items the object holds, such as the goods in the Boy's box.</summary>
    /// <value>An amount of the item named by <see cref="ItemGlobalId"/>.</value>
    public int Count { get; init; }

    /// <summary>Gets or sets whether the creature has been collected.</summary>
    /// <value><see langword="true"/> if the seasonal creature was caught; otherwise, <see langword="false"/>, or <see langword="null"/> (read as <see langword="false"/>) when not saved.</value>
    [JsonPropertyName("Collected")]
    public bool? CreatureCollected { get; init; }

    /// <summary>Gets or sets the creature's spawning-rule id.</summary>
    /// <value>Global id of the spawning rule that created the creature; <see langword="null"/> is read as -1, no rule.</value>
    [JsonPropertyName("SpawningRuleID")]
    public int? CreatureSpawningRuleId { get; init; }

    /// <summary>Gets or sets the player-selected pet or sanctuary animal name.</summary>
    /// <value>The name the player typed, saved as <c>Name</c>; <see langword="null"/> when the animal was never named.</value>
    [JsonPropertyName("Name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CustomName { get; init; }

    /// <summary>Gets or sets the time of the object's daily reset as saved.</summary>
    /// <value>The saved integer, carried along unchanged; its unit is not identified.</value>
    public int DailyResetTime { get; init; }

    /// <summary>Gets or sets the visitors a spawner spawned today as saved.</summary>
    /// <value>The saved visitor count, carried along unchanged.</value>
    public int DailyVisitorsSpawned { get; init; }

    /// <summary>Gets or sets the global id of the object's table entry (saved as <c>ID</c>).</summary>
    /// <value>The data row that says what the object is; its table id is the global id divided by 100,000.</value>
    [JsonPropertyName("ID")]
    public int DataGlobalId { get; init; }

    /// <summary>Gets or sets the rewards the delivery truck is bringing back from a completed truck order.</summary>
    /// <value>The pending rewards in delivery order, saved as <c>rewards</c>; <see langword="null"/> when the truck brings none.</value>
    [JsonPropertyName("rewards")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TruckDeliveryRewardSnapshot[]? DeliveryRewards { get; init; }

    /// <summary>Gets or sets the sanctuary animal requested by the passenger.</summary>
    /// <value>Global id of the sanctuary animal row the town passenger wants to see, saved as <c>DesiredAnimalID</c>; 0 or <see langword="null"/> when none.</value>
    [JsonPropertyName("DesiredAnimalID")]
    public int? DesiredAnimalId { get; init; }

    /// <summary>Gets or sets whether a tree or bush has started being cleared.</summary>
    /// <value><see langword="true"/> if the plant clearing was started; otherwise, <see langword="false"/>.</value>
    public bool DestructionStarted { get; init; }

    /// <summary>Gets or sets the remaining timer of a wild obstacle that is being cleared.</summary>
    /// <value>A countdown at 15 ticks per second; <see langword="null"/> loads as an elapsed timer.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? DestructionTimer { get; init; }

    /// <summary>Gets or sets the diamond price of a mystery box's reward.</summary>
    /// <value>Diamonds the box's reward is worth while it is closed; <see langword="null"/> is read as 0.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? DiamondCost { get; init; }

    /// <summary>Gets or sets the diamonds the player must pay to take an opened mystery box's reward.</summary>
    /// <value>A diamond amount; 0 when nothing has to be paid, and <see langword="null"/> is read as 0.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? DiamondCostToTake { get; init; }

    /// <summary>Gets or sets the live event the object is tied to (saved as <c>EventID</c>); 0 when none.</summary>
    /// <value>A live event id from the event manager's list; ordinary objects keep 0.</value>
    [JsonPropertyName("EventID")]
    public int EventId { get; init; }

    /// <summary>Gets or sets the town service experience and reputation upgrade level.</summary>
    /// <value>A zero-based index into the service building's <c>ExpAndRepBonus</c> column, saved as <c>ExpBonus</c>; <see langword="null"/> is read as level 0.</value>
    [JsonPropertyName("ExpBonus")]
    public int? ExpAndRepBonus { get; init; }

    /// <summary>Gets or sets the wheel spins from the Farm Pass extra-spin perk and not used yet.</summary>
    /// <value>A spin count.</value>
    public int FarmPassSpins { get; init; }

    /// <summary>Gets or sets whether a livestock animal has been fed and is making its product.</summary>
    /// <value><see langword="true"/> if the animal was fed and its product is not collected yet; otherwise, <see langword="false"/>, or <see langword="null"/> (read as <see langword="false"/>) when not saved.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Fed { get; init; }

    /// <summary>Gets or sets the last finished town-passenger service index when saved.</summary>
    /// <value>The service building slot whose finished service waits for the passenger to collect it; -1 or <see langword="null"/> when nothing is waiting.</value>
    public int? FinishedServiceIndex { get; init; }

    /// <summary>Gets or sets the duck's retained random value.</summary>
    /// <value>A number below 10,000, saved as <c>ARand</c>; <see langword="null"/> is read as 0.</value>
    [JsonPropertyName("ARand")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? FishingAnimalRandomValue { get; init; }

    /// <summary>Gets or sets the fishing-spot instance index; an omitted value denotes index zero.</summary>
    /// <value>The instance part of the fishing spot's global id (global id modulo 100,000) that a fish belongs to.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? FishingSpotIndex { get; init; }

    /// <summary>Gets or sets the fruit the gatherer is flying to; -1 when none.</summary>
    /// <value>A zero-based fruit slot on the source the gatherer is heading for.</value>
    public int FlashFruitIndex { get; init; }

    /// <summary>Gets or sets the quantities generated for the Boy's current offers.</summary>
    /// <value>One amount per offer, pairing with <see cref="FoundItemPrices"/> by position; empty when the Boy has no offer.</value>
    public int[] FoundItemAmounts { get; init; } = [];

    /// <summary>Gets or sets the good selected for the Boy's current search.</summary>
    /// <value>Global id of the good the Boy offers to find, saved as <c>FoundItemID</c>; 0 when he holds no offer.</value>
    [JsonPropertyName("FoundItemID")]
    public int FoundItemGlobalId { get; init; }

    /// <summary>Gets or sets the prices generated for the Boy's current offers.</summary>
    /// <value>One price per offer, pairing with <see cref="FoundItemAmounts"/> by position; empty when the Boy has no offer.</value>
    public int[] FoundItemPrices { get; init; } = [];

    /// <summary>Gets or sets the roadside free-advertisement cooldown.</summary>
    /// <value>The countdown, at 15 ticks per second and saved as <c>FreeAdTimer</c>, until the next free newspaper advertisement; <see langword="null"/> loads as elapsed.</value>
    [JsonPropertyName("FreeAdTimer")]
    public TimerSnapshot? FreeAdvertisementTimer { get; init; }

    /// <summary>Gets or sets whether the Boy offers a free re-hire.</summary>
    /// <value><see langword="true"/> if the Boy's next hire is free; otherwise, <see langword="false"/>.</value>
    public bool FreeReEngagementAvailable { get; init; }

    /// <summary>Gets or sets whether the last mystery box shown at a friend's farm was opened.</summary>
    /// <value><see langword="true"/> if it was opened; otherwise, <see langword="false"/>, or <see langword="null"/> (read as <see langword="false"/>).</value>
    [JsonPropertyName("LastOpenedFriend")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? FriendLastOpened { get; init; }

    /// <summary>Gets or sets the cooldown after a mystery box was opened at a friend's farm.</summary>
    /// <value>A countdown at 15 ticks per second over one day (86,400 seconds), saved as <c>OpenedBoxTimerFriend</c>; <see langword="null"/> loads as elapsed.</value>
    [JsonPropertyName("OpenedBoxTimerFriend")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? FriendOpenedBoxTimer { get; init; }

    /// <summary>Gets or sets the time the mystery-box spawner has waited toward the next box at a friend's farm.</summary>
    /// <value>Elapsed game updates since the last friend box, saved as <c>TimerFriend</c>; <see langword="null"/> is read as 0.</value>
    [JsonPropertyName("TimerFriend")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? FriendSpawnTimer { get; init; }

    /// <summary>Gets or sets which fruit slots of a fruit tree, bush or gatherer source are already taken.</summary>
    /// <value>One flag per fruit slot, where <see langword="true"/> marks a fruit already picked or harvested.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool[]? Fruits { get; init; }

    /// <summary>Gets or sets the goods the gatherers brought back to the nest and the player has not collected yet.</summary>
    /// <value>A resource count.</value>
    public int GatheredResource { get; init; }

    /// <summary>Gets or sets the step the gatherer is in, such as 2 taking off or 6 flying back (saved as <c>AIState</c>).</summary>
    /// <value>The gatherer's behavior code; 1 means it rests at its nest, and other values mark a trip in progress.</value>
    [JsonPropertyName("AIState")]
    public int GathererAiState { get; init; }

    /// <summary>Gets or sets the position of the nest's habitat among the habitats of its kind.</summary>
    /// <value>The instance part (global id modulo 100,000) of the habitat object the nest sits on.</value>
    public int GathererHabitatIndex { get; init; }

    /// <summary>Gets or sets the mine the gatherer works at; -1 when none.</summary>
    /// <value>The instance part (global id modulo 100,000) of the gatherer source the gatherer is assigned to.</value>
    public int GathererMineIndex { get; init; }

    /// <summary>Gets or sets the nest the gatherer belongs to.</summary>
    /// <value>The instance part (global id modulo 100,000) of the gatherer's nest object.</value>
    public int GathererNestIndex { get; init; }

    /// <summary>Gets or sets how many of the gift's reward the gift gives.</summary>
    /// <value>The amount for <see cref="GiftGid"/> in the older single-reward gift form.</value>
    public int GiftAmount { get; init; }

    /// <summary>
    /// Gets or sets the number of visitor gifts already processed by this spawner during its current daily
    /// cycle.
    /// </summary>
    /// <value>A gift count saved as <c>GiftCnt</c> on the visitor spawner.</value>
    [JsonPropertyName("GiftCnt")]
    public int GiftCount { get; init; }

    /// <summary>Gets or sets the global id of the gift's reward.</summary>
    /// <value>The single reward row of an older-form gift or of a visitor carrying a gift; 0 when there is none.</value>
    public int GiftGid { get; init; }

    /// <summary>Gets or sets the retained time-limited gift-mailbox slot.</summary>
    /// <value>The gift mailbox's time-limited offer slot, saved as <c>slot4</c>; <see langword="null"/> for objects other than the gift mailbox.</value>
    [JsonPropertyName("slot4")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public GiftMailboxSlotSnapshot? GiftMailboxSlot4 { get; init; }

    /// <summary>Gets or sets the native acquisition reason assigned to this gift.</summary>
    /// <value>The reward reason code, saved as <c>CustomTag</c>.</value>
    [JsonPropertyName("CustomTag")]
    public int GiftRewardReason { get; init; }

    /// <summary>Gets or sets how many goods the visitor asks for.</summary>
    /// <value>The requested amount of <see cref="GoodGlobalId"/>; a closed mystery box uses it for the amount of its reward.</value>
    public int GoodAmount { get; init; }

    /// <summary>Gets or sets the amount of the reward an opened mystery box holds.</summary>
    /// <value>The amount of <see cref="GoodGlobalIdToReward"/>; <see langword="null"/> is read as 0.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? GoodAmountToReward { get; init; }

    /// <summary>Gets or sets the amounts of the rewards a gift holds.</summary>
    /// <value>One amount per entry of <see cref="GoodGlobalIds"/>, in the same order; the two lists must have the same length. <see langword="null"/> when the gift uses the single-reward form.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int[]? GoodAmounts { get; init; }

    /// <summary>Gets or sets the global id of the goods the visitor asks for.</summary>
    /// <value>A good row the visitor wants, or -1 once a served or sent-away visitor's order is closed; a closed mystery box keeps its reward row here.</value>
    [JsonPropertyName("GoodGlobalId")]
    public int GoodGlobalId { get; init; }

    /// <summary>Gets or sets the reward an opened mystery box is holding for the player.</summary>
    /// <value>Global id of the prepared reward; -1 after the reward is taken or the box is discarded, and 0 for a box that was never opened.</value>
    [JsonPropertyName("GoodGlobalIdToReward")]
    public int GoodGlobalIdToReward { get; init; }

    /// <summary>Gets or sets the reward rows a gift holds, such as a postman package.</summary>
    /// <value>Global ids of the rewards, paired by position with <see cref="GoodAmounts"/>; <see langword="null"/> when the gift uses the single-reward form.</value>
    [JsonPropertyName("GoodGlobalIds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int[]? GoodGlobalIds { get; init; }

    /// <summary>Gets or sets the growth countdown of a fruit tree, bush, gatherer source or mollusc.</summary>
    /// <value>A countdown at 15 ticks per second until the current growth phase is ready; <see langword="null"/> loads as elapsed.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? GrowTimer { get; init; }

    /// <summary>Gets whether the saved object explicitly supplied its random seed.</summary>
    /// <value><see langword="true"/> if the document set <see cref="RandomSeed"/>, even to 0; otherwise, <see langword="false"/>.</value>
    /// <remarks>The flag tells a saved seed of 0 apart from a missing one.</remarks>
    [JsonIgnore]
    public bool HasRandomSeed { get; private set; }

    /// <summary>Gets or sets the object's id in the avatar's help sequence.</summary>
    /// <value>A number from the home's help counter, saved as <c>HID</c>, by which help such as a plant revival names its target; <see langword="null"/> is read as 0.</value>
    [JsonPropertyName("HID")]
    public int? HelpId { get; init; }

    /// <summary>Gets or sets whether the Boy's hire ran out.</summary>
    /// <value><see langword="true"/> if the Boy's hire timer ran out; otherwise, <see langword="false"/>.</value>
    public bool HireEnded { get; init; }

    /// <summary>Gets or sets the time left on the Boy's hire.</summary>
    /// <value>A countdown at 15 ticks per second; <see langword="null"/> loads as an ended hire.</value>
    public TimerSnapshot? HireTimer { get; init; }

    /// <summary>Gets or sets the horizontal tile position (saved as <c>X</c>); <see cref="AccurateX"/> wins when it is saved.</summary>
    /// <value>A tile column index on the home's tile map; <see langword="null"/> when only the exact position is saved.</value>
    [JsonPropertyName("X")]
    public int? HorizontalCoordinate { get; init; }

    /// <summary>Gets or sets whether the Boy's interval offer is running.</summary>
    /// <value><see langword="true"/> if the Boy's current offer came from his interval timer rather than from the end of a hire; otherwise, <see langword="false"/>.</value>
    public bool IntervalOfferActive { get; init; }

    /// <summary>Gets or sets the countdown until the Boy's next interval offer.</summary>
    /// <value>A countdown at 15 ticks per second; <see langword="null"/> loads as elapsed.</value>
    public TimerSnapshot? IntervalOfferTimer { get; init; }

    /// <summary>Gets or sets whether the wheel's current prize comes from an event.</summary>
    /// <value><see langword="true"/> if the wheel's prizes come from a wheel event's configuration; otherwise, <see langword="false"/>.</value>
    public bool IsPrizeFromEvent { get; init; }

    /// <summary>Gets or sets the global id of the item the object holds (saved as <c>ItemID</c>), such as the goods in the Boy's box.</summary>
    /// <value>A good row whose amount is <see cref="Count"/>; 0 when the object holds nothing.</value>
    [JsonPropertyName("ItemID")]
    public int ItemGlobalId { get; init; }

    /// <summary>Gets or sets the retained collection-area item list.</summary>
    /// <value>The goods waiting in a helper or sanctuary collection area with their amounts; <see langword="null"/> is read as an empty list.</value>
    [JsonPropertyName("ItemList")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ItemQuantityListSnapshot? ItemList { get; init; }

    /// <summary>Gets or sets the jackpot tier of the wheel.</summary>
    /// <value>A tier from 0 up to the wheel's <c>TierMax</c>.</value>
    public int JackpotCount { get; init; }

    /// <summary>Gets or sets the last roadside advertisement timestamp.</summary>
    /// <value>Server time in Unix seconds when the player last placed a newspaper advertisement; 0 when never.</value>
    public long LastAdvertisementTimestamp { get; init; }

    /// <summary>Gets or sets the reset hour the wheel used when it last began a day.</summary>
    /// <value>The wheel's <c>DailyResetHour</c> table value, an hour of the UTC day from 0 to 23; despite its name it is not a Unix hour.</value>
    public int LastDailyResetHourIndex { get; init; }

    /// <summary>Gets or sets the last live event the wheel used (saved as <c>LastEventID</c>); 0 when none.</summary>
    /// <value>The id of the last wheel event whose prizes the wheel took; -1 for a wheel that never took event prizes.</value>
    [JsonPropertyName("LastEventID")]
    public int LastEventId { get; init; }

    /// <summary>Gets or sets the Unix day the wheel was last set up (saved as <c>LastInitDayIndex</c>).</summary>
    /// <value>Server seconds divided by 86,400 at the last daily set-up.</value>
    [JsonPropertyName("LastInitDayIndex")]
    public int LastInitializationDayIndex { get; init; }

    /// <summary>Gets or sets the Unix hour the wheel was last set up (saved as <c>LastInitHourIndex</c>).</summary>
    /// <value>Server seconds divided by 3,600 at the last daily set-up.</value>
    [JsonPropertyName("LastInitHourIndex")]
    public int LastInitializationHourIndex { get; init; }

    /// <summary>Gets or sets whether the player opened the last mystery box on their own farm.</summary>
    /// <value><see langword="true"/> if the last own box was opened; otherwise, <see langword="false"/> after a discard, or <see langword="null"/> (read as <see langword="false"/>).</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? LastOpened { get; init; }

    /// <summary>Gets or sets the storage rank the materials signpost last showed.</summary>
    /// <value>A silo or barn rank, saved as <c>LastSignpostMats</c>; <see langword="null"/> is read as 0.</value>
    [JsonPropertyName("LastSignpostMats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? LastSignpostMaterials { get; init; }

    /// <summary>Gets or sets the storage rank the upgrade signpost last showed.</summary>
    /// <value>A silo or barn rank, saved as <c>LastSignpostUpg</c>; <see langword="null"/> is read as 0.</value>
    [JsonPropertyName("LastSignpostUpg")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? LastSignpostUpgrade { get; init; }

    /// <summary>Gets or sets the Unix day of the wheel's last spin.</summary>
    /// <value>Server seconds divided by 86,400 when a spin last started.</value>
    public int LastSpinDayIndex { get; init; }

    /// <summary>Gets or sets the global id of another object this one is linked to (saved as <c>GlobalId</c>); 0 when none.</summary>
    /// <value>An object instance global id; which objects set it is not fully identified, and a photographer is expected to save 0.</value>
    [JsonPropertyName("GlobalId")]
    public int LinkedGlobalId { get; init; }

    /// <summary>Gets or sets whether an opened mystery box's reward turned out locked behind diamonds.</summary>
    /// <value><see langword="true"/> if taking the reward costs <see cref="DiamondCost"/> diamonds; otherwise, <see langword="false"/>, or <see langword="null"/> (read as <see langword="false"/>).</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Locked { get; init; }
    /// <summary>Gets or sets whether the mystery box's lock state has been set.</summary>
    /// <value><see langword="true"/> if <see cref="Locked"/> already holds the box's lock state; otherwise, <see langword="false"/>, or <see langword="null"/> (read as <see langword="false"/>).</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? LockedChecked { get; init; }

    /// <summary>Gets or sets the gatherings counted on a gatherer habitat (saved as <c>Gather</c>).</summary>
    /// <value>A running count of gatherings toward the habitat's mastery, carried in the save.</value>
    [JsonPropertyName("Gather")]
    public int MasteryGatherCount { get; init; }

    /// <summary>Gets or sets the miller's saved hire state, offers and orders.</summary>
    /// <value>The miller part of the helper house, or <see langword="null"/> when the save does not carry it and the miller is absent.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HelperHouseStateSnapshot? Miller { get; init; }

    /// <summary>Gets or sets the resources a gatherer source has left to give.</summary>
    /// <value>A resource count; <see langword="null"/> is read as 0, an exhausted source.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MineCount { get; init; }

    /// <summary>Gets or sets whether the object is placed mirrored.</summary>
    /// <value><see langword="true"/> if the object is flipped, which swaps its width and height on the tile map; otherwise, <see langword="false"/>.</value>
    public bool Mirrored { get; init; }

    /// <summary>Gets or sets the passenger's displayed name index.</summary>
    /// <value>A zero-based index into the male or female name list, matching the passenger's <c>IsMale</c> column; <see langword="null"/> is read as -1.</value>
    public int? NameIndex { get; init; }

    /// <summary>Gets or sets a fishing spot's retained net or trap and drops.</summary>
    /// <value>The placed net or trap with its drops and random state; <see langword="null"/> when the spot carries none.</value>
    public FishingNetSnapshot? Net { get; init; }

    /// <summary>
    /// Gets or sets the retained timestamp of the visitor spawner's next daily refresh
    /// boundary.
    /// </summary>
    /// <value>A timestamp saved as <c>TimeStamp</c> on the visitor spawner and carried along.</value>
    [JsonPropertyName("TimeStamp")]
    public int NextDailyRefreshTimestamp { get; init; }

    /// <summary>Gets or sets the next point of the photographer's route.</summary>
    /// <value>A zero-based index into the route the character is walking; farm visitors store their walk progress here too.</value>
    public int NextPoint { get; init; }

    /// <summary>Gets or sets the free daily wheel spins not used yet (saved as <c>NumSpins</c>).</summary>
    /// <value>A spin count.</value>
    [JsonPropertyName("NumSpins")]
    public int NumberSpins { get; init; }

    /// <summary>Gets or sets the time left on the Boy's current offer.</summary>
    /// <value>A countdown at 15 ticks per second; <see langword="null"/> loads as elapsed.</value>
    public TimerSnapshot? OfferTimer { get; init; }

    /// <summary>Gets or sets the cooldown after the player opened a free mystery box on their own farm.</summary>
    /// <value>A countdown at 15 ticks per second over one day (86,400 seconds); <see langword="null"/> loads as elapsed.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? OpenedBoxTimer { get; init; }

    /// <summary>Gets or sets the upgrade rank of the object; 1 for an object that was never upgraded.</summary>
    /// <value>A rank starting at 1 (the default); for storage it selects the capacity level.</value>
    public int Rank { get; init; } = 1;

    /// <summary>Gets or sets the passenger's completion-bonus quantity.</summary>
    /// <value>How many of <see cref="PassengerBonusResourceId"/> the passenger gives when all its services are done, saved as <c>BonusAmount</c>.</value>
    [JsonPropertyName("BonusAmount")]
    public int? PassengerBonusAmount { get; init; }

    /// <summary>Gets or sets whether the passenger's completion bonus belongs to an event.</summary>
    /// <value><see langword="true"/> if the bonus comes from an event's rewards, saved as <c>BonusEvent</c>; otherwise, <see langword="false"/>, or <see langword="null"/> (read as <see langword="false"/>).</value>
    [JsonPropertyName("BonusEvent")]
    public bool? PassengerBonusEvent { get; init; }

    /// <summary>Gets or sets the legacy passenger completion-bonus row.</summary>
    /// <value>Global id of an older-format bonus row, saved as <c>BonusID</c>; current saves leave it out.</value>
    [JsonPropertyName("BonusID")]
    public int? PassengerBonusId { get; init; }

    /// <summary>Gets or sets the passenger's completion-bonus resource.</summary>
    /// <value>Global id of the reward row, saved as <c>BonusID2</c>; 0 or <see langword="null"/> when the passenger brings no completion bonus.</value>
    [JsonPropertyName("BonusID2")]
    public int? PassengerBonusResourceId { get; init; }

    /// <summary>Gets or sets the high part of the passenger's pickup-origin home id.</summary>
    /// <value>The high 32 bits of the home that sent its personal train for the passenger, saved as <c>PH</c>; <see langword="null"/> when no pickup is pending.</value>
    [JsonPropertyName("PH")]
    public int? PassengerOriginHigh { get; init; }

    /// <summary>Gets or sets the low part of the passenger's pickup-origin home id.</summary>
    /// <value>The low 32 bits of the home that sent its personal train for the passenger, saved as <c>PL</c>; <see langword="null"/> when no pickup is pending.</value>
    [JsonPropertyName("PL")]
    public int? PassengerOriginLow { get; init; }

    /// <summary>Gets or sets the number of personal-train pickups for this passenger.</summary>
    /// <value>A count saved as <c>PC</c>; <see langword="null"/> is read as 0.</value>
    [JsonPropertyName("PC")]
    public int? PassengerPickupCount { get; init; }

    /// <summary>Gets or sets the passenger spawner's private service-request RNG state.</summary>
    /// <value>A random state saved as <c>ServiceRandomizer</c>; <see langword="null"/> when not saved.</value>
    [JsonPropertyName("ServiceRandomizer")]
    public int? PassengerServiceRandomState { get; init; }

    /// <summary>Gets or sets the saved town-passenger service requests.</summary>
    /// <value>The services the passenger asks for, each with its goods and progress; <see langword="null"/> when the passenger has no requests yet.</value>
    public TownPassengerServiceSnapshot[]? PassengerServices { get; init; }

    /// <summary>Gets or sets how much the visitor pays for the goods.</summary>
    /// <value>An amount of <see cref="PaymentObjectGlobalId"/>, paid when the visitor's order is completed.</value>
    public int PaymentObjectAmount { get; init; }

    /// <summary>Gets or sets the global id of what the visitor pays with, such as coins.</summary>
    /// <value>A money or good row; -1 once a served or sent-away visitor's order is closed.</value>
    [JsonPropertyName("PaymentObjectGlobalId")]
    public int PaymentObjectGlobalId { get; init; }

    /// <summary>Gets or sets the quest the visitor brings, kept as received; <see langword="null"/> when the visitor has none.</summary>
    /// <value>The quest document as raw JSON, not decoded.</value>
    public EncodedDocumentValue? PeopleQuestV2 { get; init; }

    /// <summary>Gets or sets the retained index of an adult pet's habitat among the home's pet habitats.</summary>
    /// <value>A zero-based habitat index, saved as <c>Habitat</c>, that sanctuary animals use the same way; <see langword="null"/> is read as 0.</value>
    [JsonPropertyName("Habitat")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? PetHabitatIndex { get; init; }

    /// <summary>Gets or sets the feeding countdown of a pet, baby pet or sanctuary animal.</summary>
    /// <value>A countdown at 15 ticks per second, saved as <c>T</c>, of how long the last feeding lasts; <see langword="null"/> loads as elapsed, meaning the animal is hungry.</value>
    [JsonPropertyName("T")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerSnapshot? PetTimer { get; init; }

    /// <summary>Gets or sets the growth phase of a fruit tree or bush.</summary>
    /// <value>A zero-based index into the tree's <c>FruitCount</c> phases; <see langword="null"/> is read as 0.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Phase { get; init; }

    /// <summary>Gets or sets how many of the wheel's current prize the spin gives.</summary>
    /// <value>The amount on the slot the last spin landed on.</value>
    public int PrizeCount { get; init; }

    /// <summary>Gets or sets the global id of the wheel's current prize (saved as <c>PrizeGlobalID</c>).</summary>
    /// <value>The prize row on the slot the last spin landed on, such as a good or booster; 0 when no prize is waiting.</value>
    [JsonPropertyName("PrizeGlobalID")]
    public int PrizeGlobalId { get; init; }

    /// <summary>Gets or sets the kind of the wheel's current prize: 1 an item, 2 a free spin, 3 the jackpot.</summary>
    /// <value>A prize kind code; 0 while no prize is waiting.</value>
    public int PrizeType { get; init; }

    /// <summary>Gets or sets the production queue of a production building.</summary>
    /// <value>The finished products waiting to be collected and the productions still running; <see langword="null"/> when the save carries no queue for the object.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProductionListSnapshot? ProductionList { get; init; }

    /// <summary>Gets or sets the accumulated production minutes used by legacy building mastery.</summary>
    /// <value>Whole minutes of production started in the building; <see langword="null"/> is read as 0.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ProductionTime { get; init; }

    /// <summary>Gets or sets the seed of the object's own random numbers, for objects that draw their own.</summary>
    /// <value>
    /// The current state of the object's private random stream, saved for objects such as the wheel, the gift mailbox or a fishing
    /// spot. Setting it also sets <see cref="HasRandomSeed"/>.
    /// </value>
    public int RandomSeed
    {
        get;
        init
        {
            field = value;
            HasRandomSeed = true;
        }
    }
    /// <summary>Gets or sets the fishing spot's saved random-seed migration version.</summary>
    /// <value>A version number saved with the spot's <see cref="RandomSeed"/>.</value>
    public int RandomSeedFix { get; init; }

    /// <summary>Gets or sets the slots of a visitor spawner, one per visitor it can have.</summary>
    /// <value>Each slot's spawn time and visitor, in slot order; empty for objects other than the visitor spawner.</value>
    public PeopleSpawnerSlotSnapshot[] SlotStates { get; init; } = [];

    /// <summary>Gets or sets the seed of the truck order board's order generation.</summary>
    /// <value>The state of the board's random stream; <see langword="null"/> when not saved.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Seed { get; init; }

    /// <summary>Gets or sets the selected boat order; old saves omit this field and select zero.</summary>
    /// <value>A zero-based index into <see cref="BoatOrders"/> of the crate order the boat shows as selected.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SelectedBoatOrderIndex { get; init; }

    /// <summary>Gets or sets the selected index in the Boy's current offer.</summary>
    /// <value>A zero-based index into <see cref="FoundItemAmounts"/> of the offer the player accepted, saved as <c>SelectedOfferIndex</c>; -1 or <see langword="null"/> while none is chosen.</value>
    [JsonPropertyName("SelectedOfferIndex")]
    public int? SelectedBoyOfferIndex { get; init; }

    /// <summary>Gets or sets the current town-passenger service index when saved.</summary>
    /// <value>The service building slot that is serving the passenger right now; -1 when no service is running. <see langword="null"/> is read as 0.</value>
    public int? ServiceIndex { get; init; }

    /// <summary>Gets or sets the saved town service entries.</summary>
    /// <value>The services a town service building is running and the finished ones waiting for their passengers; <see langword="null"/> is read as none.</value>
    public TownServiceListSnapshot? ServiceList { get; init; }

    /// <summary>Gets or sets the town service-slot upgrade level.</summary>
    /// <value>A zero-based index into the service building's <c>ServiceSlots</c> column, which gives how many services it runs at once; <see langword="null"/> is read as level 0.</value>
    public int? ServiceSlots { get; init; }

    /// <summary>Gets or sets a lobster or duck's slot in its processing facility.</summary>
    /// <value>The zero-based place the animal takes in its pool or salon.</value>
    public int Slot { get; init; }

    /// <summary>Gets or sets the truck orders on the order board.</summary>
    /// <value>The board's orders in slot order; empty for objects other than the truck order board.</value>
    public OrderSnapshot[] Orders { get; init; } = [];

    /// <summary>Gets or sets how many mystery boxes the spawner has placed on the player's own farm.</summary>
    /// <value>A box count; <see langword="null"/> is read as 0.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SpawnedBoxCount { get; init; }

    /// <summary>Gets or sets whether the visitor was spawned by the tutorial instead of the visitor spawner.</summary>
    /// <value><see langword="true"/> if the tutorial spawned the visitor; otherwise, <see langword="false"/>.</value>
    public bool SpawnedFromTutorial { get; init; }

    /// <summary>Gets or sets whether the visitor came from the second version of the visitor spawner.</summary>
    /// <value><see langword="true"/> if the visitor spawner's newer slot logic created the visitor; otherwise, <see langword="false"/>.</value>
    public bool SpawnedFromV2 { get; init; }

    /// <summary>Gets or sets the number of ordinary passengers spawned for the current town train.</summary>
    /// <value>A count saved as <c>Spawn</c> on the passenger spawner; <see langword="null"/> is read as 0.</value>
    [JsonPropertyName("Spawn")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SpawnedTrainPassengerCount { get; init; }

    /// <summary>Gets or sets the state number of the object; what each value means depends on the kind of object.</summary>
    /// <value>An object-specific state code, such as the Boy's behavior, a visitor's order step or the wheel car's driving step; 0 when the save leaves it out.</value>
    public int State { get; init; }

    /// <summary>Gets or sets how many sanctuary viewing stops a town passenger has made.</summary>
    /// <value>A count of the passenger's finished sanctuary viewings; <see langword="null"/> is read as 0.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? StateLoops { get; init; }

    /// <summary>Gets or sets the retained state timer used by stateful home objects.</summary>
    /// <value>For the boat and the town train, the game updates (30 per second) already spent in the current state; 0 for objects that do not use it.</value>
    public int StateTimer { get; init; }

    /// <summary>Gets or sets the retained duration of the boat's current dock or travel state.</summary>
    /// <value>The length of the state in game updates (30 per second); the boat's remaining time is this minus <see cref="StateTimer"/>.</value>
    public int StayTime { get; init; }

    /// <summary>Gets or sets the secondary state of a pet or baby pet.</summary>
    /// <value>An adult pet's movement purpose or a baby pet's step, such as 1 moving to food or 2 moving to its sleep area; <see langword="null"/> is read as 0, none.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SubState { get; init; }

    /// <summary>Gets or sets the horizontal part of the passenger's retained walking destination.</summary>
    /// <value>A map-unit coordinate (512 per tile) the passenger walks toward; <see langword="null"/> when it has no saved destination.</value>
    public int? TargetAccurateX { get; init; }

    /// <summary>Gets or sets the vertical part of the passenger's retained walking destination.</summary>
    /// <value>A map-unit coordinate (512 per tile) the passenger walks toward; <see langword="null"/> when it has no saved destination.</value>
    public int? TargetAccurateY { get; init; }

    /// <summary>Gets or sets the global id of the building a construction site turns into.</summary>
    /// <value>A building row, such as a processing building being built or a neighborhood building being repaired; 0 for objects that are not construction sites.</value>
    public int TargetData { get; init; }

    /// <summary>Gets or sets the passenger request currently associated with a service building.</summary>
    /// <value>A zero-based index into the passenger's <see cref="PassengerServices"/> of the request being booked or served; -1 when none.</value>
    public int? TargetPassengerServiceIndex { get; init; }

    /// <summary>Gets or sets the horizontal flight target of a gatherer, relative to the object it belongs to.</summary>
    /// <value>A map-unit offset from the gatherer's parent object.</value>
    public int TargetX { get; init; }

    /// <summary>Gets or sets the vertical flight target of a gatherer, relative to the object it belongs to.</summary>
    /// <value>A map-unit offset from the gatherer's parent object.</value>
    public int TargetY { get; init; }

    /// <summary>Gets or sets the town service time-reduction upgrade level.</summary>
    /// <value>A zero-based index into the service building's <c>TimeBonus</c> column, which gives the service time reduction percentage; <see langword="null"/> is read as level 0.</value>
    public int? TimeBonus { get; init; }

    /// <summary>Gets or sets the main countdown of the object; what it times depends on the kind of object.</summary>
    /// <value>Either a countdown, such as an animal's production or a duck's processing, or a plain counter, such as a spawner's elapsed updates; the object kind decides which form it reads.</value>
    public TimerValue Timer { get; init; }

    /// <summary>Gets or sets the number of passengers assigned to a saved town train.</summary>
    /// <value>The train's passenger quota, saved as <c>Passengers</c>.</value>
    [JsonPropertyName("Passengers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TrainPassengerCount { get; init; }

    /// <summary>Gets or sets the flight time of the gatherer's current trip, in updates.</summary>
    /// <value>The game updates the trip takes.</value>
    public int TravelTime { get; init; }

    /// <summary>Gets or sets the tutorial visitors the visitor spawner has spawned.</summary>
    /// <value>A count kept on the visitor spawner; 0 when the save leaves it out.</value>
    public int TutorialPeopleSpawned { get; init; }

    /// <summary>Gets or sets the upgrade materials already paid for the current building rank.</summary>
    /// <value>Global ids of the collection tools already handed in toward the next rank of a town building, fishing spot or mussel bed; empty means nothing was paid.</value>
    public int[] UpgradeCollectionToolReadyDatas { get; init; } = [];

    /// <summary>Gets or sets whether the building's upgrade is ready.</summary>
    /// <value><see langword="true"/> if the save marks the upgrade as ready; otherwise, <see langword="false"/>.</value>
    /// <remarks>The flag is written back unchanged.</remarks>
    public bool UpgradeReady { get; init; }

    /// <summary>Gets or sets the countdown of a fishing-area building's upgrade.</summary>
    /// <value>A countdown at 15 ticks per second; <see langword="null"/> loads as elapsed, so no upgrade is running.</value>
    public TimerSnapshot? UpgradeTimer { get; init; }

    /// <summary>Gets or sets whether the passenger spawner retains the town tutorial's wandering region.</summary>
    /// <value><see langword="true"/> if passengers still wander in the tutorial area; otherwise, <see langword="false"/>, or <see langword="null"/> (read as <see langword="false"/>).</value>
    [JsonPropertyName("TutorialWalkRandom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? UseTownTutorialWandering { get; init; }

    /// <summary>Gets or sets the wheel spins started today.</summary>
    /// <value>A spin count.</value>
    public int UsedSpins { get; init; }

    /// <summary>Gets or sets the vertical tile position (saved as <c>Y</c>); <see cref="AccurateY"/> wins when it is saved.</summary>
    /// <value>A tile row index on the home's tile map; <see langword="null"/> when only the exact position is saved.</value>
    [JsonPropertyName("Y")]
    public int? VerticalCoordinate { get; init; }

    /// <summary>Gets or sets the global ids of the prizes on the wheel's slots, matching <see cref="WheelAmounts"/> by position.</summary>
    /// <value>Prize rows, each with one entry per wheel slot (the wheel's <c>WheelNumSlots</c>); 0 or -1 marks a slot without an item.</value>
    public int[][] WheelPrizes { get; init; } = [];

    /// <summary>Gets or sets the amounts of the prizes on the wheel's slots, matching <see cref="WheelPrizes"/> by position.</summary>
    /// <value>Amount rows shaped exactly like <see cref="WheelPrizes"/>.</value>
    public int[][] WheelAmounts { get; init; } = [];
}
