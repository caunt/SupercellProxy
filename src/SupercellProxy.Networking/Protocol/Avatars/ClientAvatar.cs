using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Avatars.Collections;
using SupercellProxy.Networking.Protocol.Avatars.Derby;
using SupercellProxy.Networking.Protocol.Inventory;
using SupercellProxy.Networking.Protocol.Mail;
using SupercellProxy.Networking.Protocol.MapGame;
using SupercellProxy.Networking.Protocol.Neighborhoods;
using SupercellProxy.Networking.Protocol.RoadsideShops;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Town;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// A player's account-level record as home messages carry it: farm name and ids, inventory, roadside shop, mail, neighbourhood,
/// derby standing, live events and purchase history. Own-home loads carry the player's own avatar; visits also carry the owner's.
/// </summary>
public sealed record ClientAvatar : ICodecProvider<ClientAvatar>
{
    /// <inheritdoc />
    public static ICodec<ClientAvatar> Codec { get; } = Codecs.Composite(Codecs.VarInt,
        static avatar => avatar.AvatarTimestamp,
        Codecs.VarInt,
        static avatar => avatar.Unknown1,
        Codecs.VarInt,
        static avatar => avatar.AvatarVersion,
        Codecs.VarInt,
        static avatar => avatar.SecondsWithoutSpending,
        Codecs.OptionalString,
        static avatar => avatar.FarmName,
        Codecs.LongId,
        static avatar => avatar.HomeId,
        Codecs.LongId,
        static avatar => avatar.AccountId,
        AvatarInventorySection.Codec,
        static avatar => new AvatarInventorySection(avatar.InventoryLayout,
            avatar.InventoryValues,
            avatar.InventoryMaps,
            avatar.DeprecatedInventoryDataCount,
            avatar.InventoryUnknown0),
        AvatarSocialSection.Codec,
        static avatar => new AvatarSocialSection(avatar.RoadsideShop,
            avatar.Neighborhood,
            avatar.MailEntries,
            avatar.UnknownValues0,
            avatar.BoatCrateHelpEntries,
            avatar.TrainStationReady,
            avatar.IsMuted,
            avatar.CanEditFarm,
            avatar.PassengerReservations,
            avatar.PickedPassengers,
            avatar.ServiceHelpRequests,
            avatar.TruckOrderHelpRequests),
        AvatarProgressionSection.Codec,
        static avatar => new AvatarProgressionSection(avatar.UnknownNullableListCount,
            avatar.CurrentDerbyInstanceId,
            avatar.PreviousDerbyInstanceId,
            avatar.LeagueType,
            avatar.PreviousLeagueType,
            avatar.LeagueScore,
            avatar.DerbyProgress,
            avatar.EventManager,
            avatar.LiveGlobals,
            avatar.DerbyMinigameCounters,
            avatar.MapGameId,
            avatar.UnknownOptionalId3,
            avatar.Unknown4,
            avatar.MapGameParticipants,
            avatar.BirthTimestamp,
            avatar.AgeCountryCode,
            avatar.StorePromotionAllowed,
            avatar.UnknownString1,
            avatar.UnknownBoolean1,
            avatar.ShopPurchaseHistory,
            avatar.Settings),
        static (avatarTimestamp, unknown1, avatarVersion, secondsWithoutSpending, farmName, homeId, accountId, inventory, social, progression) => new ClientAvatar
        {
            AvatarTimestamp = avatarTimestamp,
            Unknown1 = unknown1,
            AvatarVersion = avatarVersion,
            SecondsWithoutSpending = secondsWithoutSpending,
            FarmName = farmName,
            HomeId = homeId,
            AccountId = accountId,
            InventoryValues = inventory.Values,
            InventoryLayout = inventory.Layout,
            InventoryMaps = inventory.Maps,
            DeprecatedInventoryDataCount = inventory.DeprecatedDataCount,
            InventoryUnknown0 = inventory.Unknown0,
            RoadsideShop = social.RoadsideShop,
            Neighborhood = social.Neighborhood,
            MailEntries = social.MailEntries,
            UnknownValues0 = social.UnknownValues0,
            BoatCrateHelpEntries = social.BoatCrateHelpEntries,
            TrainStationReady = social.TrainStationReady,
            IsMuted = social.IsMuted,
            CanEditFarm = social.CanEditFarm,
            PassengerReservations = social.PassengerReservations,
            PickedPassengers = social.PickedPassengers,
            ServiceHelpRequests = social.ServiceHelpRequests,
            TruckOrderHelpRequests = social.TruckOrderHelpRequests,
            UnknownNullableListCount = progression.UnknownNullableListCount,
            CurrentDerbyInstanceId = progression.CurrentDerbyInstanceId,
            PreviousDerbyInstanceId = progression.PreviousDerbyInstanceId,
            LeagueType = progression.LeagueType,
            PreviousLeagueType = progression.PreviousLeagueType,
            LeagueScore = progression.LeagueScore,
            DerbyProgress = progression.DerbyProgress,
            EventManager = progression.EventManager,
            LiveGlobals = progression.LiveGlobals,
            DerbyMinigameCounters = progression.DerbyMinigameCounters,
            MapGameId = progression.MapGameId,
            MapGameParticipants = progression.MapGameParticipants,
            UnknownOptionalId3 = progression.UnknownOptionalId3,
            Unknown4 = progression.Unknown4,
            BirthTimestamp = progression.BirthTimestamp,
            AgeCountryCode = progression.AgeCountryCode,
            StorePromotionAllowed = progression.StorePromotionAllowed,
            UnknownString1 = progression.UnknownString1,
            UnknownBoolean1 = progression.UnknownBoolean1,
            ShopPurchaseHistory = progression.ShopPurchaseHistory,
            Settings = progression.Settings,
        });

    /// <summary>Gets the number of inventory arrays in the legacy 1.72 layout.</summary>
    /// <value>
    /// Always 93: one unlock bit set for each of the 92 tables of <see cref="AvatarInventoryLayout.Legacy"/>, plus the closing
    /// shop-badge list. The 1.73 layout has more; use <see cref="InventoryLayout"/> for the layout of a decoded avatar.
    /// </value>
    public static int InventoryArrayCount => AvatarInventoryLayout.Legacy.ArrayCount;

    /// <summary>Gets the number of inventory maps in the legacy 1.72 layout.</summary>
    /// <value>Always 3, taken from <see cref="AvatarInventoryLayout.Legacy"/>; the 1.73 layout carries 4.</value>
    public static int InventoryMapCount => AvatarInventoryLayout.Legacy.MapCount;

    /// <summary>
    /// Gets or sets the account the avatar belongs to, as used for login.
    /// </summary>
    /// <value>
    /// The long id read right after <see cref="HomeId"/>; the anonymous client reads the level and experience of a loaded avatar only
    /// when this id equals the account it logged in with.
    /// </value>
    [JsonPropertyName("AccountId")]
    public LongId AccountId { get; init; }

    /// <summary>Gets or sets the country code used by age restrictions.</summary>
    /// <value>
    /// The optional string of the age block, read after <see cref="BirthTimestamp"/>; <see langword="null"/> when the server sent
    /// none.
    /// </value>
    public string? AgeCountryCode { get; init; }

    /// <summary>
    /// Gets or sets the Unix seconds at which the server last stored the avatar; in captures it sits a second or so before the load's
    /// server time.
    /// </summary>
    /// <value>The first field of the avatar record, a variable-length integer.</value>
    public int AvatarTimestamp { get; init; }

    /// <summary>
    /// Gets or sets the revision number of the avatar record (21 in 1.73.81 captures).
    /// </summary>
    /// <value>
    /// The variable-length integer read after <see cref="Unknown1"/>; it is carried unchanged.
    /// </value>
    public int AvatarVersion { get; init; }

    /// <summary>Gets or sets the retained birth timestamp used by age restrictions.</summary>
    /// <value>
    /// Unix seconds of the player's birth date; 0 or -1 means no birth date is known.
    /// </value>
    public int BirthTimestamp { get; init; }

    /// <summary>Gets or sets the native inventory layout used to decode this avatar.</summary>
    /// <value>
    /// <see cref="AvatarInventoryLayout.Legacy"/> (the default) for 1.72 connections and <see cref="AvatarInventoryLayout.WithAngus"/>
    /// for 1.73, picked from the connection's game version while decoding. It fixes how many arrays and maps
    /// <see cref="InventoryValues"/> and <see cref="InventoryMaps"/> must hold, and it is not written to JSON.
    /// </value>
    [JsonIgnore]
    public AvatarInventoryLayout InventoryLayout { get; init; } = AvatarInventoryLayout.Legacy;

    /// <summary>
    /// Gets or sets the farm-editing permission flag from the social section (<see langword="true"/> in captures).
    /// </summary>
    /// <value>
    /// <see langword="true"/> if the third flag of the social section, read after <see cref="IsMuted"/> and carried for re-encoding,
    /// is set; otherwise, <see langword="false"/>.
    /// </value>
    public bool CanEditFarm { get; init; }

    /// <summary>Gets or sets the instance id of the derby the avatar takes part in.</summary>
    /// <value>
    /// The optional long id read after <see cref="UnknownNullableListCount"/>; <see langword="null"/> while the player is in no
    /// derby.
    /// </value>
    public LongId? CurrentDerbyInstanceId { get; init; }

    /// <summary>
    /// Gets or sets the count of a deprecated inventory section; 0 in captures, kept for re-encoding.
    /// </summary>
    /// <value>
    /// The variable-length integer read after <see cref="InventoryMaps"/>. Only 0 is supported: decoding throws
    /// <see cref="InvalidDataException"/> and encoding throws <see cref="InvalidOperationException"/> for any other count.
    /// </value>
    public int DeprecatedInventoryDataCount { get; init; }

    /// <summary>
    /// Gets or sets the inventory's plain number arrays, in the order <see cref="InventoryLayout"/> defines for the game release;
    /// which array means what depends on that layout.
    /// </summary>
    /// <value>
    /// Exactly <see cref="AvatarInventoryLayout.ArrayCount"/> arrays, read right after <see cref="AccountId"/>: one unlock bit set
    /// per table of the layout, in ascending table id order, then the data ids that show a new-item shop badge. Encoding throws
    /// <see cref="InvalidOperationException"/> when the count does not match the layout.
    /// </value>
    public int[][] InventoryValues { get; init; } = [];

    /// <summary>
    /// Gets or sets the inventory's keyed lists, one per map slot of <see cref="InventoryLayout"/>: each entry pairs a game data
    /// entry (an item, building or similar) with a number, such as how many of that item are stored.
    /// </summary>
    /// <value>
    /// Exactly <see cref="AvatarInventoryLayout.MapCount"/> lists (3 for 1.72, 4 for 1.73), read after <see cref="InventoryValues"/>.
    /// The first list holds the amounts of money-table resources, such as diamonds, experience level and experience points.
    /// </value>
    public DataReferenceValue[][] InventoryMaps { get; init; } = [];

    /// <summary>
    /// Gets or sets the farm's roadside shop crates in slot order: what each offers, at what price, and whether it was bought.
    /// </summary>
    /// <value>
    /// One entry per unlocked stand, the first list of the social section; an empty stand has item id 0.
    /// </value>
    public RoadsideShopEntry[] RoadsideShop { get; set; } = [];

    /// <summary>
    /// Gets or sets the in-game farm name.
    /// </summary>
    /// <value>
    /// The optional string read after <see cref="SecondsWithoutSpending"/>; <see langword="null"/> or empty for a farm that has not
    /// been named.
    /// </value>
    public string? FarmName { get; init; }

    /// <summary>
    /// Gets or sets the player's farm, which visits and home messages refer to.
    /// </summary>
    /// <value>The home long id read after <see cref="FarmName"/>; JSON keeps it under the name HomeId.</value>
    [JsonPropertyName("HomeId")]
    public LongId HomeId { get; init; }

    /// <summary>
    /// Gets or sets the mailbox letters waiting for the player, such as gifts and rewards to collect.
    /// </summary>
    /// <value>
    /// The letters in mailbox order, read after <see cref="Neighborhood"/>.
    /// </value>
    public MailEntry[] MailEntries { get; set; } = [];

    /// <summary>
    /// Gets or sets the number list sent after the mail; its meaning is not identified and captures leave it empty.
    /// </summary>
    /// <value>
    /// A list of variable-length integers read after <see cref="MailEntries"/>, kept only so the avatar re-encodes unchanged.
    /// </value>
    public int[] UnknownValues0 { get; init; } = [];

    /// <summary>
    /// Gets or sets the number that closes the inventory section; its meaning is not identified and captures carry 0.
    /// </summary>
    /// <value>The variable-length integer read after <see cref="DeprecatedInventoryDataCount"/>, kept for re-encoding.</value>
    public int InventoryUnknown0 { get; init; }

    /// <summary>
    /// Gets or sets the help requests on the player's boat crates and, for crates already filled, who filled them.
    /// </summary>
    /// <value>
    /// One entry per crate with its expiry time, help kind, crate index and helper account, read after
    /// <see cref="UnknownValues0"/>.
    /// </value>
    public BoatCrateHelpEntry[] BoatCrateHelpEntries { get; set; } = [];

    /// <summary>
    /// Gets or sets the mute flag from the social section (<see langword="false"/> in captures).
    /// </summary>
    /// <value>
    /// <see langword="true"/> if the second flag of the social section, read after <see cref="TrainStationReady"/> and carried for
    /// re-encoding, is set; otherwise, <see langword="false"/>.
    /// </value>
    public bool IsMuted { get; init; }

    /// <summary>
    /// Gets or sets the player's points in the running derby.
    /// </summary>
    /// <value>The point total read after <see cref="PreviousLeagueType"/>; 0 until the player earns derby points.</value>
    public int LeagueScore { get; init; }

    /// <summary>
    /// Gets or sets the league of the running derby the player competes in, as native numbers leagues.
    /// </summary>
    /// <value>
    /// The league number read after <see cref="PreviousDerbyInstanceId"/>.
    /// </value>
    public int LeagueType { get; init; }

    /// <summary>
    /// Gets or sets the town passenger pickups reserved by helpers, keyed by passenger help id.
    /// </summary>
    /// <value>
    /// One entry per reserved passenger, read after <see cref="CanEditFarm"/>; each names the home that picks the passenger up, or
    /// <see langword="null"/> until a helper claims it.
    /// </value>
    public PassengerReservation[] PassengerReservations { get; init; } = [];

    /// <summary>
    /// Gets or sets the passengers the player picked up in other players' towns and has not yet brought home on the personal train.
    /// </summary>
    /// <value>
    /// One entry per passenger, read after <see cref="PassengerReservations"/>: its passenger table id, the home whose train picked
    /// it up and its saved object state.
    /// </value>
    public PickedPassenger[] PickedPassengers { get; init; } = [];

    /// <summary>
    /// Gets or sets the Valley game the player is in; <see langword="null"/> when not playing one.
    /// </summary>
    /// <value>
    /// The optional long id read after <see cref="DerbyMinigameCounters"/>.
    /// </value>
    [JsonPropertyName("MapGameId")]
    public LongId? MapGameId { get; init; }

    /// <summary>
    /// Gets or sets the players taking part in the same Valley game; <see langword="null"/> when the avatar carries no Valley game.
    /// </summary>
    /// <value>
    /// Read after <see cref="Unknown4"/> only when <see cref="MapGameId"/> is set, as an optional list; it can also be
    /// <see langword="null"/> with a game when the server omits the list.
    /// </value>
    public MapGameParticipant[]? MapGameParticipants { get; init; }

    /// <summary>
    /// Gets or sets the player's neighbourhood: its id, name and the player's role in it; <see langword="null"/> when the player is
    /// not in one.
    /// </summary>
    /// <value>
    /// The optional block read right after <see cref="RoadsideShop"/>; besides the id, name and role it carries the badge and the
    /// points of the neighbourhood event object.
    /// </value>
    public NeighborhoodData? Neighborhood { get; init; }

    /// <summary>
    /// Gets or sets the town service help requests and the helpers that completed them.
    /// </summary>
    /// <value>
    /// One entry per help id, read after <see cref="PickedPassengers"/>; the helper home stays <see langword="null"/> until someone
    /// completes the service.
    /// </value>
    public ServiceHelpRequest[] ServiceHelpRequests { get; init; } = [];

    /// <summary>
    /// Gets or sets a second help request list with the service request layout; captures never carry entries.
    /// </summary>
    /// <value>
    /// The last list of the social section, read after <see cref="ServiceHelpRequests"/>; truck order help is tracked from it.
    /// </value>
    public ServiceHelpRequest[] TruckOrderHelpRequests { get; init; } = [];

    /// <summary>
    /// Gets or sets the id of the derby the player's neighbourhood raced in before the current one, kept so that derby's results can still be shown.
    /// </summary>
    /// <value>
    /// The optional long id read after <see cref="CurrentDerbyInstanceId"/>; <see langword="null"/> when the player took part in no
    /// earlier derby.
    /// </value>
    public LongId? PreviousDerbyInstanceId { get; init; }

    /// <summary>Gets or sets the league type of the previous derby.</summary>
    /// <value>The league number, numbered as <see cref="LeagueType"/> is, read right after it.</value>
    public int PreviousLeagueType { get; init; }

    /// <summary>
    /// Gets or sets the native <c language="csharp">LogicEventManager</c> data: live events, purchases and seen offers.
    /// </summary>
    /// <value>
    /// The event-manager block read after <see cref="DerbyProgress"/>; by default it schedules no live events and its lists are empty.
    /// </value>
    public EventManagerData EventManager { get; init; } = new();

    /// <summary>
    /// Gets or sets the elapsed seconds without real-money spending at the avatar's load boundary.
    /// </summary>
    /// <value>
    /// Seconds since the player's last real-money purchase as of the load, read after <see cref="AvatarVersion"/>.
    /// </value>
    public int SecondsWithoutSpending { get; init; }

    /// <summary>
    /// Gets or sets the native <c language="csharp">LogicLiveGlobalManager</c> configurations.
    /// </summary>
    /// <value>
    /// The JSON texts of the GENERAL, SPINWHEEL and RENOVATOR globals, read after <see cref="EventManager"/>; by default all three
    /// are absent.
    /// </value>
    public LiveGlobalConfigurations LiveGlobals { get; init; } = new();

    /// <summary>
    /// Gets or sets the settings block that closes the progression section; <see langword="null"/> when the avatar was sent without
    /// it.
    /// </summary>
    /// <value>
    /// The optional last field of the avatar record; when present it holds numbered switches whose meanings are not identified.
    /// </value>
    public AvatarSettings? Settings { get; init; }

    /// <summary>
    /// Gets or sets the shop purchase receipts and spending totals.
    /// </summary>
    /// <value>
    /// The purchase block read after <see cref="UnknownBoolean1"/>; by default it has no receipts and zero totals.
    /// </value>
    public PurchaseHistory ShopPurchaseHistory { get; init; } = new();

    /// <summary>
    /// Gets or sets the store-promotion permission flag sent next to the age-restriction fields.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if the age block, right after <see cref="AgeCountryCode"/>, marks store promotions as allowed, a flag
    /// carried only for re-encoding; otherwise, <see langword="false"/>.
    /// </value>
    public bool StorePromotionAllowed { get; init; }

    /// <summary>
    /// Gets or sets the train-station flag from the social section (<see langword="true"/> in captures); its exact meaning is not
    /// identified.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if the first flag of the social section, read after <see cref="BoatCrateHelpEntries"/>, is set;
    /// otherwise, <see langword="false"/>.
    /// </value>
    public bool TrainStationReady { get; init; }

    /// <summary>Gets or sets the bingo and bunny counters of the running and previous derby.</summary>
    /// <value>
    /// The counter block read after <see cref="LiveGlobals"/>: bingo count, bingo value and bunny count for the current and the
    /// previous derby, all 0 by default.
    /// </value>
    public AvatarDerbyMinigameCounters DerbyMinigameCounters { get; init; } = new();

    /// <summary>
    /// Gets or sets the number sent between the avatar time stamp and its revision; captures carry 0 and its meaning is not
    /// identified.
    /// </summary>
    /// <value>
    /// The second variable-length integer of the avatar record, read after <see cref="AvatarTimestamp"/>; kept for re-encoding.
    /// </value>
    public int Unknown1 { get; init; }

    /// <summary>
    /// Gets or sets the number sent after the Valley ids; captures carry 0 and its meaning is not identified.
    /// </summary>
    /// <value>
    /// The variable-length integer read after <see cref="UnknownOptionalId3"/>; kept for re-encoding.
    /// </value>
    public int Unknown4 { get; init; }

    /// <summary>
    /// Gets or sets the flag sent after the second country code; captures carry <see langword="false"/> and its meaning is not
    /// identified.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if the flag read after <see cref="UnknownString1"/>, kept only for re-encoding, is set; otherwise,
    /// <see langword="false"/>.
    /// </value>
    public bool UnknownBoolean1 { get; init; }

    /// <summary>
    /// Gets or sets the count that opens the progression section; captures carry 0 and its meaning is not identified.
    /// </summary>
    /// <value>
    /// The variable-length integer read after <see cref="TruckOrderHelpRequests"/>. Only 0 is supported: a positive count makes
    /// decoding throw <see cref="InvalidDataException"/> and encoding throw <see cref="InvalidOperationException"/>, because the
    /// section it counts is not implemented.
    /// </value>
    public int UnknownNullableListCount { get; init; }

    /// <summary>
    /// Gets or sets the optional id sent right after <see cref="MapGameId"/>; which object it names is not identified.
    /// </summary>
    /// <value>
    /// An optional long id; <see langword="null"/> when the server sent none.
    /// </value>
    [JsonPropertyName("UnknownOptionalId3")]
    public LongId? UnknownOptionalId3 { get; init; }

    /// <summary>
    /// Gets or sets the second country code sent after the store-promotion flag; captures repeat <see cref="AgeCountryCode"/>, but
    /// its own use is not identified.
    /// </summary>
    /// <value>The optional string read after <see cref="StorePromotionAllowed"/>; <see langword="null"/> when it was not sent.</value>
    public string? UnknownString1 { get; init; }

    /// <summary>Gets or sets the current and previous derby points, ranks, seasons and leagues.</summary>
    /// <value>
    /// The derby standing block read after <see cref="LeagueScore"/>, holding league indexes, neighbourhood and player points,
    /// participant counts, ranks and season ids; all 0 by default.
    /// </value>
    public AvatarDerbyProgress DerbyProgress { get; init; } = new();
}
