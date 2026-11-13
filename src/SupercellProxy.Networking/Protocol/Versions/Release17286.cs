using SupercellProxy.Networking.Protocol.Accounts;
using SupercellProxy.Networking.Protocol.Accounts.Binding;
using SupercellProxy.Networking.Protocol.Accounts.DeviceLinking;
using SupercellProxy.Networking.Protocol.Accounts.Permissions;
using SupercellProxy.Networking.Protocol.Accounts.Rewards;
using SupercellProxy.Networking.Protocol.Accounts.Settings;
using SupercellProxy.Networking.Protocol.Accounts.SupercellId;
using SupercellProxy.Networking.Protocol.Accounts.Unlock;
using SupercellProxy.Networking.Protocol.Achievements;
using SupercellProxy.Networking.Protocol.Advertisements;
using SupercellProxy.Networking.Protocol.Animals;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.Avatars.Data;
using SupercellProxy.Networking.Protocol.Avatars.Information;
using SupercellProxy.Networking.Protocol.Avatars.Names;
using SupercellProxy.Networking.Protocol.Avatars.Streams;
using SupercellProxy.Networking.Protocol.Balloons;
using SupercellProxy.Networking.Protocol.Billing;
using SupercellProxy.Networking.Protocol.Boats;
using SupercellProxy.Networking.Protocol.Boats.Help;
using SupercellProxy.Networking.Protocol.Boats.Orders;
using SupercellProxy.Networking.Protocol.Boats.States;
using SupercellProxy.Networking.Protocol.Boosters;
using SupercellProxy.Networking.Protocol.Boosters.FreshBeats;
using SupercellProxy.Networking.Protocol.Buildings.Upgrades;
using SupercellProxy.Networking.Protocol.ConnectionControl;
using SupercellProxy.Networking.Protocol.Creatures;
using SupercellProxy.Networking.Protocol.CropFields;
using SupercellProxy.Networking.Protocol.Currency;
using SupercellProxy.Networking.Protocol.Customization;
using SupercellProxy.Networking.Protocol.DailyTasks;
using SupercellProxy.Networking.Protocol.DataTables;
using SupercellProxy.Networking.Protocol.Debugging;
using SupercellProxy.Networking.Protocol.DecorationEvents;
using SupercellProxy.Networking.Protocol.Decorations;
using SupercellProxy.Networking.Protocol.Decorations.Modular;
using SupercellProxy.Networking.Protocol.Deeplinks;
using SupercellProxy.Networking.Protocol.Derby;
using SupercellProxy.Networking.Protocol.Emotes;
using SupercellProxy.Networking.Protocol.Events;
using SupercellProxy.Networking.Protocol.Events.Birthday;
using SupercellProxy.Networking.Protocol.Events.Board;
using SupercellProxy.Networking.Protocol.Events.Boards;
using SupercellProxy.Networking.Protocol.Events.BonusRounds;
using SupercellProxy.Networking.Protocol.Events.Calendar;
using SupercellProxy.Networking.Protocol.Events.ChainOffers.Chapters;
using SupercellProxy.Networking.Protocol.Events.Chronos;
using SupercellProxy.Networking.Protocol.Events.Counters;
using SupercellProxy.Networking.Protocol.Events.CountyFair;
using SupercellProxy.Networking.Protocol.Events.Decoration;
using SupercellProxy.Networking.Protocol.Events.Decoration.Canvas;
using SupercellProxy.Networking.Protocol.Events.Decoration.Designs;
using SupercellProxy.Networking.Protocol.Events.Decoration.Gallery;
using SupercellProxy.Networking.Protocol.Events.Decoration.StickerBook;
using SupercellProxy.Networking.Protocol.Events.Decoration.Voting;
using SupercellProxy.Networking.Protocol.Events.Derby.Commands;
using SupercellProxy.Networking.Protocol.Events.Derby.Commands.Rewards;
using SupercellProxy.Networking.Protocol.Events.Derby.Messages;
using SupercellProxy.Networking.Protocol.Events.Derby.Messages.History;
using SupercellProxy.Networking.Protocol.Events.Derby.Messages.Race;
using SupercellProxy.Networking.Protocol.Events.Pop;
using SupercellProxy.Networking.Protocol.Events.Tasks;
using SupercellProxy.Networking.Protocol.Events.TradingSeasons;
using SupercellProxy.Networking.Protocol.Events.Wrapped;
using SupercellProxy.Networking.Protocol.Expansion;
using SupercellProxy.Networking.Protocol.FarmLayouts;
using SupercellProxy.Networking.Protocol.FarmLayouts.Names;
using SupercellProxy.Networking.Protocol.FarmPass;
using SupercellProxy.Networking.Protocol.FarmPass.Rewards;
using SupercellProxy.Networking.Protocol.FarmPass.Season;
using SupercellProxy.Networking.Protocol.FarmPass.Seen;
using SupercellProxy.Networking.Protocol.FarmPass.Tasks;
using SupercellProxy.Networking.Protocol.Fishing;
using SupercellProxy.Networking.Protocol.Fishing.Ducks;
using SupercellProxy.Networking.Protocol.Fishing.Fishes;
using SupercellProxy.Networking.Protocol.Fishing.Lobsters;
using SupercellProxy.Networking.Protocol.Fishing.Molluscs;
using SupercellProxy.Networking.Protocol.Fishing.Spots;
using SupercellProxy.Networking.Protocol.Forestry;
using SupercellProxy.Networking.Protocol.Friends;
using SupercellProxy.Networking.Protocol.Friends.Bar;
using SupercellProxy.Networking.Protocol.Friends.Book;
using SupercellProxy.Networking.Protocol.Friends.Entries;
using SupercellProxy.Networking.Protocol.Friends.Helpers;
using SupercellProxy.Networking.Protocol.Friends.Search;
using SupercellProxy.Networking.Protocol.Friends.Social;
using SupercellProxy.Networking.Protocol.Friends.SupercellId;
using SupercellProxy.Networking.Protocol.GameObjects;
using SupercellProxy.Networking.Protocol.GameObjects.Placement;
using SupercellProxy.Networking.Protocol.Gatherers;
using SupercellProxy.Networking.Protocol.Gifts;
using SupercellProxy.Networking.Protocol.Helpers;
using SupercellProxy.Networking.Protocol.Helpers.Boy;
using SupercellProxy.Networking.Protocol.Homes;
using SupercellProxy.Networking.Protocol.Homes.Details;
using SupercellProxy.Networking.Protocol.Homes.Requests;
using SupercellProxy.Networking.Protocol.Inventory;
using SupercellProxy.Networking.Protocol.Inventory.Currency;
using SupercellProxy.Networking.Protocol.Inventory.Shop;
using SupercellProxy.Networking.Protocol.Mail;
using SupercellProxy.Networking.Protocol.MapGame;
using SupercellProxy.Networking.Protocol.MapGame.Animals;
using SupercellProxy.Networking.Protocol.MapGame.Events;
using SupercellProxy.Networking.Protocol.MapGame.Membership;
using SupercellProxy.Networking.Protocol.MapGame.Movement;
using SupercellProxy.Networking.Protocol.MapGame.Notifications;
using SupercellProxy.Networking.Protocol.MapGame.Quests;
using SupercellProxy.Networking.Protocol.MapGame.Rewards;
using SupercellProxy.Networking.Protocol.MapGame.Seen;
using SupercellProxy.Networking.Protocol.MapGame.Tasks;
using SupercellProxy.Networking.Protocol.MapGame.Wallets;
using SupercellProxy.Networking.Protocol.MiniPass;
using SupercellProxy.Networking.Protocol.Mining;
using SupercellProxy.Networking.Protocol.MovieTickets;
using SupercellProxy.Networking.Protocol.MysteryBoxes;
using SupercellProxy.Networking.Protocol.Neighborhoods;
using SupercellProxy.Networking.Protocol.Neighborhoods.Chat;
using SupercellProxy.Networking.Protocol.Neighborhoods.Chat.Reactions;
using SupercellProxy.Networking.Protocol.Neighborhoods.Directory;
using SupercellProxy.Networking.Protocol.Neighborhoods.Events;
using SupercellProxy.Networking.Protocol.Neighborhoods.FarmPassGifts;
using SupercellProxy.Networking.Protocol.Neighborhoods.Help;
using SupercellProxy.Networking.Protocol.Neighborhoods.Joining;
using SupercellProxy.Networking.Protocol.Neighborhoods.Management;
using SupercellProxy.Networking.Protocol.Neighborhoods.Members;
using SupercellProxy.Networking.Protocol.Neighborhoods.Membership;
using SupercellProxy.Networking.Protocol.Neighborhoods.Objects;
using SupercellProxy.Networking.Protocol.Neighborhoods.Requests;
using SupercellProxy.Networking.Protocol.Neighborhoods.Rewards;
using SupercellProxy.Networking.Protocol.Neighborhoods.Search;
using SupercellProxy.Networking.Protocol.Neighborhoods.Streams;
using SupercellProxy.Networking.Protocol.Neighborhoods.Tasks;
using SupercellProxy.Networking.Protocol.Newspapers;
using SupercellProxy.Networking.Protocol.Offers;
using SupercellProxy.Networking.Protocol.Offers.PopPromos;
using SupercellProxy.Networking.Protocol.Offers.Promotions;
using SupercellProxy.Networking.Protocol.Orders;
using SupercellProxy.Networking.Protocol.Orders.Help;
using SupercellProxy.Networking.Protocol.Orders.Remote;
using SupercellProxy.Networking.Protocol.Pets;
using SupercellProxy.Networking.Protocol.PrizeWheels;
using SupercellProxy.Networking.Protocol.Production;
using SupercellProxy.Networking.Protocol.Progression;
using SupercellProxy.Networking.Protocol.Rankings;
using SupercellProxy.Networking.Protocol.Recipients;
using SupercellProxy.Networking.Protocol.Reengagement;
using SupercellProxy.Networking.Protocol.Rewards;
using SupercellProxy.Networking.Protocol.RoadsideShops;
using SupercellProxy.Networking.Protocol.RoadsideShops.Advertisements;
using SupercellProxy.Networking.Protocol.RoadsideShops.AiShop;
using SupercellProxy.Networking.Protocol.RoadsideShops.Sales;
using SupercellProxy.Networking.Protocol.RoadsideShops.Stands;
using SupercellProxy.Networking.Protocol.Sanctuary;
using SupercellProxy.Networking.Protocol.Sanctuary.Puzzles;
using SupercellProxy.Networking.Protocol.Screens;
using SupercellProxy.Networking.Protocol.SeasonalCollectibles;
using SupercellProxy.Networking.Protocol.Settings;
using SupercellProxy.Networking.Protocol.Settings.Ads;
using SupercellProxy.Networking.Protocol.Settings.Preferences;
using SupercellProxy.Networking.Protocol.ShopEvents;
using SupercellProxy.Networking.Protocol.ShopEvents.Purchases;
using SupercellProxy.Networking.Protocol.SpeedUps;
using SupercellProxy.Networking.Protocol.Support;
using SupercellProxy.Networking.Protocol.Surveys;
using SupercellProxy.Networking.Protocol.Telemetry;
using SupercellProxy.Networking.Protocol.Telemetry.Analytics;
using SupercellProxy.Networking.Protocol.Telemetry.Soil;
using SupercellProxy.Networking.Protocol.Timing;
using SupercellProxy.Networking.Protocol.Town;
using SupercellProxy.Networking.Protocol.Town.Circus;
using SupercellProxy.Networking.Protocol.Town.Hall;
using SupercellProxy.Networking.Protocol.Town.Passengers;
using SupercellProxy.Networking.Protocol.Town.Services;
using SupercellProxy.Networking.Protocol.Town.Trains;
using SupercellProxy.Networking.Protocol.Turns;
using SupercellProxy.Networking.Protocol.Tutorials;

using static SupercellProxy.Networking.Protocol.Versions.GameRelease;
using SupercellProxy.Networking.Protocol.Visitors;
using SupercellProxy.Networking.Protocol.Visitors.Friendship;
using SupercellProxy.Networking.Protocol.Visitors.Orders;
using SupercellProxy.Networking.Protocol.Visitors.Quests;
using SupercellProxy.Networking.Protocol.Workbench;

namespace SupercellProxy.Networking.Protocol.Versions;

/// <summary>Hay Day 1.72.86, the oldest release with an id table; it lists every command and message id.</summary>
internal static class Release17286
{
    /// <summary>Gets the release version.</summary>
    public static Version Version { get; } = new(major: 1, minor: 72, build: 86);

    /// <summary>Gets the release and its id table.</summary>
    internal static GameRelease Release => field ??= new GameRelease(Version,
        Commands:
        [
            Command(MoveGameObjectByOffsetCommand.Codec, id: 3),
            Command(ScareLivestockCommand.Codec, id: 6),
            Command(ApplyFieldBoosterCommand.Codec, id: 7),
            Command(UpgradeBuildingCommand.Codec, id: 11),
            Command(SpeedUpAnimalCommand.Codec, id: 15),
            Command(SpeedUpConstructionCommand.Codec, id: 16),
            Command(CompleteConstructionCommand.Codec, id: 17),
            Command(StartForestClearingCommand.Codec, id: 18),
            Command(SpeedUpForestClearingCommand.Codec, id: 19),
            Command(CompleteForestClearingCommand.Codec, id: 20),
            Command(SelectLivestockAnimalCommand.Codec, id: 21),
            Command(SpeedUpFieldCommand.Codec, id: 24),
            Command(UpgradeStorageBuildingCommand.Codec, id: 25),
            Command(MarkTruckOrdersSeenCommand.Codec, id: 26),
            Command(CancelTruckOrderCommand.Codec, id: 27),
            Command(SpeedUpTruckOrderCommand.Codec, id: 29),
            Command(AcknowledgeLevelUpCommand.Codec, id: 33),
            Command(FinishTutorialCommand.Codec, id: 34),
            Command(StartTutorialCommand.Codec, id: 35),
            Command(AdvanceTimersServerCommand.Codec, id: 38),
            Command(BuyGoodsWithDiamondsCommand.Codec, id: 39),
            Command(TapAmbientAnimalCommand.Codec, id: 42),
            Command(RejectFarmVisitorOrderCommand.Codec, id: 43),
            Command(OpenMysteryBoxCommand.Codec, id: 44),
            Command(DiscardMysteryBoxCommand.Codec, id: 45),
            Command(CheckMysteryBoxLockCommand.Codec, id: 46),
            Command(CollectRoadsideShopSaleCommand.Codec, id: 47),
            Command(CollectMysteryBoxRewardCommand.Codec, id: 48),
            Command(SetAppRateStateCommand.Codec, id: 49),
            Command(SpawnAmbientAnimalCommand.Codec, id: 50),
            Command(ClaimAchievementRewardCommand.Codec, id: 51),
            Command(MirrorGameObjectCommand.Codec, id: 53),
            Command(RemoveGameObjectCommand.Codec, id: 54),
            Command(SpeedUpFruitTreeCommand.Codec, id: 59),
            Command(StartPlantClearingCommand.Codec, id: 60),
            Command(CompletePlantClearingCommand.Codec, id: 61),
            Command(RevivePlantCommand.Codec, id: 63),
            Command(MineCommand.Codec, id: 64),
            Command(RequestPlantRevivalCommand.Codec, id: 65),
            Command(FinishPlantRevivalCommand.Codec, id: 66),
            Command(HireBoyCommand.Codec, id: 68),
            Command(SelectBoyOfferCommand.Codec, id: 70),
            Command(SearchWithBoyCommand.Codec, id: 71),
            Command(CollectWheelRewardCommand.Codec, id: 80),
            Command(DebugWheelCarsCommand.Codec, id: 84),
            Command(DebugActionCommand.Codec, id: 85),
            Command(FeedPetHabitatCommand.Codec, id: 86),
            Command(WakePetCommand.Codec, id: 87),
            Command(TapPetCommand.Codec, id: 88),
            Command(CallPetsToHabitatCommand.Codec, id: 89),
            Command(ExpandLandCommand.Codec, id: 90),
            Command(RequestBoatCrateHelpCommand.Codec, id: 91),
            Command(StoreGameObjectCommand.Codec, id: 93),
            Command(SpeedUpProductionCommand.Codec, id: 96),
            Command(BuyToolOfferCommand.Codec, id: 97),
            Command(PopBalloonCommand.Codec, id: 98),
            Command(BuyCashPackageCommand.Codec, id: 100),
            Command(SetLastOpenedFriendLeaderboardCommand.Codec, id: 101),
            Command(SendThankYouGiftCommand.Codec, id: 102),
            Command(BuyCatalogueGiftCommand.Codec, id: 104),
            Command(CollectGiftCommand.Codec, id: 105),
            Command(BuyCatalogueGiftWithDiamondsCommand.Codec, id: 107),
            Command(CollectFishingSpotCommand.Codec, id: 109),
            Command(CompleteFishingCatchCommand.Codec, id: 110),
            Command(PlaceFishingBaitCommand.Codec, id: 111),
            Command(SetFishStateCommand.Codec, id: 112),
            Command(RestockFishingSpotCommand.Codec, id: 113),
            Command(SpeedUpFishingSpotCommand.Codec, id: 114),
            Command(MarkFishSeenCommand.Codec, id: 115),
            Command(MarkEventBoardSeenCommand.Codec, id: 116),
            Command(PlaceFishingNetCommand.Codec, id: 118),
            Command(CollectFishingTrapCommand.Codec, id: 119),
            Command(SelectLobsterCommand.Codec, id: 120),
            Command(CollectFishingAnimalCommand.LobsterCodec, id: 121),
            Command(RemoveFishingAnimalCommand.LobsterCodec, id: 122),
            Command(SpeedUpLobsterCommand.Codec, id: 123),
            Command(MoveGameObjectCommand.Codec, id: 124),
            Command(FinishBuildingUpgradeWithDiamondsCommand.Codec, id: 125),
            Command(CompleteBuildingUpgradeCommand.Codec, id: 126),
            Command(CollectSeasonalCollectibleCommand.Codec, id: 128),
            Command(SetSettingsFlagCommand.Codec, id: 130),
            Command(SetUserSettingCommand.Codec, id: 131),
            Command(SetBoyOfferFlagCommand.Codec, id: 132),
            Command(JoinNeighborhoodServerCommand.Codec, id: 134),
            Command(LeaveNeighborhoodServerCommand.Codec, id: 135),
            Command(NeighborhoodEmblemChangedServerCommand.Codec, id: 136),
            Command(NeighborhoodRoleChangedServerCommand.Codec, id: 137),
            Command(KickNeighborhoodMemberCommand.Codec, id: 138),
            Command(MarkNeighborhoodChatReadCommand.Codec, id: 139),
            Command(TownPassengerActionCommand.Codec, id: 140),
            Command(ValidateTownPassengerCommand.Codec, id: 141),
            Command(StartTownServiceCommand.Codec, id: 142),
            Command(SpeedUpTownServiceCommand.Codec, id: 143),
            Command(CollectTownServiceCommand.Codec, id: 144),
            Command(BookTownPassengerServiceCommand.Codec, id: 145),
            Command(CancelTownServiceCommand.Codec, id: 147),
            Command(PassengerPickedFromNeighborhoodServerCommand.Codec, id: 148),
            Command(SharedPassengersServerCommand.Codec, id: 149),
            Command(MarkTownCalendarEventSeenCommand.Codec, id: 150),
            Command(PickUpTownVisitorPassengersCommand.Codec, id: 152),
            Command(BuyGathererCommand.Codec, id: 154),
            Command(SpeedUpGathererHabitatCommand.Codec, id: 155),
            Command(CollectGathererNestCommand.Codec, id: 156),
            Command(SpeedUpGathererNestCommand.Codec, id: 157),
            Command(CollectFishingAnimalCommand.DuckCodec, id: 159),
            Command(SpeedUpDuckCommand.Codec, id: 160),
            Command(RemoveFishingAnimalCommand.DuckCodec, id: 161),
            Command(SelectDuckCommand.Codec, id: 162),
            Command(FeedCircusAnimalCommand.Codec, id: 165),
            Command(CompleteCircusTrainPartCommand.Codec, id: 166),
            Command(AssignDerbyServerCommand.Codec, id: 167),
            Command(EndDerbyServerCommand.Codec, id: 168),
            Command(ResolveCompletedDerbyTaskCommand.Codec, id: 169),
            Command(StartDerbyTaskServerCommand.Codec, id: 170),
            Command(ResolveDerbyTaskServerCommand.Codec, id: 171),
            Command(RerollDerbyRewardsCommand.Codec, id: 172),
            Command(ClaimDerbyRewardsCommand.Codec, id: 173),
            Command(UpdateDerbyPointsServerCommand.Codec, id: 174),
            Command(AcknowledgeDerbyEntriesCommand.Codec, id: 176),
            Command(MarkDerbyRewardsSeenCommand.Codec, id: 178),
            Command(RecordDerbyHelpServerCommand.Codec, id: 179),
            Command(SetDerbyTaskTimeServerCommand.Codec, id: 181),
            Command(NeighborhoodNotificationServerCommand.Codec, id: 182),
            Command(SetCampaignPackageStateCommand.Codec, id: 183),
            Command(SetCurrentDerbyLeagueServerCommand.Codec, id: 184),
            Command(PurchaseLayoutSlotCommand.Codec, id: 185),
            Command(StartLayoutEditCommand.Codec, id: 187),
            Command(DiscardLayoutChangesCommand.Codec, id: 188),
            Command(SaveLayoutChangesCommand.Codec, id: 189),
            Command(RemoveLayoutObjectCommand.Codec, id: 190),
            Command(CopyLayoutCommand.Codec, id: 191),
            Command(PlaceLayoutObjectCommand.Codec, id: 192),
            Command(SetActiveLayoutCommand.Codec, id: 193),
            Command(StoreLayoutCategoryCommand.Codec, id: 194),
            Command(RecoverLayoutChangesCommand.Codec, id: 196),
            Command(NeighborhoodItemDonatedServerCommand.Codec, id: 197),
            Command(NeighborhoodItemReceivedServerCommand.Codec, id: 198),
            Command(RequestNeighborhoodItemCommand.Codec, id: 199),
            Command(NeighborhoodRequestUpdatedServerCommand.Codec, id: 200),
            Command(MarkTradingSeasonCheckedCommand.Codec, id: 202),
            Command(HireHelperCommand.Codec, id: 204),
            Command(SetHelperOrdersCommand.Codec, id: 205),
            Command(MailEntryStateServerCommand.Codec, id: 207),
            Command(SetHelperHouseModeCommand.Codec, id: 208),
            Command(SetHelperHireOfferStateCommand.Codec, id: 209),
            Command(RoadsideFriendCountServerCommand.Codec, id: 210),
            Command(SetDerbyParticipationServerCommand.Codec, id: 211),
            Command(ActivateBoosterCommand.Codec, id: 212),
            Command(DiscardBoosterCommand.Codec, id: 213),
            Command(ExchangeBoosterCommand.Codec, id: 214),
            Command(DiscardPendingBoosterCommand.Codec, id: 215),
            Command(BuyBoosterCommand.Codec, id: 216),
            Command(CallSanctuaryAnimalsToHabitatCommand.Codec, id: 217),
            Command(FeedSanctuaryHabitatCommand.Codec, id: 218),
            Command(WakeSanctuaryAnimalCommand.Codec, id: 219),
            Command(TapSanctuaryAnimalCommand.Codec, id: 220),
            Command(PlaceSanctuaryPuzzlePiecesCommand.Codec, id: 221),
            Command(TouchParachuteCommand.Codec, id: 223),
            Command(MarkNewsStoryViewedCommand.Codec, id: 225),
            Command(GrantEventPointsServerCommand.Codec, id: 226),
            Command(RequestShopPurchaseCommand.Codec, id: 227),
            Command(ShopPurchaseResultServerCommand.Codec, id: 228),
            Command(ShopPurchaseServerCommand.Codec, id: 229),
            Command(RecordEventSeenCommand.Codec, id: 230),
            Command(DerbyNotificationServerCommand.Codec, id: 231),
            Command(SelectCustomizationsCommand.Codec, id: 233),
            Command(SynchronizeDerbyTaskServerCommand.Codec, id: 234),
            Command(RerollCustomizationOffersCommand.Codec, id: 235),
            Command(MarkCustomizationOfferCommand.Codec, id: 236),
            Command(ApplyCustomizationCommand.Codec, id: 237),
            Command(UnlockStyleGuideCommand.Codec, id: 238),
            Command(SelectTownHallSettingCommand.Codec, id: 240),
            Command(RequestInitialCustomizationOffersCommand.Codec, id: 241),
            Command(RoadsidePurchaseServerCommand.Codec, id: 243),
            Command(RoadsideStockServerCommand.Codec, id: 244),
            Command(RenameAnimalServerCommand.Codec, id: 245),
            Command(ExpireDerbyTaskServerCommand.Codec, id: 246),
            Command(BuyBackCustomizationsCommand.Codec, id: 247),
            Command(RevokeGoodServerCommand.Codec, id: 248),
            Command(DerbyTaskProgressServerCommand.Codec, id: 249),
            Command(RequestTownServiceHelpCommand.Codec, id: 250),
            Command(HelpTownServiceCommand.Codec, id: 251),
            Command(HelpReceivedServerCommand.Codec, id: 252),
            Command(TownServiceHelpCompletedServerCommand.Codec, id: 253),
            Command(ThankTownServiceHelperCommand.Codec, id: 254),
            Command(CollectTownServiceHelpCommand.Codec, id: 255),
            Command(TownServiceHelpUpdatedServerCommand.Codec, id: 256),
            Command(RequestTruckOrderHelpCommand.Codec, id: 259),
            Command(HelpTruckOrderCommand.Codec, id: 260),
            Command(TruckOrderHelpReceivedServerCommand.Codec, id: 261),
            Command(RemoteOrderCompletionServerCommand.Codec, id: 262),
            Command(RemoteOrderUpdatesServerCommand.Codec, id: 263),
            Command(ThankTruckOrderHelperCommand.Codec, id: 264),
            Command(RecordPromotionPopupStateCommand.Codec, id: 265),
            Command(OpenDeeplinkServerCommand.Codec, id: 266),
            Command(RequestAutomaticNeighborhoodCreationCommand.Codec, id: 267),
            Command(RepairBuildingCommand.Codec, id: 268),
            Command(RevokePuzzlePieceServerCommand.Codec, id: 269),
            Command(MoveMapGameCommand.Codec, id: 270),
            Command(SetMapGameServerCommand.Codec, id: 272),
            Command(EndMapGameServerCommand.Codec, id: 273),
            Command(MapGameEventsServerCommand.Codec, id: 274),
            Command(MapGameTaskCommand.ContractCodecs[MapGameTaskKind.CompleteDumpTask], id: 278),
            Command(MapGameTaskCommand.ContractCodecs[MapGameTaskKind.JoinSharedTask], id: 279),
            Command(MapGameTaskCommand.ContractCodecs[MapGameTaskKind.CompleteDeliveryTask], id: 280),
            Command(MapGameTaskCommand.ContractCodecs[MapGameTaskKind.SkipTaskCooldown], id: 281),
            Command(MapGameTaskCommand.ContractCodecs[MapGameTaskKind.CompleteObstacleWithPayment], id: 282),
            Command(MapGameTaskCommand.ContractCodecs[MapGameTaskKind.RequestObstacleHelp], id: 283),
            Command(MapGameTaskCommand.ContractCodecs[MapGameTaskKind.CompleteObstacle], id: 284),
            Command(ClaimMapGameQuestProgressionPrizeCommand.Codec, id: 285),
            Command(MarkMapGameDailyQuestSeenCommand.Codec, id: 286),
            Command(RemoveMapGameNotificationCommand.Codec, id: 288),
            Command(MapGameTaskCommand.ContractCodecs[MapGameTaskKind.FindChicken], id: 290),
            Command(MapGameTaskCommand.ContractCodecs[MapGameTaskKind.CollectChicken], id: 291),
            Command(MapGameTaskCommand.ContractCodecs[MapGameTaskKind.FindSharedTaskChicken], id: 295),
            Command(BuyMapGameShopRewardCommand.Codec, id: 296),
            Command(MarkMapGameEventSeenCommand.Codec, id: 297),
            Command(CollectMapGamePiggyBankCommand.Codec, id: 299),
            Command(ClaimAccountLinkRewardCommand.Codec, id: 300),
            Command(UpdateMapGameQuestProgressCommand.Codec, id: 302),
            Command(ClaimMapGameQuestRewardCommand.Codec, id: 303),
            Command(ProcessBoatCrateHelpServerCommand.Codec, id: 304),
            Command(BirthdayHugsServerCommand.Codec, id: 305),
            Command(SetBirthdayHugsEnabledCommand.Codec, id: 306),
            Command(RoadsidePurchaseRejectedServerCommand.Codec, id: 309),
            Command(MapGameTaskCommand.ContractCodecs[MapGameTaskKind.CollectSanctuaryAnimal], id: 310),
            Command(MapGameTaskCommand.ContractCodecs[MapGameTaskKind.CollectGasStation], id: 312),
            Command(BonusRoundRewardServerCommand.Codec, id: 313),
            Command(MapGameTaskCommand.ContractCodecs[MapGameTaskKind.PurchaseGasStation], id: 314),
            Command(FinishBonusRoundCommand.Codec, id: 316),
            Command(ScratchBonusRoundSlotCommand.Codec, id: 317),
            Command(ClaimMapGameDailyQuestRewardCommand.Codec, id: 318),
            Command(BuyNextBonusRoundCommand.Codec, id: 319),
            Command(MapGamePawnTaskCommand.Codec, id: 321),
            Command(SetModularDecorationOptionsCommand.Codec, id: 322),
            Command(DerbySeasonNotificationServerCommand.Codec, id: 323),
            Command(MarkPopEventSeenCommand.Codec, id: 324),
            Command(ExternalPurchaseServerCommand.Codec, id: 325),
            Command(MarkFlyingDerbyBunniesSeenCommand.Codec, id: 326),
            Command(TreeRevivalServerCommand.Codec, id: 328),
            Command(MarkPopPromoSeenCommand.Codec, id: 329),
            Command(ClaimPopPromoRewardCommand.Codec, id: 330),
            Command(PopPromoUpdateServerCommand.Codec, id: 331),
            Command(DismissFarmPassNotificationCommand.Codec, id: 333),
            Command(ClaimNeighborhoodFarmPassGiftCommand.Codec, id: 334),
            Command(SelectFarmPassRewardCommand.Codec, id: 335),
            Command(ClaimFarmPassLevelRewardCommand.Codec, id: 336),
            Command(ClaimFarmPassTaskGoalRewardCommand.Codec, id: 338),
            Command(TapBabyPetCommand.Codec, id: 340),
            Command(SetAnimalAccessoryCommand.Codec, id: 342),
            Command(ActivateFarmPassPerkCommand.Codec, id: 343),
            Command(GrantResourcesServerCommand.Codec, id: 344),
            Command(ClaimFarmPassBabyPetRewardCommand.Codec, id: 346),
            Command(ShopOfferStateServerCommand.Codec, id: 349),
            Command(ConfirmShopPurchaseServerCommand.Codec, id: 350),
            Command(CancelShopPurchaseCommand.Codec, id: 351),
            Command(RoadsideShopSaleServerCommand.Codec, id: 353),
            Command(ShopEventsServerCommand.Codec, id: 355),
            Command(PerformEventTaskActionCommand.Codec, id: 359),
            Command(ClaimEventTaskRewardCommand.Codec, id: 360),
            Command(MarkTaskEventOpenedCommand.Codec, id: 361),
            Command(SpeedUpEventTaskCommand.Codec, id: 363),
            Command(EventLeaderboardUpdateServerCommand.Codec, id: 366),
            Command(ClaimEventMilestoneRewardCommand.Codec, id: 367),
            Command(SubmitEventTaskCommand.Codec, id: 368),
            Command(SetManagerValueCommand.Codec, id: 369),
            Command(ClaimBirthdayEventLeaderboardRewardCommand.Codec, id: 370),
            Command(ClearEventLeaderboardNotificationCommand.Codec, id: 371),
            Command(OfferDeliveredServerCommand.Codec, id: 372),
            Command(FailWithErrorCodeCommand.Codec, id: 373),
            Command(DiscardFarmPassTaskCommand.Codec, id: 374),
            Command(RoadsideSaleServerCommand.Codec, id: 375),
            Command(SkipFarmPassTaskCooldownCommand.Codec, id: 376),
            Command(CompleteFarmPassTaskWithDiamondsCommand.Codec, id: 377),
            Command(AdjustDiamondsServerCommand.Codec, id: 378),
            Command(SpendDiamondsServerCommand.Codec, id: 379),
            Command(BuyFarmPassLevelCommand.Codec, id: 380),
            Command(BuySeasonalCatalogueGiftCommand.Codec, id: 381),
            Command(RoadsideAdvertisementServerCommand.Codec, id: 382),
            Command(NeighborhoodObjectPointsServerCommand.Codec, id: 384),
            Command(VisitedBoatDepartureServerCommand.Codec, id: 385),
            Command(BoatCrateHelpedServerCommand.VisitedHomeCodec, id: 386),
            Command(BoatCrateHelpedServerCommand.OwnHomeCodec, id: 387),
            Command(VisitedBoatHelpRequestServerCommand.Codec, id: 388),
            Command(BoatCrateHelpResponseServerCommand.Codec, id: 389),
            Command(OpenHayDayWrappedCommand.Codec, id: 390),
            Command(CloseHayDayWrappedCommand.Codec, id: 391),
            Command(FarmPassProgressServerCommand.Codec, id: 392),
            Command(UpgradeMolluscBedCommand.Codec, id: 394),
            Command(UnlockProductionSlotCommand.Codec, id: 501),
            Command(MarkChronosEventUserInterfaceOpenedCommand.Codec, id: 502),
            Command(MarkInstagramAdClickedCommand.Codec, id: 503),
            Command(MarkFacebookAdClickedCommand.Codec, id: 505),
            Command(HarvestFieldCommand.Codec, id: 506),
            Command(ClaimDerbyPlacementRewardsCommand.Codec, id: 508),
            Command(RequestRoadsidePurchaseCommand.Codec, id: 509),
            Command(CollectMovieTicketRewardCommand.Codec, id: 510),
            Command(AdvertiseRoadsideListingCommand.Codec, id: 511),
            Command(PlayMiniPassFinaleCommand.Codec, id: 512),
            Command(RefreshFarmPassTasksCommand.Codec, id: 513),
            Command(PlantFieldCommand.Codec, id: 514),
            Command(ResetCustomizationSelectionCommand.Codec, id: 515),
            Command(CollectCalendarGiftCommand.Codec, id: 516),
            Command(ResetWheelCarCommand.Codec, id: 517),
            Command(CollectBuildingProductCommand.Codec, id: 518),
            Command(ReviveCountyFairStreakCommand.Codec, id: 519),
            Command(SelectDailyTaskCommand.Codec, id: 520),
            Command(StartBuildingMasteryJobCommand.Codec, id: 521),
            Command(MarkCountyFairOpenedCommand.Codec, id: 522),
            Command(ClaimDecoStickerBookRewardCommand.Codec, id: 523),
            Command(ClearNeighborhoodDonationNotificationCommand.Codec, id: 524),
            Command(PublishDecorationEventDesignCommand.Codec, id: 525),
            Command(MarkMapGameCompletedQuestsSeenCommand.Codec, id: 526),
            Command(MarkEditModeIntroSeenCommand.Codec, id: 527),
            Command(RequestLeaveMapGameCommand.Codec, id: 529),
            Command(AcknowledgePerformanceProfilingCommand.Codec, id: 530),
            Command(FeedLivestockAnimalCommand.Codec, id: 532),
            Command(DiscardBoatOrderCommand.Codec, id: 533),
            Command(ClaimEventBoardSeenRewardCommand.Codec, id: 534),
            Command(EnterBoyIntervalRestCommand.Codec, id: 537),
            Command(DismissVisitorGiftCommand.Codec, id: 538),
            Command(MarkFarmPassTasksSeenCommand.Codec, id: 539),
            Command(SelectWorkbenchPerkCommand.Codec, id: 540),
            Command(SpawnBalloonCommand.Codec, id: 541),
            Command(StartWorkbenchPerkBuildCommand.Codec, id: 543),
            Command(StartHarvestFieldCommand.Codec, id: 544),
            Command(BuyExtraNeighborhoodObjectTaskCommand.Codec, id: 547),
            Command(MarkDailyTasksSeenCommand.Codec, id: 548),
            Command(MarkEventTasksSeenCommand.Codec, id: 549),
            Command(SetEventCounterCommand.Codec, id: 550),
            Command(CollectDailyTaskYarnCommand.Codec, id: 551),
            Command(CompleteDecorationEventChallengeChainCommand.Codec, id: 552),
            Command(SpeedUpMapGameTaskCommand.Codec, id: 553),
            Command(ClaimBoyBoxCommand.Codec, id: 554),
            Command(PurchaseRoadsideAdvertisementCreditCommand.Codec, id: 555),
            Command(BuyFreshBeatCommand.Codec, id: 556),
            Command(BuyEventTaskSlotCommand.Codec, id: 558),
            Command(CollectVisitorFriendshipPointsCommand.Codec, id: 559),
            Command(SetMiniPassStateFlagsCommand.Codec, id: 560),
            Command(ClaimFarmPassLoopRewardCommand.Codec, id: 561),
            Command(OpenFarmPassBuildingCommand.Codec, id: 562),
            Command(BuyBuildingMasteryCommand.Codec, id: 563),
            Command(ClearMovieTicketShopNotificationsCommand.Codec, id: 564),
            Command(ToggleEventTypeLikeCommand.Codec, id: 565),
            Command(CollectSanctuaryVisitorGiftCommand.Codec, id: 566),
            Command(ClaimSurveyRewardCommand.Codec, id: 568),
            Command(FulfillFarmVisitorOrderCommand.Codec, id: 569),
            Command(SelectBoatOrderCommand.Codec, id: 570),
            Command(MarkDecorationVoteTooltipSeenCommand.Codec, id: 571),
            Command(MarkVouchersHintSeenCommand.Codec, id: 572),
            Command(CreateRoadsideListingCommand.Codec, id: 574),
            Command(EditDecorationEventDesignCommand.Codec, id: 575),
            Command(CompleteNeighborhoodObjectTaskCommand.Codec, id: 576),
            Command(ConstructGameObjectCommand.Codec, id: 577),
            Command(MarkMapGameCurrentQuestsSeenCommand.Codec, id: 578),
            Command(AssignWorkbenchPerkCommand.Codec, id: 579),
            Command(MarkTrackingConfirmationSeenCommand.Codec, id: 580),
            Command(RejectBoyOfferCommand.Codec, id: 583),
            Command(SetSupercellStorePromotionAllowedCommand.Codec, id: 585),
            Command(CollectAnimalProductCommand.Codec, id: 586),
            Command(StartWheelSpinCommand.Codec, id: 587),
            Command(FinishBuildingMasteryWithDiamondsCommand.Codec, id: 588),
            Command(CancelRoadsideListingCommand.Codec, id: 589),
            Command(CloseWheelCarCommand.Codec, id: 590),
            Command(OpenMiniPassChestCommand.Codec, id: 591),
            Command(MarkMapGameSunPointsSeenCommand.Codec, id: 592),
            Command(SubmitDecorationEventDesignCommand.Codec, id: 593),
            Command(RecordStorageSignpostRankCommand.Codec, id: 594),
            Command(WakeBoyFromRestCommand.Codec, id: 595),
            Command(MarkNeighborhoodTasksSeenCommand.Codec, id: 596),
            Command(MarkDecorationEventSeenCommand.Codec, id: 597),
            Command(OffloadMapGameAnimalsCommand.Codec, id: 598),
            Command(SaveDecorationEventCanvasCommand.Codec, id: 599),
            Command(MarkBoatSeenCommand.Codec, id: 600),
            Command(OpenDailyTaskChestCommand.Codec, id: 604),
            Command(ClaimDecoStickerBookPageRewardCommand.Codec, id: 605),
            Command(StartBuildingProductionCommand.Codec, id: 606),
            Command(DiscardCountyFairCrateCommand.Codec, id: 607),
            Command(StartVisitorQuestCommand.Codec, id: 608),
            Command(RerollDailyTasksCommand.Codec, id: 609),
            Command(ClaimDecisionBoxCommand.Codec, id: 610),
            Command(SwapNeighborhoodObjectTaskCommand.Codec, id: 611),
            Command(StartTruckDeliveryCommand.Codec, id: 612),
            Command(RevertDecorationEventDesignCommand.Codec, id: 613),
            Command(ClaimMapGameFuelPrizeCommand.Codec, id: 614),
            Command(ClearActiveDerbyTaskCommand.Codec, id: 615),
            Command(SpinMapGameFuelWheelCommand.Codec, id: 616),
            Command(GiveVisitorFriendshipGiftCommand.Codec, id: 617),
            Command(SetEventNotificationEnabledCommand.Codec, id: 618),
            Command(RerollAiRoadsideShopCommand.Codec, id: 622),
            Command(CollectNeighborhoodDonationCommand.Codec, id: 623),
            Command(MarkChainOfferSeenCommand.Codec, id: 624),
            Command(PinDecorationEventPhotoCommand.Codec, id: 625),
            Command(ClaimDecoStickerBookCollectionRewardCommand.Codec, id: 626),
            Command(ClaimDailyTaskPatchCommand.Codec, id: 628),
            Command(DismissVisitorOrderWithGiftCommand.Codec, id: 629),
            Command(RequestBoatOrderHelpCommand.Codec, id: 630),
            Command(UnlockRoadsideStandCommand.Codec, id: 631),
            Command(RefillKnitStreakCommand.Codec, id: 632),
            Command(ClaimMapGameEndRewardsCommand.Codec, id: 633),
            Command(FillBoatCrateCommand.Codec, id: 634),
            Command(MarkWeiboAdClickedCommand.Codec, id: 635),
            Command(SetNeighborhoodObjectLeaderboardScoresCommand.Codec, id: 636),
            Command(ClaimDecorationEventChallengesCommand.Codec, id: 637),
            Command(SpeedUpBoatCommand.Codec, id: 639),
            Command(PurchaseLivestockAnimalCommand.Codec, id: 641),
            Command(InstantCompleteProductionCommand.Codec, id: 642),
            Command(ActivateMovieTicketCommand.Codec, id: 643),
            Command(StartFarmPassSeasonCommand.Codec, id: 644),
            Command(ClaimFreeDiamondsCommand.Codec, id: 645),
            Command(ClaimMapGameDailyQuestsPrizeCommand.Codec, id: 647),
            Command(CollectRoadsideSaleProceedsCommand.Codec, id: 649),
            Command(SpeedUpCountyFairRegenerationCommand.Codec, id: 650),
            Command(MarkEventBoardOfferSeenCommand.Codec, id: 651),
            Command(MarkDecoStickerBookOpenedCommand.Codec, id: 652),
            Command(CompleteBoyInteractionCommand.Codec, id: 653),
            Command(DecorationEventTutorialCommand.Codec, id: 654),
            Command(ApplyFeatureUnlockRewardsCommand.Codec, id: 655),
            Command(UseCountyFairLuckyTicketCommand.Codec, id: 656),
            Command(HarvestFieldGainCommand.Codec, id: 657),
            Command(PlaceMasteryPlusWorkbenchCommand.Codec, id: 658),
            Command(BuyWheelSpinWithDiamondsCommand.Codec, id: 659),
            Command(CollectHelperAreaCommand.Codec, id: 660),
            Command(RequestNewspaperCommand.Codec, id: 661),
            Command(CompleteBoatOrderCommand.Codec, id: 662),
            Command(AdvanceBoatStateCommand.Codec, id: 663),
            Command(RestartKnitStreakCommand.Codec, id: 664),
            Command(BuyCropSeedsCommand.Codec, id: 665),
            Command(SkipDecorationEventChallengeCommand.Codec, id: 666),
            Command(OpenCalendarGiftBoxCommand.Codec, id: 667),
            Command(AddShopBadgesCommand.Codec, id: 686),
            Command(CatchCreatureCommand.Codec, id: 669),
            Command(PlaceEditModeObjectsCommand.Codec, id: 670),
            Command(RequestJoinMapGameCommand.Codec, id: 671),
            Command(CollectAllLettersCommand.Codec, id: 672),
            Command(AcknowledgeBoatCommand.Codec, id: 674),
            Command(CollectFruitCommand.Codec, id: 675),
            Command(SynchronizeServerTimeCommand.Codec, id: 676),
            Command(CollectTruckDeliveryRewardsCommand.Codec, id: 677),
            Command(CollectPostmanDeliveryCommand.Codec, id: 678),
            Command(ReportHayDayWrappedCommand.Codec, id: 679),
            Command(RefreshShopBadgesCommand.Codec, id: 681),
            Command(EnterEditModeCommand.Codec, id: 682),
            Command(MarkTermsOfServiceSeenCommand.Codec, id: 546),
            Command(AdvanceReengagementFlowCommand.Codec, id: 684),
            Command(CallLumberjackCommand.Codec, id: 685),
            Command(SubmitDecorationVoteCommand.Codec, id: 687),
            Command(ClaimChainOfferStepCommand.Codec, id: 691),
            Command(SetPlayerNameCommand.Codec, id: 692),
            Command(MarkFreshBeatsSeenCommand.Codec, id: 693),
            Command(PostmanStateCommand.Codec, id: 694),
            Command(MarkEmotesSeenCommand.Codec, id: 695),
            Command(BuyVisitorItemCommand.Codec, id: 696),
            Command(MarkTimeLimitedOfferSeenCommand.Codec, id: 697),
            Command(ResetDerbyRewardsCommand.Codec, id: 698),
            Command(StartBirthdayTasksCommand.Codec, id: 699),
            Command(CollectMolluscCommand.Codec, id: 393),
            Command(OpenMolluscCommand.Codec, id: 620),
            Command(ClaimNeighborhoodObjectRewardCommand.Codec, id: 627),
            Command(SettleBoatTrackPointsCommand.Codec, id: 688),
            Command(ClaimNeighborhoodObjectVisualRewardCommand.Codec, id: 602),
            Command(MarkFarmPassPointsSeenCommand.Codec, id: 528),
            Command(RemoveShopBadgesCommand.Codec, id: 601),
            Command(RespondToBoatTimeExtensionCommand.Codec, id: 673),
            Command(LoadFarmLayoutsServerCommand.Codec, id: 743),
            Command(DecorationVoteCandidatesServerCommand.Codec, id: 755),
            Command(DecorationEventDesignServerCommand.Codec, id: 771),
            Command(VisitedBoatStateServerCommand.Codec, id: 810),
            Command(ClaimMiniPassRewardCommand.Codec, id: 621),
            Command(SetChatMutedServerCommand.Codec, id: 839),
            Command(SkipPersonalTrainCooldownCommand.Codec, id: 507),
            Command(SpeedUpTrainCommand.Codec, id: 557),
            Command(RequestPersonalTrainDepartureCommand.Codec, id: 640),
            Command(ReceiveCircusTrainCommand.Codec, id: 536),
            Command(DispatchCircusTrainCommand.Codec, id: 582),
            Command(BuyMapGameFuelSpinCommand.Codec, id: 668),
            Command(RefreshBoatOrdersCommand.Codec, id: 683),
        ],
        Messages:
        [
            Message<AcceptFriendRequestMessage>(id: 17654),
            Message<AccountCandidatesMessage>(id: 24843, captureName: "Clientbound24843Message"),
            Message<AddFriendMessage>(id: 11539),
            Message<AmazonAccountAlreadyBoundMessage>(id: 21023),
            Message<AmazonAccountBoundMessage>(id: 20524),
            Message<AnalyticEventMessage>(id: 10906),
            Message<AnalyticsBatchMessage>(id: 38000),
            Message<AskForAddableFriendsMessage>(id: 14301),
            Message<AudioAnalyticsMessage>(id: 38200),
            Message<AvailableServerCommandMessage>(id: 20187),
            Message<AvatarDataMessage>(id: 22808),
            Message<AvatarInformationListRequestMessage>(id: 15281),
            Message<AvatarInformationListResponseMessage>(id: 27398),
            Message<AvatarProfileMessage>(id: 21108),
            Message<AvatarStreamEntryMessage>(id: 22018),
            Message<AvatarStreamEntryRemovedMessage>(id: 22477),
            Message<AvatarStreamMessage>(id: 24281),
            Message<BindAmazonAccountMessage>(id: 15756),
            Message<BindFacebookAccountMessage>(id: 11055),
            Message<BindGamecenterAccountMessage>(id: 19131),
            Message<BindGoogleServiceAccountMessage>(id: 13583),
            Message<BoatScoreLeaderboardMessage>(id: 23708),
            Message<BuyExtraDerbyTaskMessage>(id: 12056),
            Message<BuyExtraDerbyTaskResponseMessage>(id: 24089),
            Message<BuyFromNewspaperAdvertisementMessage>(id: 14799),
            Message<BuyFromStandResponseMessage>(id: 22878, captureName: "RoadsidePurchaseResultMessage"),
            Message<ChangeAccountSettingsMessage>(id: 12967),
            Message<ChangeAccountSettingsResponseMessage>(id: 26456),
            Message<ChangeHomeNameMessage>(id: 12816),
            Message<ChangeHomeNameResponseMessage>(id: 20111),
            Message<ChangeLayoutNamesMessage>(id: 17061),
            Message<ChangeNeighborhoodMemberRoleMessage>(id: 14249),
            Message<ChangeNeighborhoodMemberRoleResponseMessage>(id: 26910),
            Message<ChangeNeighborhoodSettingsFailedMessage>(id: 24569),
            Message<ChangeNeighborhoodSettingsMessage>(id: 11818),
            Message<ChatToNeighborhoodStreamMessage>(id: 18487),
            Message<ClaimAdvertisementRewardMessage>(id: 19698),
            Message<ClaimChickFarmPassAccessoryMessage>(id: 11259),
            Message<ClaimNeighborhoodGiftResultMessage>(id: 25713),
            Message<ClientCapabilitiesMessage>(id: 16679),
            Message<ClientCryptoErrorMessage>(id: 10099),
            Message<ClientDeviceInformationMessage>(id: 30000),
            Message<ClientHelloMessage>(id: 10100),
            Message<ClientScreenEventMessage>(id: 12576),
            Message<CollectAllLettersMessage>(id: 13024),
            Message<ConfirmAccountActionMessage>(id: 13477),
            Message<ConfirmLoginMessage>(id: 16627),
            Message<ConfirmPopupActionMessage>(id: 12591),
            Message<ConnectionStatusMessage>(id: 22890),
            Message<CreateNeighborhoodFailedMessage>(id: 24315),
            Message<CreateNeighborhoodMessage>(id: 10928),
            Message<DeclineFriendRequestMessage>(id: 17199),
            Message<DecorationCanvasDataMessage>(id: 28544),
            Message<DecorationDesignMessage>(id: 24838),
            Message<DerbyBoardMessage>(id: 23074, captureName: "Clientbound23074Message"),
            Message<DerbyRaceUpdateMessage>(id: 21436),
            Message<DerbyRankingsMessage>(id: 23227),
            Message<DerbyTaskLogMessage>(id: 23139),
            Message<DeviceLinkNoticeMessage>(id: 29612),
            Message<DeviceLinkStateMessage>(id: 25987),
            Message<DeviceLinkTextMessage>(id: 21984),
            Message<DiscardDerbyTaskMessage>(id: 19163),
            Message<DonateToNeighborhoodRequestMessage>(id: 18400),
            Message<EditPetNameMessage>(id: 14228),
            Message<EndClientTurnMessage>(id: 10224),
            Message<EventLeaderboardMessage>(id: 22903),
            Message<FacebookAccountAlreadyBoundMessage>(id: 29304),
            Message<FacebookAccountBoundConfirmedMessage>(id: 16167),
            Message<FacebookAccountBoundMessage>(id: 27376),
            Message<FarmLikeStatusMessage>(id: 26668, captureName: "Clientbound26668Message"),
            Message<FavouriteDecorationDesignMessage>(id: 16659),
            Message<FeaturingDesignListMessage>(id: 27413),
            Message<FishingDataMessage>(id: 24222),
            Message<FollowMessage>(id: 18335),
            Message<FollowResponseMessage>(id: 21236),
            Message<FollowerCountMessage>(id: 23455),
            Message<FollowerListPageMessage>(id: 26605),
            Message<FriendActionResultMessage>(id: 28665),
            Message<FriendCountMessage>(id: 25679),
            Message<FriendListMessage>(id: 29415),
            Message<FriendListUpdateMessage>(id: 26582),
            Message<FriendMetadataMessage>(id: 26199, captureName: "Clientbound26199Message"),
            Message<FriendRequestResultMessage>(id: 26412),
            Message<GameModeNoticeMessage>(id: 26257),
            Message<GamecenterAccountAlreadyBoundMessage>(id: 28503),
            Message<GamecenterAccountBoundConfirmedMessage>(id: 10958),
            Message<GamecenterAccountBoundMessage>(id: 29396),
            Message<GoogleServiceAccountAlreadyBoundMessage>(id: 24149),
            Message<GoogleServiceAccountBoundMessage>(id: 22302),
            Message<GregFarmDataMessage>(id: 20699),
            Message<GregTownDataMessage>(id: 24829),
            Message<HandleDeeplinkActionMessage>(id: 12681),
            Message<HelperListMessage>(id: 21628),
            Message<HideFacebookFriendMessage>(id: 13871),
            Message<HomeIntegerListsMessage>(id: 25133),
            Message<HomeLoadFailedMessage>(id: 21767),
            Message<InboxUnreadCountMessage>(id: 20155),
            Message<InviteFriendMessage>(id: 13346),
            Message<JoinNeighborhoodResponseMessage>(id: 25454),
            Message<KeepAliveMessage>(id: 10108),
            Message<KeepAliveOkMessage>(id: 20108),
            Message<KickNeighborhoodMemberMessage>(id: 15336),
            Message<LayoutNameListMessage>(id: 23098),
            Message<LeagueMemberListMessage>(id: 26542, captureName: "AvatarStreamPageMessage"),
            Message<LeaveNeighborhoodResponseMessage>(id: 20015),
            Message<LeftNeighborhoodMessage>(id: 23992),
            Message<LikeFarmMessage>(id: 15117),
            Message<LogicDeviceLinkCodeResponseMessage>(id: 28497),
            Message<LogicDeviceLinkDoneMessage>(id: 23877),
            Message<LogicDeviceLinkResponseMessage>(id: 24885),
            Message<LoginFailedMessage>(id: 20103),
            Message<LoginMessage>(id: 10101),
            Message<LoginOkMessage>(id: 25220),
            Message<MailEntryListMessage>(id: 21958),
            Message<MailListMessage>(id: 21915),
            Message<MarkNeighborhoodStreamEntriesSeenMessage>(id: 12192),
            Message<MarkStickerBookSeenMessage>(id: 13846),
            Message<MuteFriendMessage>(id: 12249),
            Message<MuteNeighborhoodMemberMessage>(id: 13044),
            Message<MuteNeighborhoodMemberResponseMessage>(id: 21194),
            Message<NeighborhoodActionResultMessage>(id: 21997),
            Message<NeighborhoodChatMessage>(id: 27910),
            Message<NeighborhoodComparisonMessage>(id: 28183),
            Message<NeighborhoodFullEntryMessage>(id: 29247),
            Message<NeighborhoodFullListMessage>(id: 29897),
            Message<NeighborhoodGiftStreamEntryMessage>(id: 28937),
            Message<NeighborhoodHelpEntriesMessage>(id: 28070),
            Message<NeighborhoodIdMessage>(id: 25864),
            Message<NeighborhoodListMessage>(id: 24260),
            Message<NeighborhoodListsMessage>(id: 20013),
            Message<NeighborhoodMemberListsMessage>(id: 21945),
            Message<NeighborhoodMemberMessage>(id: 21156),
            Message<NeighborhoodMembersMessage>(id: 28583),
            Message<NeighborhoodMutedMembersMessage>(id: 22802),
            Message<NeighborhoodNotificationStreamEntryMessage>(id: 26710),
            Message<NeighborhoodNotificationStreamEntryRemovedMessage>(id: 25159),
            Message<NeighborhoodNotificationStreamMessage>(id: 20621, captureName: "Clientbound20621Message"),
            Message<NeighborhoodObjectLeaderboardListMessage>(id: 23444, captureName: "PlayerRankingsPageMessage"),
            Message<NeighborhoodStreamEntryFlagMessage>(id: 23867),
            Message<NeighborhoodStreamMessage>(id: 28061, captureName: "Clientbound28061Message"),
            Message<NeighborhoodUpdatedMessage>(id: 24734),
            Message<NewspaperDataMessage>(id: 28967),
            Message<NotifyOnBoatHelpRequestedMessage>(id: 29551),
            Message<NotifyVisitorsOnBoatHelpReceivedMessage>(id: 26588),
            Message<OpenHamburgerMenuMessage>(id: 12809),
            Message<OtherFishingHomeDataMessage>(id: 28917),
            Message<OtherHomeDataMessage>(id: 24489),
            Message<OtherTrainStationDataMessage>(id: 21179),
            Message<OutOfSyncMessage>(id: 23626),
            Message<OwnHomeDataMessage>(id: 24180),
            Message<PerformanceProfilingSettingsMessage>(id: 26385),
            Message<PickUpPassengerResponseMessage>(id: 25372),
            Message<PlayerPermissionsMessage>(id: 29901),
            Message<PlayerRankingsMessage>(id: 23443),
            Message<PostNeighborhoodNotificationStreamMessage>(id: 18215),
            Message<PreviousEventStatusRequestMessage>(id: 18386),
            Message<PreviousEventStatusResponseMessage>(id: 27439),
            Message<ProductListMessage>(id: 27616),
            Message<ReactToStreamEntryMessage>(id: 13980),
            Message<ReactivateDerbyTaskMessage>(id: 12690),
            Message<ReceiptVerificationMessage>(id: 20200),
            Message<RecipientSelectedMessage>(id: 29926),
            Message<RecipientSettingsMessage>(id: 29734),
            Message<RecipientStatusListMessage>(id: 26994),
            Message<RecoverLostFarmMessage>(id: 12772),
            Message<ReloadGameRequestMessage>(id: 29997),
            Message<RemoveStreamEntryReactionMessage>(id: 14481),
            Message<ReportAdvertisementShownMessage>(id: 12255),
            Message<ReportAdvertisementStartedMessage>(id: 10425),
            Message<ReportAdvertisementWatchedMessage>(id: 16273),
            Message<ReportDecorationDesignMessage>(id: 19451),
            Message<ReportEventSeenMessage>(id: 19173),
            Message<ReportGameModeEnteredMessage>(id: 14060),
            Message<ReportLoginCountryMessage>(id: 12703),
            Message<ReportNeighborhoodMemberMessage>(id: 17494),
            Message<ReportStoreLinkOutAvatarMessage>(id: 18766),
            Message<ReportStoreLinkOutMessage>(id: 15590),
            Message<ReportSupportTicketMessage>(id: 14213),
            Message<RequestAllianceDataMessage>(id: 10865),
            Message<RequestAvatarProfileMessage>(id: 12127),
            Message<RequestAvatarStreamMessage>(id: 14779),
            Message<RequestBaseAvatarDataMessage>(id: 13701),
            Message<RequestDailyRewardsMessage>(id: 14942),
            Message<RequestDataTableUpdateMessage>(id: 14902),
            Message<RequestDataTablesMessage>(id: 14903),
            Message<RequestDecorationEventPhaseMessage>(id: 19147),
            Message<RequestDerbyBoardMessage>(id: 16886),
            Message<RequestDerbyRaceMessage>(id: 15980),
            Message<RequestDerbyRankingsMessage>(id: 15762),
            Message<RequestDerbyTaskLogMessage>(id: 13511),
            Message<RequestEventLeaderboardMessage>(id: 10301),
            Message<RequestEventRewardsMessage>(id: 11841),
            Message<RequestEventStatusMessage>(id: 17211),
            Message<RequestFarmLikeStatusMessage>(id: 16037),
            Message<RequestFollowedNeighborhoodsMessage>(id: 17757),
            Message<RequestFollowerCountMessage>(id: 18272),
            Message<RequestFollowerListPageMessage>(id: 19845),
            Message<RequestFriendBookMessage>(id: 14274),
            Message<RequestFriendListMessage>(id: 10518),
            Message<RequestGameModeDataMessage>(id: 14050),
            Message<RequestGregFarmMessage>(id: 14038),
            Message<RequestGregTownMessage>(id: 10392),
            Message<RequestHelpersMessage>(id: 14972),
            Message<RequestHomeIntegerListsMessage>(id: 10550),
            Message<RequestJoinNeighborhoodMessage>(id: 17418),
            Message<RequestLeaderboardMessage>(id: 19064),
            Message<RequestLeagueMessage>(id: 12862),
            Message<RequestNeighborhoodChatHistoryMessage>(id: 14428),
            Message<RequestNeighborhoodComparisonMessage>(id: 12069),
            Message<RequestNeighborhoodDerbyComparisonMessage>(id: 14014),
            Message<RequestNeighborhoodEventBoardMessage>(id: 13392),
            Message<RequestNeighborhoodEventsMessage>(id: 13853),
            Message<RequestNeighborhoodMembersMessage>(id: 11393),
            Message<RequestNeighborhoodObjectLeaderboardMessage>(id: 10254),
            Message<RequestNeighborhoodObjectRewardsMessage>(id: 19065),
            Message<RequestNeighborhoodTradingMessage>(id: 17805),
            Message<RequestNeighborhoodTradingStatsMessage>(id: 11859),
            Message<RequestNotificationSettingsMessage>(id: 15767),
            Message<RequestOtherAvatarMessage>(id: 17276),
            Message<RequestOtherTownMessage>(id: 18919),
            Message<RequestOwnFishingHomeMessage>(id: 18475),
            Message<RequestOwnHomeMessage>(id: 19949),
            Message<RequestOwnTownMessage>(id: 14049),
            Message<RequestPetNameValidationMessage>(id: 17310),
            Message<RequestScoreboardMessage>(id: 12840),
            Message<RequestSeasonalCatalogueMessage>(id: 15996),
            Message<RequestSupercellIdFriendsMessage>(id: 19172),
            Message<RequestTaskEventLeaderboardMessage>(id: 19602),
            Message<RequestTownLeaderboardMessage>(id: 12833),
            Message<RequestTownPassengerPickupMessage>(id: 19430),
            Message<RequestUserDefaultsMessage>(id: 14437),
            Message<RequestWebViewTokenMessage>(id: 16254),
            Message<RespondToFriendRequestMessage>(id: 13942),
            Message<RoadsideBuyerMessage>(id: 22158),
            Message<RoadsideListingBuyerMessage>(id: 28562),
            Message<RoadsideShopSlotUpdatedMessage>(id: 20349),
            Message<SaveEditModeLayoutMessage>(id: 10943),
            Message<ScidJwtMessage>(id: 29275),
            Message<ScratchBonusRoundRewardMessage>(id: 15918),
            Message<SearchNeighborhoodAdvancedMessage>(id: 19730),
            Message<SearchNeighborhoodMessage>(id: 12533),
            Message<SecureConnectionOkMessage>(id: 22357),
            Message<SelectEditModeSlotMessage>(id: 16891),
            Message<SelectRecipientMessage>(id: 19658),
            Message<SendNeighborhoodEmoteMessage>(id: 18812),
            Message<HelpBoatCrateMessage>(id: 14182),
            Message<ServerHelloMessage>(id: 20100),
            Message<SetNotificationSettingsMessage>(id: 16697),
            Message<SetSupercellIdFriendsMessage>(id: 19361),
            Message<ShareFishingSpotStatsMessage>(id: 13279),
            Message<ShowPopupMessage>(id: 25707),
            Message<ShutdownStartedMessage>(id: 25373),
            Message<SocialProfileMessage>(id: 28302),
            Message<SoilAdsInteractionAnalyticsMessage>(id: 38102),
            Message<SoilInformationClosedAnalyticsMessage>(id: 38104),
            Message<SoilIntroTutorialAnalyticsMessage>(id: 38103),
            Message<SoilUserInterfaceUsedAnalyticsMessage>(id: 38101),
            Message<SpeedUpDerbyBoardTaskMessage>(id: 19071),
            Message<SpeedUpDerbyBoardTaskResponseMessage>(id: 26106),
            Message<SpellFactoryVoucherClaimFailedMessage>(id: 28201),
            Message<StreamEntryReactionAddedMessage>(id: 22338),
            Message<StreamEntryReactionCountMessage>(id: 25374),
            Message<StreamEntryReactionMessage>(id: 27340),
            Message<StreamEntryReactionRemovedMessage>(id: 28247),
            Message<SupercellIdAccountAlreadyBoundMessage>(id: 22542),
            Message<SupercellIdAccountBoundMessage>(id: 22580),
            Message<SupercellIdClaimItemResultMessage>(id: 23292),
            Message<SupercellIdLoginMessage>(id: 18755),
            Message<SupercellIdLogoutAllDevicesResultMessage>(id: 24840),
            Message<SupercellIdNoticeMessage>(id: 24965),
            Message<SupercellIdNotificationTokenMessage>(id: 29900),
            Message<SurveyEventMessage>(id: 39005),
            Message<TakeDerbyTaskMessage>(id: 19110),
            Message<TextEntryListMessage>(id: 24719),
            Message<TitanDisconnectedMessage>(id: 25892),
            Message<TownPassengerStateMessage>(id: 21348),
            Message<TrackingMessage>(id: 15276),
            Message<TrainStationDataMessage>(id: 28543),
            Message<TrashDerbyBoardTaskMessage>(id: 18447),
            Message<UnblockFriendMessage>(id: 10658),
            Message<UnfollowFriendMessage>(id: 13775),
            Message<UnfollowMessage>(id: 14664),
            Message<UnhideFacebookFriendMessage>(id: 15768),
            Message<UnlockAccountMessage>(id: 10474),
            Message<UnlockAccountOkMessage>(id: 28078),
            Message<UploadClientDebugDataMessage>(id: 17339),
            Message<UploadClientLogMessage>(id: 17338),
            Message<UploadDecorationDesignMessage>(id: 17027),
            Message<VerifyAmazonBillingReceiptMessage>(id: 16128),
            Message<VerifyBillingReceiptMessage>(id: 17738),
            Message<VerifyGoogleBillingReceiptMessage>(id: 15502),
            Message<VerifySupercellIdBillingReceiptMessage>(id: 10614),
            Message<VisitHomeMessage>(id: 14484),
            Message<VisitHomeTargetMessage>(id: 18671),
            Message<VisitOtherFishingHomeMessage>(id: 17703),
            Message<VoteForDecorationDesignMessage>(id: 15262),
        ]);
}
