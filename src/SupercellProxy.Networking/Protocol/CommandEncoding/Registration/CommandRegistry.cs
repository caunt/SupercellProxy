using System.Globalization;

using SupercellProxy.Networking.Protocol.CommandEncoding.FieldSchemas;
using SupercellProxy.Networking.Protocol.MapGame.Tasks;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

/// <summary>Maps native command ids to wire contracts and validates their registered encoding schemas.</summary>
public static partial class CommandRegistry
{
    /// <summary>
    /// Provides the Acknowledge Boat Command Type value or operation.
    /// </summary>
    public const int AcknowledgeBoatCommandType = 674;

    /// <summary>
    /// Acknowledges receipt of the platform profiling settings without changing farm state.
    /// </summary>
    public const int AcknowledgePerformanceProfilingCommandType = 530;

    /// <summary>Activates a booster held in the player's booster storage.</summary>
    public const int ActivateBoosterCommandType = 212;
    /// <summary>Provides the Activate Farm Pass Perk Command Type.</summary>
    public const int ActivateFarmPassPerkCommandType = 343;
    /// <summary>
    /// Provides the Activate Movie Ticket Command Type value or operation.
    /// </summary>
    public const int ActivateMovieTicketCommandType = 643;

    /// <summary>Adds new-item badges to the shop.</summary>
    public const int AddShopBadgesCommandType = 668;
    /// <summary>
    /// Provides the Advance Boat State Command Type value or operation.
    /// </summary>
    public const int AdvanceBoatStateCommandType = 663;
    /// <summary>
    /// Provides the Advance Reengagement Flow Command Type value or operation.
    /// </summary>
    public const int AdvanceReengagementFlowCommandType = 684;
    /// <summary>Advertises an existing roadside listing.</summary>
    public const int AdvertiseRoadsideListingCommandType = 511;
    /// <summary>
    /// Provides the Base Event Scene Command Type value or operation.
    /// </summary>
    public const int BaseEventSceneCommandType = 637;
    /// <summary>Reports a boat help request result.</summary>
    public const int BoatCrateHelpResponseServerCommandType = 389;
    /// <summary>Books a town passenger into a service slot.</summary>
    public const int BookTownPassengerServiceCommandType = 145;
    /// <summary>Buys a package from the ordinary gift catalogue.</summary>
    public const int BuyCatalogueGiftCommandType = 104;

    /// <summary>
    /// Provides the Buy Crop Seeds Command Type value or operation.
    /// </summary>
    public const int BuyCropSeedsCommandType = 665;
    /// <summary>
    /// Provides the Buy Seasonal Catalogue Gift Command Type value or operation.
    /// </summary>
    public const int BuySeasonalCatalogueGiftCommandType = 381;
    /// <summary>Cancels an unsold roadside listing.</summary>
    public const int CancelRoadsideListingCommandType = 589;
    /// <summary>Cancels an unstarted service in the player's town.</summary>
    public const int CancelTownServiceCommandType = 147;
    /// <summary>Discards a truck order without paying to skip its replacement timer.</summary>
    public const int CancelTruckOrderCommandType = 27;
    /// <summary>Starts a seasonal creature's catch animation and collection timer.</summary>
    public const int CatchCreatureCommandType = 669;

    /// <summary>
    /// Provides the Check Mystery Box Lock Command Type value or operation.
    /// </summary>
    public const int CheckMysteryBoxLockCommandType = 46;

    /// <summary>Claims an account-link reward.</summary>
    public const int ClaimAccountLinkRewardCommandType = 300;

    /// <summary>
    /// Provides the Claim Achievement Reward Command Type value or operation.
    /// </summary>
    public const int ClaimAchievementRewardCommandType = 51;

    /// <summary>
    /// Provides the Claim Chain Offer Step Command Type value or operation.
    /// </summary>
    public const int ClaimChainOfferStepCommandType = 691;

    /// <summary>
    /// Provides the Claim Decision Box Command Type value or operation.
    /// </summary>
    public const int ClaimDecisionBoxCommandType = 610;

    /// <summary>
    /// Provides the Claim Deco Sticker Book Collection Reward Command Type value or operation.
    /// </summary>
    public const int ClaimDecoStickerBookCollectionRewardCommandType = 626;

    /// <summary>
    /// Provides the Claim Event Board Seen Reward Command Type value or operation.
    /// </summary>
    public const int ClaimEventBoardSeenRewardCommandType = 534;

    /// <summary>Provides the Claim Farm Pass Baby Pet Reward Command Type.</summary>
    public const int ClaimFarmPassBabyPetRewardCommandType = 346;

    /// <summary>Provides the Claim Farm Pass Level Reward Command Type.</summary>
    public const int ClaimFarmPassLevelRewardCommandType = 336;
    /// <summary>Claims the selected Valley fuel prize.</summary>
    public const int ClaimMapGameFuelPrizeCommandType = 614;

    /// <summary>Claims one Valley quest-progression mystery-box prize.</summary>
    public const int ClaimMapGameQuestProgressionPrizeCommandType = 285;

    /// <summary>
    /// Provides the Clear Event Leaderboard Notification Command Type value or operation.
    /// </summary>
    public const int ClearEventLeaderboardNotificationCommandType = 371;

    /// <summary>
    /// Provides the Clear Movie Ticket Shop Notifications Command Type value or operation.
    /// </summary>
    public const int ClearMovieTicketShopNotificationsCommandType = 564;

    /// <summary>
    /// Provides the client command 33 type. The native semantics are unestablished; the proven wire shape is one var int.
    /// </summary>
    public const int ClientCommand33Type = 33;

    /// <summary>
    /// Provides the client command 47 type. The proven wire shape is one var int, an index into a
    /// level-owned collection; native validates the entry, grants a resource under change reason 0x58,
    /// and notifies a level manager with the same index. None of that reaches a turn checksum lane.
    /// </summary>
    public const int ClientCommand47Type = 47;

    /// <summary>
    /// Provides the client command 528 type. The native semantics are unestablished; the proven wire shape has no fields.
    /// </summary>
    public const int ClientCommand528Type = 528;

    /// <summary>
    /// Provides the client command 686 type. The native semantics are unestablished; the proven wire shape is one var-int array.
    /// </summary>
    public const int ClientCommand686Type = 686;

    /// <summary>
    /// Provides the Close Wheel Car Command Type value or operation.
    /// </summary>
    public const int CloseWheelCarCommandType = 590;

    /// <summary>Collects eligible letters and their gift-card rewards.</summary>
    public const int CollectAllLettersCommandType = 672;

    /// <summary>Identifies collection from Angus's pearl bucket.</summary>
    public const int CollectAngusPearlsCommandType = 701;

    /// <summary>
    /// Provides the Collect Animal Product Command Type value or operation.
    /// </summary>
    public const int CollectAnimalProductCommandType = 586;

    /// <summary>
    /// Provides the Collect Building Product Command Type value or operation.
    /// </summary>
    public const int CollectBuildingProductCommandType = 518;

    /// <summary>Collects an unlocked gift from Greg's calendar.</summary>
    public const int CollectCalendarGiftCommandType = 516;

    /// <summary>Collects the product of a ready duck.</summary>
    public const int CollectDuckCommandType = 159;

    /// <summary>Collects one fish from a fishing spot.</summary>
    public const int CollectFishingSpotCommandType = 109;

    /// <summary>Collects a completed fishing net or animal trap.</summary>
    public const int CollectFishingTrapCommandType = 119;

    /// <summary>
    /// Provides the Collect Fruit Command Type value or operation.
    /// </summary>
    public const int CollectFruitCommandType = 675;

    /// <summary>
    /// Provides the Collect Gatherer Nest Command Type value or operation.
    /// </summary>
    public const int CollectGathererNestCommandType = 156;

    /// <summary>
    /// Provides the Collect Gift Command Type value or operation.
    /// </summary>
    public const int CollectGiftCommandType = 105;

    /// <summary>
    /// Provides the Collect Helper Area Command Type value or operation.
    /// </summary>
    public const int CollectHelperAreaCommandType = 660;

    /// <summary>Collects the product of a ready lobster.</summary>
    public const int CollectLobsterCommandType = 121;

    /// <summary>Collects a sanctuary animal from a shared Valley task.</summary>
    public const int CollectMapGameSanctuaryAnimalCommandType = 310;

    /// <summary>Identifies collection of a grown mollusc.</summary>
    public const int CollectMolluscCommandType = 703;

    /// <summary>
    /// Collects the prepared Movie Ticket reward after its activation.
    /// </summary>
    public const int CollectMovieTicketRewardCommandType = 510;

    /// <summary>
    /// Provides the Collect Mystery Box Reward Command Type value or operation.
    /// </summary>
    public const int CollectMysteryBoxRewardCommandType = 48;

    /// <summary>Collects goods received through a neighborhood donation.</summary>
    public const int CollectNeighborhoodDonationCommandType = 623;

    /// <summary>Collects the proceeds of a sold roadside listing.</summary>
    public const int CollectRoadsideSaleProceedsCommandType = 649;

    /// <summary>Collects the last pending sanctuary visitor gift in town.</summary>
    public const int CollectSanctuaryVisitorGiftCommandType = 566;

    /// <summary>Collects a seasonal gift such as the Halloween ghost chicken.</summary>
    public const int CollectSeasonalCollectibleCommandType = 128;

    /// <summary>Collects a completed town service.</summary>
    public const int CollectTownServiceCommandType = 144;

    /// <summary>
    /// Provides the Collect Truck Delivery Rewards Command Type value or operation.
    /// </summary>
    public const int CollectTruckDeliveryRewardsCommandType = 677;

    /// <summary>
    /// Provides the Collect Wheel Reward Command Type value or operation.
    /// </summary>
    public const int CollectWheelRewardCommandType = 80;

    /// <summary>Completes the selected boat order and grants its rewards.</summary>
    public const int CompleteBoatOrderCommandType = 662;

    /// <summary>Provides the Complete Boy Interaction Command Type.</summary>
    public const int CompleteBoyInteractionCommandType = 653;

    /// <summary>
    /// Provides the Complete Construction Command Type value or operation.
    /// </summary>
    public const int CompleteConstructionCommandType = 17;

    /// <summary>Completes a hooked fishing catch, either keeping it or releasing it.</summary>
    public const int CompleteFishingCatchCommandType = 110;

    /// <summary>
    /// Provides the Complete Forest Clearing Command Type value or operation.
    /// </summary>
    public const int CompleteForestClearingCommandType = 20;
    /// <summary>Completes one personal Valley dump task by submitting its required goods.</summary>
    public const int CompleteMapGameDumpTaskCommandType = 278;

    /// <summary>Completes a finished Neighborhood Object task.</summary>
    public const int CompleteNeighborhoodObjectTaskCommandType = 576;

    /// <summary>Collects clearing rewards and removes the cleared plant.</summary>
    public const int CompletePlantClearingCommandType = 61;

    /// <summary>
    /// Provides the Construct Game Object Command Type value or operation.
    /// </summary>
    public const int ConstructGameObjectCommandType = 577;

    /// <summary>Creates a roadside listing.</summary>
    public const int CreateRoadsideListingCommandType = 574;

    /// <summary>Receives two decoration-event voting candidates.</summary>
    public const int DecorationVoteCandidatesServerCommandType = 755;

    /// <summary>
    /// Provides the Discard Mystery Box Command Type value or operation.
    /// </summary>
    public const int DiscardMysteryBoxCommandType = 45;

    /// <summary>Discards one pending booster without changing stored boosters.</summary>
    public const int DiscardPendingBoosterCommandType = 215;

    /// <summary>
    /// Provides the Dismiss Farm Pass Notification Command Type value or operation.
    /// </summary>
    public const int DismissFarmPassNotificationCommandType = 333;

    /// <summary>Moves an unhired Boy to interval-offer rest.</summary>
    public const int EnterBoyIntervalRestCommandType = 537;

    /// <summary>Exchanges one booster held in the player's booster storage for another booster.</summary>
    public const int ExchangeBoosterCommandType = 214;

    /// <summary>
    /// Provides the Feed Livestock Animal Command Type value or operation.
    /// </summary>
    public const int FeedLivestockAnimalCommandType = 532;

    /// <summary>
    /// Provides the Fill Boat Crate Command Type value or operation.
    /// </summary>
    public const int FillBoatCrateCommandType = 634;

    /// <summary>Accepts a waiting farm visitor's goods order.</summary>
    public const int FulfillFarmVisitorOrderCommandType = 569;

    /// <summary>Completes harvesting a crop field.</summary>
    public const int HarvestFieldCommandType = 506;

    /// <summary>Applies the crop and experience rewards during a field harvest.</summary>
    public const int HarvestFieldGainCommandType = 657;

    /// <summary>Provides the Hire Boy Command Type.</summary>
    public const int HireBoyCommandType = 68;

    /// <summary>Hires one farm helper at a configured duration tier.</summary>
    public const int HireHelperCommandType = 204;

    /// <summary>
    /// Provides the Load Farm Layouts Command Type value or operation.
    /// </summary>
    public const int LoadFarmLayoutsCommandType = 743;
    /// <summary>Acknowledges the introduction of a boat-track reward cycle.</summary>
    public const int MarkBoatTrackCycleIntroSeenCommandType = 702;

    /// <summary>
    /// Provides the Mark Chain Offer Seen Command Type value or operation.
    /// </summary>
    public const int MarkChainOfferSeenCommandType = 624;

    /// <summary>
    /// Provides the Mark Chronos Event Ui Opened Command Type value or operation.
    /// </summary>
    public const int MarkChronosEventUserInterfaceOpenedCommandType = 502;

    /// <summary>Acknowledges opening a County Fair event.</summary>
    public const int MarkCountyFairOpenedCommandType = 522;

    /// <summary>
    /// Provides the Mark Event Board Seen Command Type value or operation.
    /// </summary>
    public const int MarkEventBoardSeenCommandType = 116;

    /// <summary>
    /// Provides the Mark Event Tasks Seen Command Type value or operation.
    /// </summary>
    public const int MarkEventTasksSeenCommandType = 549;

    /// <summary>
    /// Provides the Mark Farm Pass Tasks Seen Command Type value or operation.
    /// </summary>
    public const int MarkFarmPassTasksSeenCommandType = 539;

    /// <summary>Acknowledges the current aggregate count of completed Valley quests.</summary>
    public const int MarkMapGameCompletedQuestsSeenCommandType = 526;

    /// <summary>
    /// Provides the Mark Map Game Current Quests Seen Command Type value or operation.
    /// </summary>
    public const int MarkMapGameCurrentQuestsSeenCommandType = 578;

    /// <summary>Marks one completed Valley daily quest as presented to the player.</summary>
    public const int MarkMapGameDailyQuestSeenCommandType = 286;

    /// <summary>
    /// Provides the Mark Map Game Sun Points Seen Command Type value or operation.
    /// </summary>
    public const int MarkMapGameSunPointsSeenCommandType = 592;

    /// <summary>Records the timestamp of the latest read neighborhood chat entry.</summary>
    public const int MarkNeighborhoodChatReadCommandType = 139;

    /// <summary>
    /// Provides the Mark Neighborhood Tasks Seen Command Type value or operation.
    /// </summary>
    public const int MarkNeighborhoodTasksSeenCommandType = 596;

    /// <summary>
    /// Provides the Mark Task Event Opened Command Type value or operation.
    /// </summary>
    public const int MarkTaskEventOpenedCommandType = 361;

    /// <summary>
    /// Provides the Mark Task Event Seen Command Type value or operation.
    /// </summary>
    public const int MarkTaskEventSeenCommandType = 359;

    /// <summary>Records the current terms-of-service notice version as seen.</summary>
    public const int MarkTermsOfServiceSeenCommandType = 683;

    /// <summary>Records the trading season most recently checked by the player.</summary>
    public const int MarkTradingSeasonCheckedCommandType = 202;

    /// <summary>
    /// Provides the Mark Truck Orders Seen Command Type value or operation.
    /// </summary>
    public const int MarkTruckOrdersSeenCommandType = 26;

    /// <summary>
    /// Provides the Mine Command Type value or operation.
    /// </summary>
    public const int MineCommandType = 64;

    /// <summary>Moves the local Valley pawn toward a target map node.</summary>
    public const int MoveMapGameCommandType = 270;

    /// <summary>Updates points for an active Neighborhood Object event.</summary>
    public const int NeighborhoodObjectPointsServerCommandType = 384;

    /// <summary>Identifies opening a stored mollusc with Angus.</summary>
    public const int OpenMolluscCommandType = 704;

    /// <summary>
    /// Provides the Open Mystery Box Command Type value or operation.
    /// </summary>
    public const int OpenMysteryBoxCommandType = 44;

    /// <summary>Reports a helper's completion at the player's own boat.</summary>
    public const int OwnBoatCrateHelpedServerCommandType = 387;

    /// <summary>Places a bait from <c language="csharp">data/baits.csv</c> on a fishing spot.</summary>
    public const int PlaceFishingBaitCommandType = 111;

    /// <summary>Places a net from <c language="csharp">data/nets.csv</c> on a fishing spot.</summary>
    public const int PlaceFishingNetCommandType = 118;

    /// <summary>Places all newly received pieces in a Sanctuary animal puzzle.</summary>
    public const int PlaceSanctuaryPuzzlePiecesCommandType = 221;

    /// <summary>
    /// Provides the Plant Field Command Type value or operation.
    /// </summary>
    public const int PlantFieldCommandType = 514;

    /// <summary>Pops an unpopped balloon and collects its selected reward.</summary>
    public const int PopBalloonCommandType = 98;

    /// <summary>Applies the saved postman state transition.</summary>
    public const int PostmanStateCommandType = 694;

    /// <summary>Processes the helper's confirmed boat-crate payment.</summary>
    public const int ProcessBoatCrateHelpServerCommandType = 304;

    /// <summary>
    /// Provides the Purchase Livestock Animal Command Type value or operation.
    /// </summary>
    public const int PurchaseLivestockAnimalCommandType = 641;

    /// <summary>Purchases one roadside advertisement credit.</summary>
    public const int PurchaseRoadsideAdvertisementCreditCommandType = 555;

    /// <summary>
    /// Provides the Record Event Seen Command Type value or operation.
    /// </summary>
    public const int RecordEventSeenCommandType = 230;

    /// <summary>
    /// Provides the Record Promotion Popup State Command Type value or operation.
    /// </summary>
    public const int RecordPromotionPopupStateCommandType = 265;

    /// <summary>
    /// Provides the Record Storage Signpost Rank Command Type value or operation.
    /// </summary>
    public const int RecordStorageSignpostRankCommandType = 594;

    /// <summary>Provides the Reject Boy Offer Command Type.</summary>
    public const int RejectBoyOfferCommandType = 583;

    /// <summary>Declines a waiting farm visitor's goods order.</summary>
    public const int RejectFarmVisitorOrderCommandType = 43;

    /// <summary>
    /// Provides the Remote Order Completion Server Command Type value or operation.
    /// </summary>
    public const int RemoteOrderCompletionServerCommandType = 262;

    /// <summary>
    /// Provides the Remote Order Updates Server Command Type value or operation.
    /// </summary>
    public const int RemoteOrderUpdatesServerCommandType = 263;

    /// <summary>Removes a departing duck from the duck salon.</summary>
    public const int RemoveDuckCommandType = 161;

    /// <summary>Removes a departing lobster from the lobster pool.</summary>
    public const int RemoveLobsterCommandType = 122;

    /// <summary>Removes one pending Valley notification after presentation.</summary>
    public const int RemoveMapGameNotificationCommandType = 288;

    /// <summary>
    /// Provides the Remove New Shop Items Command Type value or operation.
    /// </summary>
    public const int RemoveNewShopItemsCommandType = 601;

    /// <summary>Requests an item from the player's neighborhood.</summary>
    public const int RequestNeighborhoodItemCommandType = 199;

    /// <summary>
    /// Provides the Request Newspaper Command Type value or operation.
    /// </summary>
    public const int RequestNewspaperCommandType = 661;

    /// <summary>
    /// Provides the Request Roadside Purchase Command Type value or operation.
    /// </summary>
    public const int RequestRoadsidePurchaseCommandType = 509;

    /// <summary>Requests a shop package; inventory changes arrive in the server receipt.</summary>
    public const int RequestShopPurchaseCommandType = 227;

    /// <summary>
    /// Provides the Reset Wheel Car Command Type value or operation.
    /// </summary>
    public const int ResetWheelCarCommandType = 517;

    /// <summary>Updates friend-count-based roadside stand unlocks.</summary>
    public const int RoadsideFriendCountServerCommandType = 210;

    /// <summary>Identifies the roadside purchase notification delivered to the client listener.</summary>
    public const int RoadsidePurchaseRejectedServerCommandType = 309;

    /// <summary>
    /// Provides the Roadside Purchase Server Command Type value or operation.
    /// </summary>
    public const int RoadsidePurchaseServerCommandType = 243;

    /// <summary>Records a roadside listing's buyer.</summary>
    public const int RoadsideSaleServerCommandType = 375;

    /// <summary>
    /// Provides the Roadside Stock Server Command Type value or operation.
    /// </summary>
    public const int RoadsideStockServerCommandType = 244;
    /// <summary>Starts panic movement for all residents of a livestock habitat.</summary>
    public const int ScareLivestockCommandType = 6;

    /// <summary>Provides the Search With Boy Command Type.</summary>
    public const int SearchWithBoyCommandType = 71;

    /// <summary>
    /// Provides the Select Boat Order Command Type value or operation.
    /// </summary>
    public const int SelectBoatOrderCommandType = 570;

    /// <summary>Provides the Select Boy Offer Command Type.</summary>
    public const int SelectBoyOfferCommandType = 70;

    /// <summary>
    /// Provides the Select Livestock Animal Command Type value or operation.
    /// </summary>
    public const int SelectLivestockAnimalCommandType = 21;
    /// <summary>Sends a thank-you gift for boat or plant help.</summary>
    public const int SendThankYouGiftCommandType = 102;

    /// <summary>
    /// Provides the Server Command148 Type value or operation.
    /// </summary>
    public const int ServerCommand148Type = 148;

    /// <summary>Identifies the Angus interaction contract, which has no baseline wire id.</summary>
    public const int SetAngusInteractionCommandType = 700;

    /// <summary>Provides the Set Boy Offer Flag Command Type.</summary>
    public const int SetBoyOfferFlagCommandType = 132;

    /// <summary>Moves one fishing-area fish to the selected runtime state.</summary>
    public const int SetFishStateCommandType = 112;

    /// <summary>Replaces a farm helper's production order quantities.</summary>
    public const int SetHelperOrdersCommandType = 205;

    /// <summary>Sets state flags on a selected Mini Pass instance.</summary>
    public const int SetMiniPassStateFlagsCommandType = 560;

    /// <summary>Sets Neighborhood Object leaderboard scores by long id.</summary>
    public const int SetNeighborhoodObjectLeaderboardScoresCommandType = 636;

    /// <summary>Changes a notification or advanced user setting.</summary>
    public const int SetUserSettingCommandType = 131;

    /// <summary>Applies a shop package receipt and its currency adjustments.</summary>
    public const int ShopPurchaseServerCommandType = 229;

    /// <summary>Completes a running town service using premium currency.</summary>
    public const int SpeedUpTownServiceCommandType = 143;

    /// <summary>Spins the active Valley fuel wheel.</summary>
    public const int SpinMapGameFuelWheelCommandType = 616;

    /// <summary>
    /// Provides the Start Building Production Command Type value or operation.
    /// </summary>
    public const int StartBuildingProductionCommandType = 606;

    /// <summary>
    /// Provides the Start Farm Pass Season Command Type value or operation.
    /// </summary>
    public const int StartFarmPassSeasonCommandType = 644;

    /// <summary>
    /// Provides the Start Forest Clearing Command Type value or operation.
    /// </summary>
    public const int StartForestClearingCommandType = 18;

    /// <summary>Starts harvesting a ready crop field.</summary>
    public const int StartHarvestFieldCommandType = 544;

    /// <summary>Starts clearing a depleted fruit tree or gatherer source.</summary>
    public const int StartPlantClearingCommandType = 60;

    /// <summary>Starts a selected service at a town service building.</summary>
    public const int StartTownServiceCommandType = 142;

    /// <summary>
    /// Provides the Start Truck Delivery Command Type value or operation.
    /// </summary>
    public const int StartTruckDeliveryCommandType = 612;

    /// <summary>
    /// Provides the Start Wheel Spin Command Type value or operation.
    /// </summary>
    public const int StartWheelSpinCommandType = 587;

    /// <summary>Submits a vote for a decoration-event canvas candidate.</summary>
    public const int SubmitDecorationVoteCommandType = 687;

    /// <summary>Taps an ambient animal identified by its runtime object id.</summary>
    public const int TapAmbientAnimalCommandType = 42;

    /// <summary>Plays the tap reaction of an available Sanctuary animal.</summary>
    public const int TapSanctuaryAnimalCommandType = 220;

    /// <summary>Toggles an event type's liked state using a linked event instance.</summary>
    public const int ToggleEventTypeLikeCommandType = 565;

    /// <summary>Applies a town-passenger action.</summary>
    public const int TownPassengerActionCommandType = 140;

    /// <summary>
    /// Provides the Passenger Service Completion Server Command Type value or operation.
    /// </summary>
    public const int TownServiceHelpCompletedServerCommandType = 253;

    /// <summary>
    /// Provides the Tree Revival Server Command Type value or operation.
    /// </summary>
    public const int TreeRevivalServerCommandType = 328;

    /// <summary>Unlocks the next roadside stand using diamonds.</summary>
    public const int UnlockRoadsideStandCommandType = 631;

    /// <summary>
    /// Provides the Update Task Event State Command Type value or operation.
    /// </summary>
    public const int UpdateTaskEventStateCommandType = 360;

    /// <summary>
    /// Provides the Upgrade Building Command Type value or operation.
    /// </summary>
    public const int UpgradeBuildingCommandType = 11;

    /// <summary>Validates a town passenger without changing its state.</summary>
    public const int ValidateTownPassengerCommandType = 141;

    /// <summary>
    /// Provides the Visited Boat Departure Server Command Type value or operation.
    /// </summary>
    public const int VisitedBoatDepartureServerCommandType = 385;

    /// <summary>
    /// Provides the Visited Boat Help Request Server Command Type value or operation.
    /// </summary>
    public const int VisitedBoatHelpRequestServerCommandType = 388;

    /// <summary>
    /// Provides the Visited Boat Help Server Command Type value or operation.
    /// </summary>
    public const int VisitedBoatHelpServerCommandType = 386;

    /// <summary>
    /// Provides the Visited Boat State Server Command Type value or operation.
    /// </summary>
    public const int VisitedBoatStateServerCommandType = 810;

    /// <summary>Wakes the Boy from his interval rest.</summary>
    public const int WakeBoyFromRestCommandType = 595;

    /// <summary>Wakes a sleeping pet and collects its feeding reward.</summary>
    public const int WakePetCommandType = 87;

    private static readonly Lazy<Dictionary<int, CommandRegistryEntry>> LazyEntries = new(CreateEntries);
    private static readonly HashSet<int> NonProductionCommandTypes = [7, 84, 85];

    /// <summary>Gets all registered command contracts by id.</summary>
    public static IReadOnlyDictionary<int, CommandRegistryEntry> Registrations =>
        Entries.AsReadOnly();
    private static Dictionary<int, CommandRegistryEntry> Entries => LazyEntries.Value;

    /// <summary>
    /// Decodes one registered command from native wire data.
    /// Rejects command types that are not valid in the selected environment.
    /// </summary>
    public static Command Decode(MessageStream stream, CommandEnvironment environment, ICommandDataResolver? dataResolver = null)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int commandType = stream.ReadVarInt();

        return Decode(commandType, stream, environment, dataResolver);
    }

    /// <summary>Decodes a command when its type has already been read from the wire.</summary>
    public static Command Decode(int commandType, MessageStream stream, CommandEnvironment environment, ICommandDataResolver? dataResolver = null)
    {
        ArgumentNullException.ThrowIfNull(stream);
        commandType = GetCommandType(commandType, stream.GameVersion);

        if (!Entries.TryGetValue(commandType, out CommandRegistryEntry? entry))
            throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"Logic command type {commandType} is not supported."));

        EnsureAllowedEnvironment(commandType, environment);

        try
        {
            bool serverCommand = entry.Direction is MessageDirection.Clientbound;
            CommandMetadata? metadata = entry.BaseFirst ? CommandMetadata.Decode(stream, environment, serverCommand) : null;
            Command command = entry.Factory(stream, environment, dataResolver);
            metadata ??= CommandMetadata.Decode(stream, environment, serverCommand);

            return metadata.Apply(command);
        }
        catch (InvalidDataException exception)
        {
            throw new InvalidDataException(
                string.Create(CultureInfo.InvariantCulture, $"Command {commandType} decoding failed at byte {stream.Position}: {exception.Message}"),
                exception
            );
        }
    }

    /// <summary>
    /// Encodes one registered command in native wire order.
    /// Rejects command models that do not match the registered type.
    /// </summary>
    public static void Encode(MessageStream stream, Command command, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(stream);

        if (!Entries.TryGetValue(command.Type, out CommandRegistryEntry? entry) || !entry.Type.IsInstanceOfType(command))
            throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"Logic command type {command.Type} is not supported."));

        EnsureAllowedEnvironment(command.Type, environment);
        stream.WriteVarInt(GetId(command, stream.GameVersion));

        if (entry.BaseFirst)
            CommandMetadata.Encode(stream, command, environment);

        command.Encode(stream, environment);

        if (!entry.BaseFirst)
            CommandMetadata.Encode(stream, command, environment);
    }

    /// <summary>
    /// Provides the Validate Fields value or operation.
    /// </summary>
    public static void ValidateFields(int type, ReadOnlySpan<CommandField> fields, MessageDirection direction)
    {
        if (FindEntry(type) is not { } entry || entry.Direction != direction || entry.FieldSchemas is null)
            throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"Logic command type {type} does not have a registered primitive field schema."));

        if (!CommandFieldSchema.AreValid(entry.FieldSchemas, fields))
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Logic command type {type} fields do not match the registered native schema."));
    }

    internal static void AddStructuredFieldCommands(
        Dictionary<int, CommandRegistryEntry> entries,
        ReadOnlySpan<int> commandTypes,
        CommandFieldSchema[] fieldSchemas,
        MessageDirection direction,
        bool baseFirst = true
    )
    {
        foreach (int type in commandTypes)
        {
            int commandType = type;
            entries.Add(
                commandType,
                new CommandRegistryEntry(
                    direction is MessageDirection.Clientbound
                        ? typeof(ServerCommandWithFields)
                        : typeof(CommandWithFields),
                    direction,
                    baseFirst,
                    fieldSchemas,
                    direction is MessageDirection.Clientbound
                        ? (stream, environment, unusedParameter2) =>
                            ServerCommandWithFields.Decode(commandType, fieldSchemas, stream)
                        : (stream, environment, unusedParameter2) =>
                            CommandWithFields.Decode(commandType, fieldSchemas, stream)
                )
            );
        }
    }

    private static void AddFieldCommands(
        Dictionary<int, CommandRegistryEntry> entries,
        ReadOnlySpan<int> commandTypes,
        CommandFieldType[] fieldTypes,
        MessageDirection direction,
        bool baseFirst = true
    )
    {
        AddStructuredFieldCommands(entries, commandTypes, [.. fieldTypes.Select(CommandFieldSchema.Primitive)], direction, baseFirst);
    }

    private static void AddPrimitiveSchemas(Dictionary<int, CommandRegistryEntry> entries, IEnumerable<CommandPrimitiveSchema> schemas)
    {
        foreach (CommandPrimitiveSchema schema in schemas)
            AddFieldCommands(entries, schema.CommandTypes, schema.FieldTypes, schema.Direction, schema.BaseFirst);
    }

    private static void AddVarCommandEntries(Dictionary<int, CommandRegistryEntry> entries)
    {
        int[] commandTypes = CommandWithNoFields.CommandTypes;

        foreach (int type in commandTypes)
        {
            int commandType = type;
            entries.Add(
                commandType,
                new CommandRegistryEntry(
                    typeof(CommandWithNoFields),
                    MessageDirection.Serverbound,
                    BaseFirst: true,
                    FieldSchemas: null,
                    (stream, environment, unusedParameter2) =>
                        CommandWithNoFields.Decode(commandType, stream, environment)
                )
            );
        }

        int[] commandTypes2 = MapGameTaskCommand.CommandTypes;

        foreach (int type2 in commandTypes2)
        {
            int commandType2 = type2;
            entries.Add(
                commandType2,
                new CommandRegistryEntry(
                    typeof(MapGameTaskCommand),
                    MessageDirection.Serverbound,
                    BaseFirst: true,
                    FieldSchemas: null,
                    (stream, environment, dataResolver) =>
                        MapGameTaskCommand.Decode(commandType2, stream, environment, dataResolver)
                )
            );
        }
    }

    private static Dictionary<int, CommandRegistryEntry> CreateEntries()
    {
        Dictionary<int, CommandRegistryEntry> entries = new(TypedCommandRegistrations.Entries);

        foreach (KeyValuePair<int, CommandRegistryEntry> entry in FishingCommandRegistrations.Entries)
            entries.Add(entry.Key, entry.Value);

        foreach (KeyValuePair<int, CommandRegistryEntry> entry in DerbyCommandRegistrations.Entries)
            entries.Add(entry.Key, entry.Value);

        AddVarCommandEntries(entries);
        AddPrimitiveSchemas(entries, PrimitiveCommandSchemas.Entries);
        StructuredCommandRegistrations.AddStructuredCommands(entries);

        return entries;
    }

    private static void EnsureAllowedEnvironment(int commandType, CommandEnvironment environment)
    {
        if (environment is CommandEnvironment.Production && NonProductionCommandTypes.Contains(commandType))
            throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"Logic command type {commandType} is not allowed in the production environment."));
    }

    private static CommandRegistryEntry? FindEntry(int type)
    {
        return Entries.GetValueOrDefault(type);
    }
}
