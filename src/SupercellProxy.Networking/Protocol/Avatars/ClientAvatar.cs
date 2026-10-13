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
/// Represents <c language="csharp">ClientAvatar</c>.
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
            avatar.UnknownEntries3
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
            UnknownEntries3 = social.UnknownEntries3,
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
    /// Gets or sets the <c language="csharp">AccountId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("AccountId")]
    public LongId AccountId { get; init; }

    /// Gets the country code used by age restrictions.
    public string? AgeCountryCode { get; init; }

    /// <summary>
    /// Gets the avatar timestamp.
    /// </summary>
    public int AvatarTimestamp { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AvatarVersion</c> value.
    /// </summary>
    public int AvatarVersion { get; init; }

    /// Gets the retained birth timestamp used by age restrictions.
    public int BirthTimestamp { get; init; }

    /// <summary>Gets the native inventory layout used to decode this avatar.</summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public AvatarInventoryLayout InventoryLayout { get; init; } = AvatarInventoryLayout.Legacy;

    /// <summary>
    /// Gets or sets the <c language="csharp">CanEditFarm</c> value.
    /// </summary>
    public bool CanEditFarm { get; init; }

    /// <summary>Gets the instance id of the derby the avatar takes part in.</summary>
    public LongId? CurrentDerbyInstanceId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">DeprecatedInventoryDataCount</c> value.
    /// </summary>
    public int DeprecatedInventoryDataCount { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">InventoryValues</c> value.
    /// </summary>
    public int[][] InventoryValues { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">InventoryMaps</c> value.
    /// </summary>
    public DataReferenceValue[][] InventoryMaps { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">RoadsideShop</c> value.
    /// </summary>
    public RoadsideShopEntry[] RoadsideShop { get; set; } = [];

    /// <summary>
    /// Gets or sets the in-game farm name.
    /// </summary>
    public string? FarmName { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">HomeId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("HomeId")]
    public LongId HomeId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">MailEntries</c> value.
    /// </summary>
    public MailEntry[] MailEntries { get; set; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownValues0</c> value.
    /// </summary>
    public int[] UnknownValues0 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">InventoryUnknown0</c> value.
    /// </summary>
    public int InventoryUnknown0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">BoatCrateHelpEntries</c> value.
    /// </summary>
    public BoatCrateHelpEntry[] BoatCrateHelpEntries { get; set; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">IsMuted</c> value.
    /// </summary>
    public bool IsMuted { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LeagueScore</c> value.
    /// </summary>
    public int LeagueScore { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LeagueType</c> value.
    /// </summary>
    public int LeagueType { get; init; }

    /// <summary>
    /// Gets the town passenger pickups reserved by helpers, keyed by passenger help id.
    /// </summary>
    public PassengerReservation[] PassengerReservations { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">PickedPassengers</c> value.
    /// </summary>
    public PickedPassenger[] PickedPassengers { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">MapGameId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("MapGameId")]
    public LongId? MapGameId { get; init; }

    /// <summary>
    /// Gets the Map Game Participants value.
    /// </summary>
    public MapGameParticipant[]? MapGameParticipants { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Neighborhood</c> value.
    /// </summary>
    public NeighborhoodData? Neighborhood { get; init; }

    /// <summary>
    /// Gets the town service help requests and the helpers that completed them.
    /// </summary>
    public ServiceHelpRequest[] ServiceHelpRequests { get; init; } = [];

    /// <summary>
    /// Gets a second help request list with the service request layout; captures never carry entries.
    /// </summary>
    public ServiceHelpRequest[] UnknownEntries3 { get; init; } = [];

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
    /// Gets or sets the <c language="csharp">Settings</c> value.
    /// </summary>
    public AvatarSettings? Settings { get; init; }

    /// <summary>
    /// Gets the shop purchase receipts and spending totals.
    /// </summary>
    public PurchaseHistory ShopPurchaseHistory { get; init; } = new();

    /// <summary>
    /// Gets or sets the <c language="csharp">StorePromotionAllowed</c> value.
    /// </summary>
    public bool StorePromotionAllowed { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TrainStationReady</c> value.
    /// </summary>
    public bool TrainStationReady { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public int Unknown1 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown4</c> value.
    /// </summary>
    public int Unknown4 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownBoolean1</c> value.
    /// </summary>
    public bool UnknownBoolean1 { get; init; }

    /// <summary>Gets the bingo and bunny counters of the running and previous derby.</summary>
    public AvatarDerbyMinigameCounters DerbyMinigameCounters { get; init; } = new();

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownNullableListCount</c> value.
    /// </summary>
    public int UnknownNullableListCount { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownOptionalId3</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownOptionalId3")]
    public LongId? UnknownOptionalId3 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownString1</c> value.
    /// </summary>
    public string? UnknownString1 { get; init; }

    /// <summary>Gets the current and previous derby points, ranks, seasons and leagues.</summary>
    public AvatarDerbyProgress DerbyProgress { get; init; } = new();
}
