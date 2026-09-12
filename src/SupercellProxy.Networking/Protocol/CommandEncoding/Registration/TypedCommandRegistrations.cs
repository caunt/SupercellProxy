using SupercellProxy.Networking.Protocol.Achievements;
using SupercellProxy.Networking.Protocol.Animals;
using SupercellProxy.Networking.Protocol.Boats;
using SupercellProxy.Networking.Protocol.CollectionPayloads;
using SupercellProxy.Networking.Protocol.CropFields;
using SupercellProxy.Networking.Protocol.Events;
using SupercellProxy.Networking.Protocol.Events.Boards;
using SupercellProxy.Networking.Protocol.Events.Chronos;
using SupercellProxy.Networking.Protocol.Events.Decoration;
using SupercellProxy.Networking.Protocol.Events.Tasks;
using SupercellProxy.Networking.Protocol.FarmLayouts;
using SupercellProxy.Networking.Protocol.FarmPass;
using SupercellProxy.Networking.Protocol.Forestry;
using SupercellProxy.Networking.Protocol.GameObjects;
using SupercellProxy.Networking.Protocol.Gifts;
using SupercellProxy.Networking.Protocol.Mail;
using SupercellProxy.Networking.Protocol.MapGame;
using SupercellProxy.Networking.Protocol.MapGame.Events;
using SupercellProxy.Networking.Protocol.MovieTickets;
using SupercellProxy.Networking.Protocol.MysteryBoxes;
using SupercellProxy.Networking.Protocol.Newspapers;
using SupercellProxy.Networking.Protocol.OpaquePayloads;
using SupercellProxy.Networking.Protocol.Orders;
using SupercellProxy.Networking.Protocol.PrizeWheels;
using SupercellProxy.Networking.Protocol.Production;
using SupercellProxy.Networking.Protocol.RoadsideShops;
using SupercellProxy.Networking.Protocol.ScalarPayloads;
using SupercellProxy.Networking.Protocol.ShopEvents;
using SupercellProxy.Networking.Protocol.Town;
using SupercellProxy.Networking.Protocol.Tutorials;

using static SupercellProxy.Networking.Protocol.CommandEncoding.Registration.CommandRegistry;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

internal static class TypedCommandRegistrations
{
    internal static readonly Dictionary<int, CommandRegistryEntry> Entries = new()
    {
        [RoadsidePurchaseServerCommandType] = new CommandRegistryEntry(
            Type: typeof(RoadsidePurchaseServerCommand),
            IsServerCommand: true,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                RoadsidePurchaseServerCommand.Decode(stream, environment)
        ),
        [PassengerServiceCompletionServerCommandType] = new CommandRegistryEntry(
            Type: typeof(PassengerServiceCompletionServerCommand),
            IsServerCommand: true,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                PassengerServiceCompletionServerCommand.Decode(stream, environment)
        ),
        [VisitedBoatHelpRequestServerCommandType] = new CommandRegistryEntry(
            Type: typeof(VisitedBoatHelpRequestServerCommand),
            IsServerCommand: true,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                VisitedBoatHelpRequestServerCommand.Decode(stream, environment)
        ),
        [ClaimFarmPassRewardCommandType] = new CommandRegistryEntry(
            Type: typeof(ClaimFarmPassRewardCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                ClaimFarmPassRewardCommand.Decode(stream, environment)
        ),
        [DismissFarmPassNotificationCommandType] = new CommandRegistryEntry(
            Type: typeof(DismissFarmPassNotificationCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                DismissFarmPassNotificationCommand.Decode(stream, environment)
        ),
        [VisitedBoatDepartureServerCommandType] = new CommandRegistryEntry(
            Type: typeof(VisitedBoatDepartureServerCommand),
            IsServerCommand: true,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                VisitedBoatDepartureServerCommand.Decode(stream, environment)
        ),
        [VisitedBoatStateServerCommandType] = new CommandRegistryEntry(
            Type: typeof(VisitedBoatStateServerCommand),
            IsServerCommand: true,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                VisitedBoatStateServerCommand.Decode(stream, environment)
        ),
        [RoadsideStockServerCommandType] = new CommandRegistryEntry(
            Type: typeof(RoadsideStockServerCommand),
            IsServerCommand: true,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                RoadsideStockServerCommand.Decode(stream, environment)
        ),
        [StartTruckDeliveryCommandType] = new CommandRegistryEntry(
            Type: typeof(StartTruckDeliveryCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                StartTruckDeliveryCommand.Decode(stream, environment)
        ),
        [FillBoatCrateCommandType] = new CommandRegistryEntry(
            Type: typeof(FillBoatCrateCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                FillBoatCrateCommand.Decode(stream, environment)
        ),
        [SelectBoatOrderCommandType] = new CommandRegistryEntry(
            Type: typeof(SelectBoatOrderCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                SelectBoatOrderCommand.Decode(stream, environment)
        ),
        [CompleteConstructionCommandType] = new CommandRegistryEntry(
            Type: typeof(CompleteConstructionCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                CompleteConstructionCommand.Decode(stream, environment)
        ),
        [ActivateMovieTicketCommandType] = new CommandRegistryEntry(
            Type: typeof(ActivateMovieTicketCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                ActivateMovieTicketCommand.Decode(stream, environment)
        ),
        [RecordEventSeenCommandType] = new CommandRegistryEntry(
            Type: typeof(RecordEventSeenCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                RecordEventSeenCommand.Decode(stream, environment)
        ),
        [ClearEventLeaderboardNotificationCommandType] = new CommandRegistryEntry(
            Type: typeof(ClearEventLeaderboardNotificationCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                ClearEventLeaderboardNotificationCommand.Decode(stream, environment)
        ),
        [MarkEventTasksSeenCommandType] = new CommandRegistryEntry(
            Type: typeof(MarkEventTasksSeenCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                MarkEventTasksSeenCommand.Decode(stream, environment)
        ),
        [MarkTaskEventOpenedCommandType] = new CommandRegistryEntry(
            Type: typeof(MarkTaskEventOpenedCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                MarkTaskEventOpenedCommand.Decode(stream, environment)
        ),
        [MarkChronosEventUserInterfaceOpenedCommandType] = new CommandRegistryEntry(
            Type: typeof(MarkChronosEventUserInterfaceOpenedCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                MarkChronosEventUserInterfaceOpenedCommand.Decode(stream, environment)
        ),
        [BuySeasonalCatalogueGiftCommandType] = new CommandRegistryEntry(
            Type: typeof(BuySeasonalCatalogueGiftCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                BuySeasonalCatalogueGiftCommand.Decode(stream, environment)
        ),
        [ClaimDecisionBoxCommandType] = new CommandRegistryEntry(
            Type: typeof(ClaimDecisionBoxCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                ClaimDecisionBoxCommand.Decode(stream, environment)
        ),
        [CollectTruckDeliveryRewardsCommandType] = new CommandRegistryEntry(
            Type: typeof(CollectTruckDeliveryRewardsCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                CollectTruckDeliveryRewardsCommand.Decode(stream, environment)
        ),
        [ConstructGameObjectCommandType] = new CommandRegistryEntry(
            Type: typeof(ConstructGameObjectCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                ConstructGameObjectCommand.Decode(stream, environment)
        ),
        [CollectWheelRewardCommandType] = new CommandRegistryEntry(
            Type: typeof(CollectWheelRewardCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                CollectWheelRewardCommand.Decode(stream, environment)
        ),
        [ClaimAchievementRewardCommandType] = new CommandRegistryEntry(
            Type: typeof(ClaimAchievementRewardCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                ClaimAchievementRewardCommand.Decode(stream, environment)
        ),
        [CollectGiftCommandType] = new CommandRegistryEntry(
            Type: typeof(CollectGiftCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                CollectGiftCommand.Decode(stream, environment)
        ),
        [MarkTruckOrdersSeenCommandType] = new CommandRegistryEntry(
            Type: typeof(MarkTruckOrdersSeenCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                MarkTruckOrdersSeenCommand.Decode(stream, environment)
        ),
        [CollectMysteryBoxRewardCommandType] = new CommandRegistryEntry(
            Type: typeof(CollectMysteryBoxRewardCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                CollectMysteryBoxRewardCommand.Decode(stream, environment)
        ),
        [OpenMysteryBoxCommandType] = new CommandRegistryEntry(
            Type: typeof(OpenMysteryBoxCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                OpenMysteryBoxCommand.Decode(stream, environment)
        ),
        [CheckMysteryBoxLockCommandType] = new CommandRegistryEntry(
            Type: typeof(CheckMysteryBoxLockCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                CheckMysteryBoxLockCommand.Decode(stream, environment)
        ),
        [UpgradeBuildingCommandType] = new CommandRegistryEntry(
            Type: typeof(UpgradeBuildingCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                UpgradeBuildingCommand.Decode(stream, environment)
        ),
        [RecordStorageSignpostRankCommandType] = new CommandRegistryEntry(
            Type: typeof(RecordStorageSignpostRankCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                RecordStorageSignpostRankCommand.Decode(stream, environment)
        ),
        [CollectBuildingProductCommandType] = new CommandRegistryEntry(
            Type: typeof(CollectBuildingProductCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                CollectBuildingProductCommand.Decode(stream, environment)
        ),
        [StartBuildingProductionCommandType] = new CommandRegistryEntry(
            Type: typeof(StartBuildingProductionCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                StartBuildingProductionCommand.Decode(stream, environment)
        ),
        [CompleteForestClearingCommandType] = new CommandRegistryEntry(
            Type: typeof(CompleteForestClearingCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                CompleteForestClearingCommand.Decode(stream, environment)
        ),
        [StartForestClearingCommandType] = new CommandRegistryEntry(
            Type: typeof(StartForestClearingCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                StartForestClearingCommand.Decode(stream, environment)
        ),
        [FeedLivestockAnimalCommandType] = new CommandRegistryEntry(
            Type: typeof(FeedLivestockAnimalCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                FeedLivestockAnimalCommand.Decode(stream, environment)
        ),
        [PurchaseLivestockAnimalCommandType] = new CommandRegistryEntry(
            Type: typeof(PurchaseLivestockAnimalCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                PurchaseLivestockAnimalCommand.Decode(stream, environment)
        ),
        [CollectAnimalProductCommandType] = new CommandRegistryEntry(
            Type: typeof(CollectAnimalProductCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                CollectAnimalProductCommand.Decode(stream, environment)
        ),
        [SelectLivestockAnimalCommandType] = new CommandRegistryEntry(
            Type: typeof(SelectLivestockAnimalCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                SelectLivestockAnimalCommand.Decode(stream, environment)
        ),
        [ClaimEventBoardSeenRewardCommandType] = new CommandRegistryEntry(
            Type: typeof(ClaimEventBoardSeenRewardCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                ClaimEventBoardSeenRewardCommand.Decode(stream, environment)
        ),
        [MarkEventBoardSeenCommandType] = new CommandRegistryEntry(
            Type: typeof(MarkEventBoardSeenCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                MarkEventBoardSeenCommand.Decode(stream, environment)
        ),
        [RequestNewspaperCommandType] = new CommandRegistryEntry(
            Type: typeof(RequestNewspaperCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                RequestNewspaperCommand.Decode(stream, environment)
        ),
        [PlantFieldCommandType] = new CommandRegistryEntry(
            Type: typeof(PlantFieldCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                PlantFieldCommand.Decode(stream, environment)
        ),
        [LoadFarmLayoutsCommandType] = new CommandRegistryEntry(
            Type: typeof(LoadFarmLayoutsServerCommand),
            IsServerCommand: true,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                LoadFarmLayoutsServerCommand.Decode(stream, environment)
        ),
        [CreateRoadsideListingCommand.CommandType] = new CommandRegistryEntry(
            Type: typeof(CreateRoadsideListingCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                CreateRoadsideListingCommand.Decode(stream, environment)
        ),
        [MarkBoatSeenCommand.CommandType] = new CommandRegistryEntry(
            Type: typeof(MarkBoatSeenCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                MarkBoatSeenCommand.Decode(stream, environment)
        ),
        [SpawnAmbientAnimalCommand.CommandType] = new CommandRegistryEntry(
            Type: typeof(SpawnAmbientAnimalCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                SpawnAmbientAnimalCommand.Decode(stream, environment)
        ),
        [RequestRoadsidePurchaseCommandType] = new CommandRegistryEntry(
            Type: typeof(RequestRoadsidePurchaseCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                RequestRoadsidePurchaseCommand.Decode(stream, environment)
        ),
        [key: 210] = new CommandRegistryEntry(
            Type: typeof(ServerCommand210),
            IsServerCommand: true,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) => ServerCommand210.Decode(stream, environment)
        ),
        [key: 274] = new CommandRegistryEntry(
            Type: typeof(MapGameEventsServerCommand),
            IsServerCommand: true,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, dataResolver) =>
                MapGameEventsServerCommand.Decode(stream, environment, dataResolver)
        ),
        [key: 355] = new CommandRegistryEntry(
            Type: typeof(ShopEventsServerCommand),
            IsServerCommand: true,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) => ShopEventsServerCommand.Decode(stream, environment)
        ),
        [key: 672] = new CommandRegistryEntry(
            Type: typeof(CollectAllLettersCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                CollectAllLettersCommand.Decode(stream, environment)
        ),
        [key: 35] = new CommandRegistryEntry(
            Type: typeof(StartTutorialCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                StartTutorialCommand.Decode(stream, environment)
        ),
        [key: 3] = new CommandRegistryEntry(
            Type: typeof(MoveGameObjectByOffsetCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                MoveGameObjectByOffsetCommand.Decode(stream, environment)
        ),
        [key: 124] = new CommandRegistryEntry(
            Type: typeof(MoveGameObjectCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                MoveGameObjectCommand.Decode(stream, environment)
        ),
        [key: 544] = new CommandRegistryEntry(
            Type: typeof(StartHarvestFieldCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                StartHarvestFieldCommand.Decode(stream, environment)
        ),
        [key: 506] = new CommandRegistryEntry(
            Type: typeof(HarvestFieldCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                HarvestFieldCommand.Decode(stream, environment)
        ),
        [key: 657] = new CommandRegistryEntry(
            Type: typeof(HarvestFieldGainCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                HarvestFieldGainCommand.Decode(stream, environment)
        ),
        [key: 247] = new CommandRegistryEntry(
            Type: typeof(Command247),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) => Command247.Decode(stream, environment)
        ),
        [key: 321] = new CommandRegistryEntry(
            Type: typeof(MapGamePawnTaskCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, dataResolver) =>
                MapGamePawnTaskCommand.Decode(stream, environment, dataResolver)
        ),
        [key: 599] = new CommandRegistryEntry(
            Type: typeof(Command599),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) => Command599.Decode(stream, environment)
        ),
        [key: 694] = new CommandRegistryEntry(
            Type: typeof(PostmanStateCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                PostmanStateCommand.Decode(stream, environment)
        ),
        [key: 654] = new CommandRegistryEntry(
            Type: typeof(DecorationEventTutorialCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                DecorationEventTutorialCommand.Decode(stream, environment)
        ),
        [key: 34] = new CommandRegistryEntry(
            Type: typeof(FinishTutorialCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                FinishTutorialCommand.Decode(stream, environment)
        ),
        [key: 649] = new CommandRegistryEntry(
            Type: typeof(RoadsideReceiptCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                RoadsideReceiptCommand.Decode(stream, environment)
        ),
        [key: 375] = new CommandRegistryEntry(
            Type: typeof(RoadsideSaleServerCommand),
            IsServerCommand: true,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, unusedParameter2) =>
                RoadsideSaleServerCommand.Decode(stream, environment)
        ),
    };

}
