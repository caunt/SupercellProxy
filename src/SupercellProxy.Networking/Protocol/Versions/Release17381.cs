using SupercellProxy.Networking.Protocol.Advertisements;
using SupercellProxy.Networking.Protocol.Animals;
using SupercellProxy.Networking.Protocol.Avatars.Names;
using SupercellProxy.Networking.Protocol.Balloons;
using SupercellProxy.Networking.Protocol.Boats;
using SupercellProxy.Networking.Protocol.Boats.Orders;
using SupercellProxy.Networking.Protocol.Boats.States;
using SupercellProxy.Networking.Protocol.Boosters.FreshBeats;
using SupercellProxy.Networking.Protocol.Characters;
using SupercellProxy.Networking.Protocol.ConnectionControl;
using SupercellProxy.Networking.Protocol.Consumables;
using SupercellProxy.Networking.Protocol.Creatures;
using SupercellProxy.Networking.Protocol.CropFields;
using SupercellProxy.Networking.Protocol.Currency;
using SupercellProxy.Networking.Protocol.Customization;
using SupercellProxy.Networking.Protocol.DailyTasks;
using SupercellProxy.Networking.Protocol.DecorationEvents;
using SupercellProxy.Networking.Protocol.Decorations;
using SupercellProxy.Networking.Protocol.Emotes;
using SupercellProxy.Networking.Protocol.Events.Board;
using SupercellProxy.Networking.Protocol.Events.Boards;
using SupercellProxy.Networking.Protocol.Events.Calendar;
using SupercellProxy.Networking.Protocol.Events.ChainOffers;
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
using SupercellProxy.Networking.Protocol.Events.Tasks;
using SupercellProxy.Networking.Protocol.Events.Wrapped;
using SupercellProxy.Networking.Protocol.FarmLayouts;
using SupercellProxy.Networking.Protocol.FarmLayouts.Names;
using SupercellProxy.Networking.Protocol.FarmPass.Rewards;
using SupercellProxy.Networking.Protocol.FarmPass.Season;
using SupercellProxy.Networking.Protocol.FarmPass.Seen;
using SupercellProxy.Networking.Protocol.FarmPass.Tasks;
using SupercellProxy.Networking.Protocol.Fishing.Angus;
using SupercellProxy.Networking.Protocol.Fishing.Molluscs;
using SupercellProxy.Networking.Protocol.Forestry;
using SupercellProxy.Networking.Protocol.GameObjects;
using SupercellProxy.Networking.Protocol.Gifts;
using SupercellProxy.Networking.Protocol.Helpers;
using SupercellProxy.Networking.Protocol.Helpers.Boy;
using SupercellProxy.Networking.Protocol.Homes;
using SupercellProxy.Networking.Protocol.Homes.Details;
using SupercellProxy.Networking.Protocol.Inventory.Shop;
using SupercellProxy.Networking.Protocol.Mail;
using SupercellProxy.Networking.Protocol.MapGame.Animals;
using SupercellProxy.Networking.Protocol.MapGame.Membership;
using SupercellProxy.Networking.Protocol.MapGame.Seen;
using SupercellProxy.Networking.Protocol.MapGame.Tasks;
using SupercellProxy.Networking.Protocol.MiniPass;
using SupercellProxy.Networking.Protocol.MovieTickets;
using SupercellProxy.Networking.Protocol.Neighborhoods;
using SupercellProxy.Networking.Protocol.Neighborhoods.Chat;
using SupercellProxy.Networking.Protocol.Neighborhoods.Chat.Reactions;
using SupercellProxy.Networking.Protocol.Neighborhoods.Objects;
using SupercellProxy.Networking.Protocol.Neighborhoods.Requests;
using SupercellProxy.Networking.Protocol.Neighborhoods.Rewards;
using SupercellProxy.Networking.Protocol.Neighborhoods.Tasks;
using SupercellProxy.Networking.Protocol.Newspapers;
using SupercellProxy.Networking.Protocol.Offers;
using SupercellProxy.Networking.Protocol.Orders;
using SupercellProxy.Networking.Protocol.PrizeWheels;
using SupercellProxy.Networking.Protocol.Production;
using SupercellProxy.Networking.Protocol.Production.Mastery;
using SupercellProxy.Networking.Protocol.Progression;
using SupercellProxy.Networking.Protocol.Rankings;
using SupercellProxy.Networking.Protocol.Reengagement;
using SupercellProxy.Networking.Protocol.RoadsideShops;
using SupercellProxy.Networking.Protocol.RoadsideShops.AiShop;
using SupercellProxy.Networking.Protocol.RoadsideShops.Stands;
using SupercellProxy.Networking.Protocol.Sanctuary;
using SupercellProxy.Networking.Protocol.Screens;
using SupercellProxy.Networking.Protocol.Settings;
using SupercellProxy.Networking.Protocol.Settings.Ads;
using SupercellProxy.Networking.Protocol.Settings.Preferences;
using SupercellProxy.Networking.Protocol.ShopEvents;
using SupercellProxy.Networking.Protocol.Surveys;
using SupercellProxy.Networking.Protocol.Telemetry;
using SupercellProxy.Networking.Protocol.Timing;
using SupercellProxy.Networking.Protocol.Town.Circus;
using SupercellProxy.Networking.Protocol.Town.Trains;
using SupercellProxy.Networking.Protocol.Turns;

using static SupercellProxy.Networking.Protocol.Versions.GameRelease;
using SupercellProxy.Networking.Protocol.Visitors;
using SupercellProxy.Networking.Protocol.Visitors.Friendship;
using SupercellProxy.Networking.Protocol.Visitors.Orders;
using SupercellProxy.Networking.Protocol.Visitors.Quests;
using SupercellProxy.Networking.Protocol.Workbench;

namespace SupercellProxy.Networking.Protocol.Versions;

/// <summary>Hay Day 1.73.81: renumbered commands and messages, and the ones it introduced.</summary>
internal static class Release17381
{
    /// <summary>Gets the release version.</summary>
    public static Version Version { get; } = new(major: 1, minor: 73, build: 81);

    /// <summary>Gets the release and its id table.</summary>
    internal static GameRelease Release => field ??= new GameRelease(Version,
        Commands:
        [
            Command(SpeedUpMolluscCommand.Codec, id: 395),
            Command(FarmPassPurchaseServerCommand.Codec, id: 396),
            Command(UnlockProductionSlotCommand.Codec, id: 575),
            Command(MarkChronosEventUserInterfaceOpenedCommand.Codec, id: 514),
            Command(MarkInstagramAdClickedCommand.Codec, id: 691),
            Command(MarkFacebookAdClickedCommand.Codec, id: 539),
            Command(HarvestFieldCommand.Codec, id: 572),
            Command(RequestRoadsidePurchaseCommand.Codec, id: 512),
            Command(CollectMovieTicketRewardCommand.Codec, id: 550),
            Command(AdvertiseRoadsideListingCommand.Codec, id: 648),
            Command(PlayMiniPassFinaleCommand.Codec, id: 553),
            Command(RefreshFarmPassTasksCommand.Codec, id: 641),
            Command(PlantFieldCommand.Codec, id: 565),
            Command(ResetCustomizationSelectionCommand.Codec, id: 562),
            Command(CollectCalendarGiftCommand.Codec, id: 645),
            Command(ResetWheelCarCommand.Codec, id: 686),
            Command(CollectBuildingProductCommand.Codec, id: 534),
            Command(ReviveCountyFairStreakCommand.Codec, id: 547),
            Command(SelectDailyTaskCommand.Codec, id: 606),
            Command(StartBuildingMasteryJobCommand.Codec, id: 532),
            Command(MarkCountyFairOpenedCommand.Codec, id: 646),
            Command(ClaimDecoStickerBookRewardCommand.Codec, id: 647),
            Command(ClearNeighborhoodDonationNotificationCommand.Codec, id: 653),
            Command(PublishDecorationEventDesignCommand.Codec, id: 687),
            Command(MarkMapGameCompletedQuestsSeenCommand.Codec, id: 614),
            Command(MarkEditModeIntroSeenCommand.Codec, id: 633),
            Command(RequestLeaveMapGameCommand.Codec, id: 583),
            Command(AcknowledgePerformanceProfilingCommand.Codec, id: 622),
            Command(FeedLivestockAnimalCommand.Codec, id: 521),
            Command(DiscardBoatOrderCommand.Codec, id: 652),
            Command(ClaimEventBoardSeenRewardCommand.Codec, id: 678),
            Command(EnterBoyIntervalRestCommand.Codec, id: 667),
            Command(DismissVisitorGiftCommand.Codec, id: 629),
            Command(MarkFarmPassTasksSeenCommand.Codec, id: 527),
            Command(SelectWorkbenchPerkCommand.Codec, id: 505),
            Command(SpawnBalloonCommand.Codec, id: 522),
            Command(StartWorkbenchPerkBuildCommand.Codec, id: 509),
            Command(StartHarvestFieldCommand.Codec, id: 696),
            Command(UseConsumableCommand.Codec, id: 546),
            Command(BuyExtraNeighborhoodObjectTaskCommand.Codec, id: 672),
            Command(MarkDailyTasksSeenCommand.Codec, id: 515),
            Command(MarkEventTasksSeenCommand.Codec, id: 634),
            Command(SetEventCounterCommand.Codec, id: 603),
            Command(CollectDailyTaskYarnCommand.Codec, id: 602),
            Command(CompleteDecorationEventChallengeChainCommand.Codec, id: 663),
            Command(SpeedUpMapGameTaskCommand.Codec, id: 673),
            Command(ClaimBoyBoxCommand.Codec, id: 632),
            Command(PurchaseRoadsideAdvertisementCreditCommand.Codec, id: 561),
            Command(BuyFreshBeatCommand.Codec, id: 516),
            Command(BuyEventTaskSlotCommand.Codec, id: 636),
            Command(CollectVisitorFriendshipPointsCommand.Codec, id: 675),
            Command(SetMiniPassStateFlagsCommand.Codec, id: 654),
            Command(ClaimFarmPassLoopRewardCommand.Codec, id: 549),
            Command(OpenFarmPassBuildingCommand.Codec, id: 695),
            Command(BuyBuildingMasteryCommand.Codec, id: 639),
            Command(ClearMovieTicketShopNotificationsCommand.Codec, id: 605),
            Command(ToggleEventTypeLikeCommand.Codec, id: 526),
            Command(CollectSanctuaryVisitorGiftCommand.Codec, id: 586),
            Command(ClaimSurveyRewardCommand.Codec, id: 642),
            Command(FulfillFarmVisitorOrderCommand.Codec, id: 610),
            Command(SelectBoatOrderCommand.Codec, id: 585),
            Command(MarkDecorationVoteTooltipSeenCommand.Codec, id: 567),
            Command(MarkVouchersHintSeenCommand.Codec, id: 607),
            Command(CreateRoadsideListingCommand.Codec, id: 533),
            Command(EditDecorationEventDesignCommand.Codec, id: 689),
            Command(CompleteNeighborhoodObjectTaskCommand.Codec, id: 638),
            Command(ConstructGameObjectCommand.Codec, id: 574),
            Command(MarkMapGameCurrentQuestsSeenCommand.Codec, id: 625),
            Command(AssignWorkbenchPerkCommand.Codec, id: 587),
            Command(MarkTrackingConfirmationSeenCommand.Codec, id: 692),
            Command(RejectBoyOfferCommand.Codec, id: 657),
            Command(SetSupercellStorePromotionAllowedCommand.Codec, id: 569),
            Command(CollectAnimalProductCommand.Codec, id: 518),
            Command(StartWheelSpinCommand.Codec, id: 670),
            Command(FinishBuildingMasteryWithDiamondsCommand.Codec, id: 640),
            Command(CancelRoadsideListingCommand.Codec, id: 592),
            Command(CloseWheelCarCommand.Codec, id: 680),
            Command(OpenMiniPassChestCommand.Codec, id: 676),
            Command(MarkMapGameSunPointsSeenCommand.Codec, id: 517),
            Command(SubmitDecorationEventDesignCommand.Codec, id: 613),
            Command(RecordStorageSignpostRankCommand.Codec, id: 682),
            Command(WakeBoyFromRestCommand.Codec, id: 519),
            Command(MarkNeighborhoodTasksSeenCommand.Codec, id: 554),
            Command(MarkDecorationEventSeenCommand.Codec, id: 601),
            Command(OffloadMapGameAnimalsCommand.Codec, id: 541),
            Command(SaveDecorationEventCanvasCommand.Codec, id: 524),
            Command(MarkBoatSeenCommand.Codec, id: 560),
            Command(OpenDailyTaskChestCommand.Codec, id: 579),
            Command(ClaimDecoStickerBookPageRewardCommand.Codec, id: 609),
            Command(StartBuildingProductionCommand.Codec, id: 664),
            Command(DiscardCountyFairCrateCommand.Codec, id: 557),
            Command(StartVisitorQuestCommand.Codec, id: 580),
            Command(RerollDailyTasksCommand.Codec, id: 581),
            Command(ClaimDecisionBoxCommand.Codec, id: 577),
            Command(SwapNeighborhoodObjectTaskCommand.Codec, id: 503),
            Command(StartTruckDeliveryCommand.Codec, id: 617),
            Command(RevertDecorationEventDesignCommand.Codec, id: 674),
            Command(ClaimMapGameFuelPrizeCommand.Codec, id: 698),
            Command(ClearActiveDerbyTaskCommand.Codec, id: 536),
            Command(SpinMapGameFuelWheelCommand.Codec, id: 644),
            Command(GiveVisitorFriendshipGiftCommand.Codec, id: 684),
            Command(SetEventNotificationEnabledCommand.Codec, id: 623),
            Command(RerollAiRoadsideShopCommand.Codec, id: 593),
            Command(CollectNeighborhoodDonationCommand.Codec, id: 616),
            Command(MarkChainOfferSeenCommand.Codec, id: 628),
            Command(PinDecorationEventPhotoCommand.Codec, id: 681),
            Command(ClaimDecoStickerBookCollectionRewardCommand.Codec, id: 693),
            Command(ClaimDailyTaskPatchCommand.Codec, id: 507),
            Command(DismissVisitorOrderWithGiftCommand.Codec, id: 529),
            Command(RequestBoatOrderHelpCommand.Codec, id: 669),
            Command(UnlockRoadsideStandCommand.Codec, id: 513),
            Command(RefillKnitStreakCommand.Codec, id: 564),
            Command(ClaimMapGameEndRewardsCommand.Codec, id: 649),
            Command(FillBoatCrateCommand.Codec, id: 551),
            Command(MarkWeiboAdClickedCommand.Codec, id: 540),
            Command(SetNeighborhoodObjectLeaderboardScoresCommand.Codec, id: 679),
            Command(SpeedUpBoatCommand.Codec, id: 688),
            Command(PurchaseLivestockAnimalCommand.Codec, id: 530),
            Command(InstantCompleteProductionCommand.Codec, id: 615),
            Command(ActivateMovieTicketCommand.Codec, id: 604),
            Command(StartFarmPassSeasonCommand.Codec, id: 520),
            Command(ClaimFreeDiamondsCommand.Codec, id: 584),
            Command(ClaimMapGameDailyQuestsPrizeCommand.Codec, id: 566),
            Command(CollectRoadsideSaleProceedsCommand.Codec, id: 662),
            Command(SpeedUpCountyFairRegenerationCommand.Codec, id: 510),
            Command(MarkEventBoardOfferSeenCommand.Codec, id: 618),
            Command(MarkDecoStickerBookOpenedCommand.Codec, id: 595),
            Command(CompleteBoyInteractionCommand.Codec, id: 523),
            Command(DecorationEventTutorialCommand.Codec, id: 544),
            Command(ApplyFeatureUnlockRewardsCommand.Codec, id: 619),
            Command(UseCountyFairLuckyTicketCommand.Codec, id: 578),
            Command(HarvestFieldGainCommand.Codec, id: 589),
            Command(PlaceMasteryPlusWorkbenchCommand.Codec, id: 570),
            Command(BuyWheelSpinWithDiamondsCommand.Codec, id: 651),
            Command(CollectHelperAreaCommand.Codec, id: 548),
            Command(RequestNewspaperCommand.Codec, id: 608),
            Command(CompleteBoatOrderCommand.Codec, id: 590),
            Command(AdvanceBoatStateCommand.Codec, id: 588),
            Command(RestartKnitStreakCommand.Codec, id: 556),
            Command(BuyCropSeedsCommand.Codec, id: 542),
            Command(SkipDecorationEventChallengeCommand.Codec, id: 620),
            Command(OpenCalendarGiftBoxCommand.Codec, id: 630),
            Command(AddShopBadgesCommand.Codec, id: 668),
            Command(CatchCreatureCommand.Codec, id: 563),
            Command(PlaceEditModeObjectsCommand.Codec, id: 697),
            Command(RequestJoinMapGameCommand.Codec, id: 552),
            Command(CollectAllLettersCommand.Codec, id: 501),
            Command(AcknowledgeBoatCommand.Codec, id: 600),
            Command(CollectFruitCommand.Codec, id: 511),
            Command(SynchronizeServerTimeCommand.Codec, id: 543),
            Command(CollectTruckDeliveryRewardsCommand.Codec, id: 528),
            Command(CollectPostmanDeliveryCommand.Codec, id: 643),
            Command(ReportHayDayWrappedCommand.Codec, id: 635),
            Command(RefreshShopBadgesCommand.Codec, id: 690),
            Command(EnterEditModeCommand.Codec, id: 573),
            Command(MarkTermsOfServiceSeenCommand.Codec, id: 683),
            Command(AdvanceReengagementFlowCommand.Codec, id: 555),
            Command(CallLumberjackCommand.Codec, id: 594),
            Command(SubmitDecorationVoteCommand.Codec, id: 559),
            Command(ClaimChainOfferStepCommand.Codec, id: 671),
            Command(SetPlayerNameCommand.Codec, id: 597),
            Command(MarkFreshBeatsSeenCommand.Codec, id: 621),
            Command(PostmanStateCommand.Codec, id: 535),
            Command(MarkEmotesSeenCommand.Codec, id: 611),
            Command(BuyVisitorItemCommand.Codec, id: 659),
            Command(MarkTimeLimitedOfferSeenCommand.Codec, id: 666),
            Command(ResetDerbyRewardsCommand.Codec, id: 538),
            Command(SetAngusInteractionCommand.Codec, id: 582),
            Command(CollectAngusPearlsCommand.Codec, id: 545),
            Command(MarkBoatTrackCycleIntroSeenCommand.Codec, id: 627),
            Command(OpenMolluscCommand.Codec, id: 677),
            Command(AcknowledgeChainOfferChapterIntroCommand.Codec, id: 656),
            Command(ClaimNeighborhoodObjectRewardCommand.Codec, id: 504),
            Command(SettleBoatTrackPointsCommand.Codec, id: 576),
            Command(ClaimNeighborhoodObjectVisualRewardCommand.Codec, id: 665),
            Command(AcknowledgeEventSequenceCommand.Codec, id: 599),
            Command(MarkFarmPassPointsSeenCommand.Codec, id: 506),
            Command(RemoveShopBadgesCommand.Codec, id: 658),
            Command(RespondToBoatTimeExtensionCommand.Codec, id: 650),
            Command(LoadFarmLayoutsServerCommand.Codec, id: 781),
            Command(DecorationVoteCandidatesServerCommand.Codec, id: 829),
            Command(DecorationEventDesignServerCommand.Codec, id: 840),
            Command(VisitedBoatStateServerCommand.Codec, id: 826),
            Command(ClaimMiniPassRewardCommand.Codec, id: 568),
            Command(SetChatMutedServerCommand.Codec, id: 784),
            Command(SkipPersonalTrainCooldownCommand.Codec, id: 525),
            Command(SpeedUpTrainCommand.Codec, id: 531),
            Command(RequestPersonalTrainDepartureCommand.Codec, id: 631),
            Command(ReceiveCircusTrainCommand.Codec, id: 660),
            Command(DispatchCircusTrainCommand.Codec, id: 598),
            Command(FeedAngusCommand.Codec, id: 502),
            Command(MarkBoatDestinationTutorialSeenCommand.Codec, id: 558),
            Command(BuyMapGameFuelSpinCommand.Codec, id: 591),
            Command(ClaimVisitorFriendshipRewardCommand.Codec, id: 596),
            Command(MarkFarmPassIntroSeenCommand.Codec, id: 612),
            Command(ApplyConsumableToFieldCommand.Codec, id: 626),
            Command(MarkBuildingMasteryExplainerSeenCommand.Codec, id: 655),
            Command(RefreshBoatOrdersCommand.Codec, id: 661),
            Command(RefreshTruckOrderCommand.Codec, id: 694),
        ],
        Messages:
        [
            Message<BuyFromNewspaperAdvertisementMessage>(id: 13492),
            Message<ClaimAdvertisementRewardMessage>(id: 18607),
            Message<ClientScreenEventMessage>(id: 14322),
            Message<ConfirmPopupActionMessage>(id: 19948),
            Message<DecorationDesignMessage>(id: 21062),
            Message<EndClientTurnMessage>(id: 11657),
            Message<FavouriteDecorationDesignMessage>(id: 13257),
            Message<FeaturingDesignListMessage>(id: 25108),
            Message<GameModeNoticeMessage>(id: 22924),
            Message<HomeIntegerListsMessage>(id: 27850),
            Message<MarkStickerBookSeenMessage>(id: 18518),
            Message<OpenHamburgerMenuMessage>(id: 14059),
            Message<PerformanceProfilingSettingsMessage>(id: 22324),
            Message<ReactToStreamEntryMessage>(id: 11201),
            Message<RemoveStreamEntryReactionMessage>(id: 11079),
            Message<ReportAdvertisementRevenueMessage>(id: 14037),
            Message<ReportAdvertisementStartedMessage>(id: 12475),
            Message<ReportAdvertisementWatchedMessage>(id: 10860),
            Message<ReportEventSeenMessage>(id: 17151),
            Message<ReportGameModeEnteredMessage>(id: 14688),
            Message<RequestDecorationEventPhaseMessage>(id: 18521),
            Message<RequestEventLeaderboardMessage>(id: 11165),
            Message<RequestHomeIntegerListsMessage>(id: 14598),
            Message<SaveEditModeLayoutMessage>(id: 19805),
            Message<SendNeighborhoodEmoteMessage>(id: 10370),
            Message<SetNotificationSettingsMessage>(id: 18452),
            Message<StreamEntryReactionAddedMessage>(id: 23442),
            Message<StreamEntryReactionCountMessage>(id: 26507),
            Message<StreamEntryReactionMessage>(id: 29506),
            Message<StreamEntryReactionRemovedMessage>(id: 24727),
            Message<UploadDecorationDesignMessage>(id: 15244),
            RemovedMessage<VisitHomeMessage>(),
            Message<VoteForDecorationDesignMessage>(id: 13900),
        ]);
}
