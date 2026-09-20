using SupercellProxy.Networking.Protocol.Achievements;
using SupercellProxy.Networking.Protocol.Animals;
using SupercellProxy.Networking.Protocol.Boats;
using SupercellProxy.Networking.Protocol.Boosters;
using SupercellProxy.Networking.Protocol.CollectionPayloads;
using SupercellProxy.Networking.Protocol.CropFields;
using SupercellProxy.Networking.Protocol.Events;
using SupercellProxy.Networking.Protocol.Events.Boards;
using SupercellProxy.Networking.Protocol.Events.Chronos;
using SupercellProxy.Networking.Protocol.Events.Decoration;
using SupercellProxy.Networking.Protocol.Events.Tasks;
using SupercellProxy.Networking.Protocol.FarmLayouts;
using SupercellProxy.Networking.Protocol.FarmPass;
using SupercellProxy.Networking.Protocol.Fishing;
using SupercellProxy.Networking.Protocol.Forestry;
using SupercellProxy.Networking.Protocol.GameObjects;
using SupercellProxy.Networking.Protocol.Gifts;
using SupercellProxy.Networking.Protocol.Gatherers;
using SupercellProxy.Networking.Protocol.Mail;
using SupercellProxy.Networking.Protocol.MapGame;
using SupercellProxy.Networking.Protocol.MapGame.Events;
using SupercellProxy.Networking.Protocol.MapGame.Notifications;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.MovieTickets;
using SupercellProxy.Networking.Protocol.MysteryBoxes;
using SupercellProxy.Networking.Protocol.Newspapers;
using SupercellProxy.Networking.Protocol.OpaquePayloads;
using SupercellProxy.Networking.Protocol.Orders;
using SupercellProxy.Networking.Protocol.PrizeWheels;
using SupercellProxy.Networking.Protocol.Production;
using SupercellProxy.Networking.Protocol.RoadsideShops;
using SupercellProxy.Networking.Protocol.ShopEvents;
using SupercellProxy.Networking.Protocol.Town;
using SupercellProxy.Networking.Protocol.Tutorials;

using static SupercellProxy.Networking.Protocol.CommandEncoding.Registration.CommandRegistry;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

internal static class TypedCommandRegistrations
{
    internal static readonly Dictionary<int, CommandRegistryEntry> Entries = new()
    {
        [ActivateBoosterCommandType] = new CommandRegistryEntry(
            typeof(ActivateBoosterCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                ActivateBoosterCommand.Decode(stream, environment)
        ),
        [ExchangeBoosterCommandType] = new CommandRegistryEntry(
            typeof(ExchangeBoosterCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                ExchangeBoosterCommand.Decode(stream, environment)
        ),
        [CancelRoadsideListingCommandType] = new CommandRegistryEntry(
            typeof(CancelRoadsideListingCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, resolver) => CancelRoadsideListingCommand.Decode(stream, environment)
        ),
        [AdvertiseRoadsideListingCommandType] = new CommandRegistryEntry(
            typeof(AdvertiseRoadsideListingCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, resolver) => AdvertiseRoadsideListingCommand.Decode(stream, environment)
        ),
        [RoadsidePurchaseRejectedServerCommandType] = new CommandRegistryEntry(
            typeof(RoadsidePurchaseRejectedServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, resolver) => RoadsidePurchaseRejectedServerCommand.Decode(stream, environment)
        ),
        [ActivateFarmPassPerkCommandType] = new CommandRegistryEntry(
            typeof(ActivateFarmPassPerkCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                ActivateFarmPassPerkCommand.Decode(stream, environment)
        ),
        [RoadsidePurchaseServerCommandType] = new CommandRegistryEntry(
            typeof(RoadsidePurchaseServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                RoadsidePurchaseServerCommand.Decode(stream, environment)
        ),
        [PassengerServiceCompletionServerCommandType] = new CommandRegistryEntry(
            typeof(PassengerServiceCompletionServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                PassengerServiceCompletionServerCommand.Decode(stream, environment)
        ),
        [VisitedBoatHelpRequestServerCommandType] = new CommandRegistryEntry(
            typeof(VisitedBoatHelpRequestServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                VisitedBoatHelpRequestServerCommand.Decode(stream, environment)
        ),
        [ClaimFarmPassLevelRewardCommandType] = new CommandRegistryEntry(
            typeof(ClaimFarmPassLevelRewardCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                ClaimFarmPassLevelRewardCommand.Decode(stream, environment)
        ),
        [ClaimFarmPassBabyPetRewardCommandType] = new CommandRegistryEntry(
            typeof(ClaimFarmPassBabyPetRewardCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                ClaimFarmPassBabyPetRewardCommand.Decode(stream, environment)
        ),
        [DismissFarmPassNotificationCommandType] = new CommandRegistryEntry(
            typeof(DismissFarmPassNotificationCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                DismissFarmPassNotificationCommand.Decode(stream, environment)
        ),
        [RemoveMapGameNotificationCommandType] = new CommandRegistryEntry(
            typeof(RemoveMapGameNotificationCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                RemoveMapGameNotificationCommand.Decode(stream, environment)
        ),
        [VisitedBoatDepartureServerCommandType] = new CommandRegistryEntry(
            typeof(VisitedBoatDepartureServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                VisitedBoatDepartureServerCommand.Decode(stream, environment)
        ),
        [VisitedBoatStateServerCommandType] = new CommandRegistryEntry(
            typeof(VisitedBoatStateServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                VisitedBoatStateServerCommand.Decode(stream, environment)
        ),
        [RoadsideStockServerCommandType] = new CommandRegistryEntry(
            typeof(RoadsideStockServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                RoadsideStockServerCommand.Decode(stream, environment)
        ),
        [StartTruckDeliveryCommandType] = new CommandRegistryEntry(
            typeof(StartTruckDeliveryCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                StartTruckDeliveryCommand.Decode(stream, environment)
        ),
        [FillBoatCrateCommandType] = new CommandRegistryEntry(
            typeof(FillBoatCrateCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                FillBoatCrateCommand.Decode(stream, environment)
        ),
        [SelectBoatOrderCommandType] = new CommandRegistryEntry(
            typeof(SelectBoatOrderCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                SelectBoatOrderCommand.Decode(stream, environment)
        ),
        [CompleteConstructionCommandType] = new CommandRegistryEntry(
            typeof(CompleteConstructionCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                CompleteConstructionCommand.Decode(stream, environment)
        ),
        [ActivateMovieTicketCommandType] = new CommandRegistryEntry(
            typeof(ActivateMovieTicketCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                ActivateMovieTicketCommand.Decode(stream, environment)
        ),
        [RecordEventSeenCommandType] = new CommandRegistryEntry(
            typeof(RecordEventSeenCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                RecordEventSeenCommand.Decode(stream, environment)
        ),
        [ClearEventLeaderboardNotificationCommandType] = new CommandRegistryEntry(
            typeof(ClearEventLeaderboardNotificationCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                ClearEventLeaderboardNotificationCommand.Decode(stream, environment)
        ),
        [MarkEventTasksSeenCommandType] = new CommandRegistryEntry(
            typeof(MarkEventTasksSeenCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                MarkEventTasksSeenCommand.Decode(stream, environment)
        ),
        [MarkTaskEventOpenedCommandType] = new CommandRegistryEntry(
            typeof(MarkTaskEventOpenedCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                MarkTaskEventOpenedCommand.Decode(stream, environment)
        ),
        [MarkChronosEventUserInterfaceOpenedCommandType] = new CommandRegistryEntry(
            typeof(MarkChronosEventUserInterfaceOpenedCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                MarkChronosEventUserInterfaceOpenedCommand.Decode(stream, environment)
        ),
        [BuySeasonalCatalogueGiftCommandType] = new CommandRegistryEntry(
            typeof(BuySeasonalCatalogueGiftCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                BuySeasonalCatalogueGiftCommand.Decode(stream, environment)
        ),
        [ClaimDecisionBoxCommandType] = new CommandRegistryEntry(
            typeof(ClaimDecisionBoxCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                ClaimDecisionBoxCommand.Decode(stream, environment)
        ),
        [ClaimChainOfferStepCommandType] = new CommandRegistryEntry(
            typeof(ClaimChainOfferStepCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                ClaimChainOfferStepCommand.Decode(stream, environment)
        ),
        [CollectTruckDeliveryRewardsCommandType] = new CommandRegistryEntry(
            typeof(CollectTruckDeliveryRewardsCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                CollectTruckDeliveryRewardsCommand.Decode(stream, environment)
        ),
        [ConstructGameObjectCommandType] = new CommandRegistryEntry(
            typeof(ConstructGameObjectCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                ConstructGameObjectCommand.Decode(stream, environment)
        ),
        [CollectWheelRewardCommandType] = new CommandRegistryEntry(
            typeof(CollectWheelRewardCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                CollectWheelRewardCommand.Decode(stream, environment)
        ),
        [ClaimAchievementRewardCommandType] = new CommandRegistryEntry(
            typeof(ClaimAchievementRewardCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                ClaimAchievementRewardCommand.Decode(stream, environment)
        ),
        [CollectGiftCommandType] = new CommandRegistryEntry(
            typeof(CollectGiftCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                CollectGiftCommand.Decode(stream, environment)
        ),
        [MarkTruckOrdersSeenCommandType] = new CommandRegistryEntry(
            typeof(MarkTruckOrdersSeenCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                MarkTruckOrdersSeenCommand.Decode(stream, environment)
        ),
        [CollectMysteryBoxRewardCommandType] = new CommandRegistryEntry(
            typeof(CollectMysteryBoxRewardCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                CollectMysteryBoxRewardCommand.Decode(stream, environment)
        ),
        [OpenMysteryBoxCommandType] = new CommandRegistryEntry(
            typeof(OpenMysteryBoxCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                OpenMysteryBoxCommand.Decode(stream, environment)
        ),
        [CheckMysteryBoxLockCommandType] = new CommandRegistryEntry(
            typeof(CheckMysteryBoxLockCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                CheckMysteryBoxLockCommand.Decode(stream, environment)
        ),
        [UpgradeBuildingCommandType] = new CommandRegistryEntry(
            typeof(UpgradeBuildingCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                UpgradeBuildingCommand.Decode(stream, environment)
        ),
        [RecordStorageSignpostRankCommandType] = new CommandRegistryEntry(
            typeof(RecordStorageSignpostRankCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                RecordStorageSignpostRankCommand.Decode(stream, environment)
        ),
        [CollectBuildingProductCommandType] = new CommandRegistryEntry(
            typeof(CollectBuildingProductCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                CollectBuildingProductCommand.Decode(stream, environment)
        ),
        [CollectGathererNestCommandType] = new CommandRegistryEntry(
            typeof(CollectGathererNestCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                CollectGathererNestCommand.Decode(stream, environment)
        ),
        [SetFishStateCommandType] = new CommandRegistryEntry(
            typeof(SetFishStateCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                SetFishStateCommand.Decode(stream, environment)
        ),
        [CollectFishingSpotCommandType] = new CommandRegistryEntry(
            typeof(CollectFishingSpotCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                CollectFishingSpotCommand.Decode(stream, environment)
        ),
        [CompleteFishingCatchCommandType] = new CommandRegistryEntry(
            typeof(CompleteFishingCatchCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                CompleteFishingCatchCommand.Decode(stream, environment)
        ),
        [PlaceFishingBaitCommandType] = new CommandRegistryEntry(
            typeof(PlaceFishingBaitCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                PlaceFishingBaitCommand.Decode(stream, environment)
        ),
        [PlaceFishingNetCommandType] = new CommandRegistryEntry(
            typeof(PlaceFishingNetCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                PlaceFishingNetCommand.Decode(stream, environment)
        ),
        [StartBuildingProductionCommandType] = new CommandRegistryEntry(
            typeof(StartBuildingProductionCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                StartBuildingProductionCommand.Decode(stream, environment)
        ),
        [CompleteForestClearingCommandType] = new CommandRegistryEntry(
            typeof(CompleteForestClearingCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                CompleteForestClearingCommand.Decode(stream, environment)
        ),
        [CollectFruitCommandType] = new CommandRegistryEntry(
            typeof(CollectFruitCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                CollectFruitCommand.Decode(stream, environment)
        ),
        [StartForestClearingCommandType] = new CommandRegistryEntry(
            typeof(StartForestClearingCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                StartForestClearingCommand.Decode(stream, environment)
        ),
        [FeedLivestockAnimalCommandType] = new CommandRegistryEntry(
            typeof(FeedLivestockAnimalCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                FeedLivestockAnimalCommand.Decode(stream, environment)
        ),
        [PurchaseLivestockAnimalCommandType] = new CommandRegistryEntry(
            typeof(PurchaseLivestockAnimalCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                PurchaseLivestockAnimalCommand.Decode(stream, environment)
        ),
        [CollectAnimalProductCommandType] = new CommandRegistryEntry(
            typeof(CollectAnimalProductCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                CollectAnimalProductCommand.Decode(stream, environment)
        ),
        [SelectLivestockAnimalCommandType] = new CommandRegistryEntry(
            typeof(SelectLivestockAnimalCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                SelectLivestockAnimalCommand.Decode(stream, environment)
        ),
        [ClaimEventBoardSeenRewardCommandType] = new CommandRegistryEntry(
            typeof(ClaimEventBoardSeenRewardCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                ClaimEventBoardSeenRewardCommand.Decode(stream, environment)
        ),
        [MarkEventBoardSeenCommandType] = new CommandRegistryEntry(
            typeof(MarkEventBoardSeenCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                MarkEventBoardSeenCommand.Decode(stream, environment)
        ),
        [RequestNewspaperCommandType] = new CommandRegistryEntry(
            typeof(RequestNewspaperCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                RequestNewspaperCommand.Decode(stream, environment)
        ),
        [PlantFieldCommandType] = new CommandRegistryEntry(
            typeof(PlantFieldCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                PlantFieldCommand.Decode(stream, environment)
        ),
        [LoadFarmLayoutsCommandType] = new CommandRegistryEntry(
            typeof(LoadFarmLayoutsServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                LoadFarmLayoutsServerCommand.Decode(stream, environment)
        ),
        [CommandRegistry.CreateRoadsideListingCommandType] = new CommandRegistryEntry(
            typeof(CreateRoadsideListingCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                CreateRoadsideListingCommand.Decode(stream, environment)
        ),
        [MarkBoatSeenCommand.CommandType] = new CommandRegistryEntry(
            typeof(MarkBoatSeenCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                MarkBoatSeenCommand.Decode(stream, environment)
        ),
        [SpawnAmbientAnimalCommand.CommandType] = new CommandRegistryEntry(
            typeof(SpawnAmbientAnimalCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                SpawnAmbientAnimalCommand.Decode(stream, environment)
        ),
        [RequestRoadsidePurchaseCommandType] = new CommandRegistryEntry(
            typeof(RequestRoadsidePurchaseCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                RequestRoadsidePurchaseCommand.Decode(stream, environment)
        ),
        [RoadsideFriendCountServerCommandType] = new CommandRegistryEntry(
            typeof(RoadsideFriendCountServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) => RoadsideFriendCountServerCommand.Decode(stream, environment)
        ),
        [key: 274] = new CommandRegistryEntry(
            typeof(MapGameEventsServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, dataResolver) =>
                MapGameEventsServerCommand.Decode(stream, environment, dataResolver)
        ),
        [key: 355] = new CommandRegistryEntry(
            typeof(ShopEventsServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) => ShopEventsServerCommand.Decode(stream, environment)
        ),
        [key: 672] = new CommandRegistryEntry(
            typeof(CollectAllLettersCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                CollectAllLettersCommand.Decode(stream, environment)
        ),
        [key: 35] = new CommandRegistryEntry(
            typeof(StartTutorialCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                StartTutorialCommand.Decode(stream, environment)
        ),
        [key: 3] = new CommandRegistryEntry(
            typeof(MoveGameObjectByOffsetCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                MoveGameObjectByOffsetCommand.Decode(stream, environment)
        ),
        [key: 124] = new CommandRegistryEntry(
            typeof(MoveGameObjectCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                MoveGameObjectCommand.Decode(stream, environment)
        ),
        [key: 544] = new CommandRegistryEntry(
            typeof(StartHarvestFieldCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                StartHarvestFieldCommand.Decode(stream, environment)
        ),
        [key: 506] = new CommandRegistryEntry(
            typeof(HarvestFieldCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                HarvestFieldCommand.Decode(stream, environment)
        ),
        [key: 657] = new CommandRegistryEntry(
            typeof(HarvestFieldGainCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                HarvestFieldGainCommand.Decode(stream, environment)
        ),
        [key: 247] = new CommandRegistryEntry(
            typeof(Command247),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) => Command247.Decode(stream, environment)
        ),
        [key: 321] = new CommandRegistryEntry(
            typeof(MapGamePawnTaskCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, dataResolver) =>
                MapGamePawnTaskCommand.Decode(stream, environment, dataResolver)
        ),
        [key: 599] = new CommandRegistryEntry(
            typeof(Command599),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) => Command599.Decode(stream, environment)
        ),
        [key: 694] = new CommandRegistryEntry(
            typeof(PostmanStateCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                PostmanStateCommand.Decode(stream, environment)
        ),
        [key: 654] = new CommandRegistryEntry(
            typeof(DecorationEventTutorialCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                DecorationEventTutorialCommand.Decode(stream, environment)
        ),
        [key: 34] = new CommandRegistryEntry(
            typeof(FinishTutorialCommand),
            MessageDirection.Serverbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                FinishTutorialCommand.Decode(stream, environment)
        ),
        [CommandRegistry.CollectRoadsideSaleProceedsCommandType] = new CommandRegistryEntry(
            typeof(CollectRoadsideSaleProceedsCommand),
            MessageDirection.Serverbound,
            BaseFirst: true,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                CollectRoadsideSaleProceedsCommand.Decode(stream, environment)
        ),
        [CommandRegistry.RoadsideSaleServerCommandType] = new CommandRegistryEntry(
            typeof(RoadsideSaleServerCommand),
            MessageDirection.Clientbound,
            BaseFirst: false,
            FieldSchemas: null,
            static (stream, environment, unusedParameter2) =>
                RoadsideSaleServerCommand.Decode(stream, environment)
        ),
    };
}
