using System.Globalization;

using SupercellProxy.Networking.Protocol.CommandEncoding.FieldSchemas;
using SupercellProxy.Networking.Protocol.MapGame.Tasks;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

/// <summary>Maps native command identifiers to wire contracts and validates their registered encoding schemas.</summary>
public static class CommandRegistry
{
    /// <summary>
    /// Provides the Acknowledge Boat Command Type value or operation.
    /// </summary>
    public const int AcknowledgeBoatCommandType = 674;

    /// <summary>Activates a booster held in the player's booster storage.</summary>
    public const int ActivateBoosterCommandType = 212;

    /// <summary>Provides the Activate Farm Pass Perk Command Type.</summary>
    public const int ActivateFarmPassPerkCommandType = 343;

    /// <summary>
    /// Provides the Activate Movie Ticket Command Type value or operation.
    /// </summary>
    public const int ActivateMovieTicketCommandType = 643;

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

    /// <summary>
    /// Provides the Check Mystery Box Lock Command Type value or operation.
    /// </summary>
    public const int CheckMysteryBoxLockCommandType = 46;

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

    /// <summary>
    /// Provides the Clear Event Leaderboard Notification Command Type value or operation.
    /// </summary>
    public const int ClearEventLeaderboardNotificationCommandType = 371;

    /// <summary>
    /// Provides the Clear Movie Ticket Shop Notifications Command Type value or operation.
    /// </summary>
    public const int ClearMovieTicketShopNotificationsCommandType = 564;

    /// <summary>
    /// Provides the client command 33 type. The native semantics are unestablished; the proven wire shape is one variable int.
    /// </summary>
    public const int ClientCommand33Type = 33;

    /// <summary>
    /// Provides the client command 47 type. The proven wire shape is one variable int, an index into a
    /// level-owned collection; native validates the entry, grants a resource under change reason 0x58,
    /// and notifies a level manager with the same index. None of that reaches a turn checksum lane.
    /// </summary>
    public const int ClientCommand47Type = 47;

    /// <summary>
    /// Provides the client command 528 type. The native semantics are unestablished; the proven wire shape has no fields.
    /// </summary>
    public const int ClientCommand528Type = 528;

    /// <summary>
    /// Provides the client command 686 type. The native semantics are unestablished; the proven wire shape is one variable-int array.
    /// </summary>
    public const int ClientCommand686Type = 686;

    /// <summary>
    /// Provides the Close Wheel Car Command Type value or operation.
    /// </summary>
    public const int CloseWheelCarCommandType = 590;

    /// <summary>
    /// Provides the Collect Animal Product Command Type value or operation.
    /// </summary>
    public const int CollectAnimalProductCommandType = 586;

    /// <summary>
    /// Provides the Collect Building Product Command Type value or operation.
    /// </summary>
    public const int CollectBuildingProductCommandType = 518;

    /// <summary>Collects one fish from a fishing spot.</summary>
    public const int CollectFishingSpotCommandType = 109;

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

    /// <summary>
    /// Provides the Collect Mystery Box Reward Command Type value or operation.
    /// </summary>
    public const int CollectMysteryBoxRewardCommandType = 48;
    /// <summary>Collects the proceeds of a sold roadside listing.</summary>
    public const int CollectRoadsideSaleProceedsCommandType = 649;

    /// <summary>
    /// Provides the Collect Truck Delivery Rewards Command Type value or operation.
    /// </summary>
    public const int CollectTruckDeliveryRewardsCommandType = 677;

    /// <summary>
    /// Provides the Collect Wheel Reward Command Type value or operation.
    /// </summary>
    public const int CollectWheelRewardCommandType = 80;

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

    /// <summary>
    /// Provides the Construct Game Object Command Type value or operation.
    /// </summary>
    public const int ConstructGameObjectCommandType = 577;
    /// <summary>Creates a roadside listing.</summary>
    public const int CreateRoadsideListingCommandType = 574;

    /// <summary>
    /// Provides the Discard Mystery Box Command Type value or operation.
    /// </summary>
    public const int DiscardMysteryBoxCommandType = 45;

    /// <summary>
    /// Provides the Dismiss Farm Pass Notification Command Type value or operation.
    /// </summary>
    public const int DismissFarmPassNotificationCommandType = 333;

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

    /// <summary>Provides the Hire Boy Command Type.</summary>
    public const int HireBoyCommandType = 68;

    /// <summary>
    /// Provides the Home Loaded Command Type value or operation.
    /// </summary>
    public const int HomeLoadedCommandType = 530;

    /// <summary>
    /// Provides the Load Farm Layouts Command Type value or operation.
    /// </summary>
    public const int LoadFarmLayoutsCommandType = 743;

    /// <summary>
    /// Provides the Mark Chain Offer Seen Command Type value or operation.
    /// </summary>
    public const int MarkChainOfferSeenCommandType = 624;

    /// <summary>
    /// Provides the Mark Chronos Event Ui Opened Command Type value or operation.
    /// </summary>
    public const int MarkChronosEventUserInterfaceOpenedCommandType = 502;

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

    /// <summary>
    /// Provides the Mark Map Game Sun Points Seen Command Type value or operation.
    /// </summary>
    public const int MarkMapGameSunPointsSeenCommandType = 592;

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

    /// <summary>
    /// Provides the Mark Truck Orders Seen Command Type value or operation.
    /// </summary>
    public const int MarkTruckOrdersSeenCommandType = 26;

    /// <summary>
    /// Provides the Mine Command Type value or operation.
    /// </summary>
    public const int MineCommandType = 64;

    /// <summary>
    /// Provides the Movie Ticket Ad Watched Command Type value or operation.
    /// </summary>
    public const int MovieTicketAdWatchedCommandType = 510;

    /// <summary>
    /// Provides the Open Mystery Box Command Type value or operation.
    /// </summary>
    public const int OpenMysteryBoxCommandType = 44;

    /// <summary>
    /// Provides the Passenger Service Completion Server Command Type value or operation.
    /// </summary>
    public const int PassengerServiceCompletionServerCommandType = 253;

    /// <summary>Places a bait from <c language="csharp">data/baits.csv</c> on a fishing spot.</summary>
    public const int PlaceFishingBaitCommandType = 111;

    /// <summary>Places a net from <c language="csharp">data/nets.csv</c> on a fishing spot.</summary>
    public const int PlaceFishingNetCommandType = 118;

    /// <summary>
    /// Provides the Plant Field Command Type value or operation.
    /// </summary>
    public const int PlantFieldCommandType = 514;

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

    /// <summary>
    /// Provides the Remote Order Completion Server Command Type value or operation.
    /// </summary>
    public const int RemoteOrderCompletionServerCommandType = 262;

    /// <summary>
    /// Provides the Remote Order Updates Server Command Type value or operation.
    /// </summary>
    public const int RemoteOrderUpdatesServerCommandType = 263;

    /// <summary>Removes one pending Valley notification after presentation.</summary>
    public const int RemoveMapGameNotificationCommandType = 288;

    /// <summary>
    /// Provides the Remove New Shop Items Command Type value or operation.
    /// </summary>
    public const int RemoveNewShopItemsCommandType = 601;

    /// <summary>
    /// Provides the Request Newspaper Command Type value or operation.
    /// </summary>
    public const int RequestNewspaperCommandType = 661;
    /// <summary>
    /// Provides the Request Roadside Purchase Command Type value or operation.
    /// </summary>
    public const int RequestRoadsidePurchaseCommandType = 509;

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

    /// <summary>
    /// Provides the Server Command148 Type value or operation.
    /// </summary>
    public const int ServerCommand148Type = 148;

    /// <summary>Provides the Set Boy Offer Flag Command Type.</summary>
    public const int SetBoyOfferFlagCommandType = 132;

    /// <summary>Moves one fishing-area fish to the selected runtime state.</summary>
    public const int SetFishStateCommandType = 112;

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

    /// <summary>
    /// Provides the Start Truck Delivery Command Type value or operation.
    /// </summary>
    public const int StartTruckDeliveryCommandType = 612;

    /// <summary>
    /// Provides the Start Wheel Spin Command Type value or operation.
    /// </summary>
    public const int StartWheelSpinCommandType = 587;

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

    private static readonly Lazy<Dictionary<int, CommandRegistryEntry>> LazyEntries = new(CreateEntries);
    private static readonly HashSet<int> NonProductionCommandTypes = [7, 84, 85];

    /// <summary>Gets all registered command contracts by identifier.</summary>
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
        int commandType = stream.ReadVariableInt();

        if (!Entries.TryGetValue(commandType, out CommandRegistryEntry? entry))
            throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"Logic command type {commandType} is not supported."));

        EnsureAllowedEnvironment(commandType, environment);

        try
        {
            return entry.Factory(stream, environment, dataResolver);
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
        stream.WriteVariableInt(command.Type);
        command.EncodeBody(stream, environment);
    }

    /// <summary>
    /// Provides the Validate Fields value or operation.
    /// </summary>
    public static bool ValidateFields(int type, ReadOnlySpan<CommandField> fields, MessageDirection direction)
    {
        return FindEntry(type) is not { } entry || entry.Direction != direction || entry.FieldSchemas is null
            ? throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"Logic command type {type} does not have a registered primitive field schema."))
            : !CommandFieldSchema.AreValid(entry.FieldSchemas, fields)
            ? throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Logic command type {type} fields do not match the registered native schema."))
            : entry.BaseFirst;
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
                            ServerCommandWithFields.Decode(commandType, fieldSchemas, baseFirst, stream, environment)
                        : (stream, environment, unusedParameter2) =>
                            CommandWithFields.Decode(commandType, fieldSchemas, baseFirst, stream, environment)
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

    private static void AddVariableCommandEntries(Dictionary<int, CommandRegistryEntry> entries)
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
        AddVariableCommandEntries(entries);
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
