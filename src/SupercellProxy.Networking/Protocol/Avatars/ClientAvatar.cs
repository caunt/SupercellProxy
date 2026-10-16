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
    public static ICodec<ClientAvatar> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
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
        static avatar => new AvatarInventorySection(
            avatar.InventoryLayout,
            avatar.InventoryValues,
            avatar.InventoryMaps,
            avatar.DeprecatedInventoryDataCount,
            avatar.InventoryUnknown0
        ),
        AvatarSocialSection.Codec,
        static avatar => new AvatarSocialSection(
            avatar.RoadsideShop,
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
            avatar.TruckOrderHelpRequests
        ),
        AvatarProgressionSection.Codec,
        static avatar => new AvatarProgressionSection(
            avatar.UnknownNullableListCount,
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
            avatar.Settings
        ),
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
        }
    );

    /// <summary>Number of inventory arrays in the legacy 1.72 layout.</summary>
    public static int InventoryArrayCount => AvatarInventoryLayout.Legacy.ArrayCount;

    /// <summary>Number of inventory maps in the legacy 1.72 layout.</summary>
    public static int InventoryMapCount => AvatarInventoryLayout.Legacy.MapCount;

    /// <summary>
    /// Account the avatar belongs to, as used for login.
    /// </summary>
    [JsonPropertyName("AccountId")]
    public LongId AccountId { get; init; }

    /// <summary>Gets the country code used by age restrictions.</summary>
    public string? AgeCountryCode { get; init; }

    /// <summary>
    /// Unix seconds at which the server last stored the avatar; in captures it sits a second or so before the load's server time.
    /// </summary>
    public int AvatarTimestamp { get; init; }

    /// <summary>
    /// Revision number of the avatar record (21 in 1.73.81 captures).
    /// </summary>
    public int AvatarVersion { get; init; }

    /// <summary>Gets the retained birth timestamp used by age restrictions.</summary>
    public int BirthTimestamp { get; init; }

    /// <summary>Gets the native inventory layout used to decode this avatar.</summary>
    [JsonIgnore]
    public AvatarInventoryLayout InventoryLayout { get; init; } = AvatarInventoryLayout.Legacy;

    /// <summary>
    /// Farm-editing permission flag from the social section (true in captures).
    /// </summary>
    public bool CanEditFarm { get; init; }

    /// <summary>Gets the instance id of the derby the avatar takes part in.</summary>
    public LongId? CurrentDerbyInstanceId { get; init; }

    /// <summary>
    /// Count native still reads from an inventory section it no longer uses; 0 in captures, kept for re-encoding.
    /// </summary>
    public int DeprecatedInventoryDataCount { get; init; }

    /// <summary>
    /// The inventory's plain number arrays, in the order <see cref="InventoryLayout"/> defines for the game release; which array
    /// means what depends on that layout.
    /// </summary>
    public int[][] InventoryValues { get; init; } = [];

    /// <summary>
    /// The inventory's keyed lists, one per map slot of <see cref="InventoryLayout"/>: each entry pairs a game data entry (an item,
    /// building or similar) with a number, such as how many of that item are stored.
    /// </summary>
    public DataReferenceValue[][] InventoryMaps { get; init; } = [];

    /// <summary>
    /// The farm's roadside shop crates in slot order: what each offers, at what price, and whether it was bought. Live updates
    /// replace it while the shop is open.
    /// </summary>
    public RoadsideShopEntry[] RoadsideShop { get; set; } = [];

    /// <summary>
    /// Gets or sets the in-game farm name.
    /// </summary>
    public string? FarmName { get; init; }

    /// <summary>
    /// The player's farm, which visits and home messages refer to.
    /// </summary>
    [JsonPropertyName("HomeId")]
    public LongId HomeId { get; init; }

    /// <summary>
    /// Mailbox letters waiting for the player, such as gifts and rewards to collect.
    /// </summary>
    public MailEntry[] MailEntries { get; set; } = [];

    /// <summary>
    /// Number list sent after the mail; its meaning is not identified and captures leave it empty.
    /// </summary>
    public int[] UnknownValues0 { get; init; } = [];

    /// <summary>
    /// Number that closes the inventory section; its meaning is not identified and captures carry 0.
    /// </summary>
    public int InventoryUnknown0 { get; init; }

    /// <summary>
    /// Help requests on the player's boat crates and, for crates already filled, who filled them.
    /// </summary>
    public BoatCrateHelpEntry[] BoatCrateHelpEntries { get; set; } = [];

    /// <summary>
    /// Mute flag from the social section (false in captures).
    /// </summary>
    public bool IsMuted { get; init; }

    /// <summary>
    /// The player's points in the running derby.
    /// </summary>
    public int LeagueScore { get; init; }

    /// <summary>
    /// League of the running derby the player competes in, as native numbers leagues.
    /// </summary>
    public int LeagueType { get; init; }

    /// <summary>
    /// Gets the town passenger pickups reserved by helpers, keyed by passenger help id.
    /// </summary>
    public PassengerReservation[] PassengerReservations { get; init; } = [];

    /// <summary>
    /// Passengers the player picked up in other players' towns and has not yet brought home on the personal train.
    /// </summary>
    public PickedPassenger[] PickedPassengers { get; init; } = [];

    /// <summary>
    /// The Valley game the player is in; null when not playing one.
    /// </summary>
    [JsonPropertyName("MapGameId")]
    public LongId? MapGameId { get; init; }

    /// <summary>
    /// Players taking part in the same Valley game; null when the avatar carries no Valley game.
    /// </summary>
    public MapGameParticipant[]? MapGameParticipants { get; init; }

    /// <summary>
    /// The player's neighbourhood: its id, name and the player's role in it; null when the player is not in one.
    /// </summary>
    public NeighborhoodData? Neighborhood { get; init; }

    /// <summary>
    /// Gets the town service help requests and the helpers that completed them.
    /// </summary>
    public ServiceHelpRequest[] ServiceHelpRequests { get; init; } = [];

    /// <summary>
    /// Gets a second help request list with the service request layout; captures never carry entries.
    /// </summary>
    public ServiceHelpRequest[] TruckOrderHelpRequests { get; init; } = [];

    /// <summary>Gets the instance id of the previous derby; native drops the previous rank, keys, bunnies and bingo lines without it.</summary>
    public LongId? PreviousDerbyInstanceId { get; init; }

    /// <summary>Gets the league type of the previous derby.</summary>
    public int PreviousLeagueType { get; init; }

    /// <summary>
    /// Gets the native <c language="csharp">LogicEventManager</c> data: live events, purchases and seen offers.
    /// </summary>
    public EventManagerData EventManager { get; init; } = new();

    /// <summary>
    /// Gets elapsed seconds without real-money spending at the avatar's load boundary.
    /// </summary>
    public int SecondsWithoutSpending { get; init; }

    /// <summary>
    /// Gets the native <c language="csharp">LogicLiveGlobalManager</c> configurations.
    /// </summary>
    public LiveGlobalConfigurations LiveGlobals { get; init; } = new();

    /// <summary>
    /// The settings block that closes the progression section; null when the avatar was sent without it.
    /// </summary>
    public AvatarSettings? Settings { get; init; }

    /// <summary>
    /// Gets the shop purchase receipts and spending totals.
    /// </summary>
    public PurchaseHistory ShopPurchaseHistory { get; init; } = new();

    /// <summary>
    /// Store-promotion permission flag sent next to the age-restriction fields.
    /// </summary>
    public bool StorePromotionAllowed { get; init; }

    /// <summary>
    /// Train-station flag from the social section (true in captures); its exact meaning is not identified.
    /// </summary>
    public bool TrainStationReady { get; init; }

    /// <summary>Gets the bingo and bunny counters of the running and previous derby.</summary>
    public AvatarDerbyMinigameCounters DerbyMinigameCounters { get; init; } = new();

    /// <summary>
    /// Number sent between the avatar time stamp and its revision; captures carry 0 and its meaning is not identified.
    /// </summary>
    public int Unknown1 { get; init; }

    /// <summary>
    /// Number sent after the Valley ids; captures carry 0 and its meaning is not identified.
    /// </summary>
    public int Unknown4 { get; init; }

    /// <summary>
    /// Flag sent after the second country code; captures carry false and its meaning is not identified.
    /// </summary>
    public bool UnknownBoolean1 { get; init; }

    /// <summary>
    /// Count that opens the progression section; captures carry 0 and its meaning is not identified.
    /// </summary>
    public int UnknownNullableListCount { get; init; }

    /// <summary>
    /// Optional id sent right after <see cref="MapGameId"/>; which object it names is not identified.
    /// </summary>
    [JsonPropertyName("UnknownOptionalId3")]
    public LongId? UnknownOptionalId3 { get; init; }

    /// <summary>
    /// Second country code sent after the store-promotion flag; captures repeat <see cref="AgeCountryCode"/>, but its own use is not
    /// identified.
    /// </summary>
    public string? UnknownString1 { get; init; }

    /// <summary>Gets the current and previous derby points, ranks, seasons and leagues.</summary>
    public AvatarDerbyProgress DerbyProgress { get; init; } = new();
}
