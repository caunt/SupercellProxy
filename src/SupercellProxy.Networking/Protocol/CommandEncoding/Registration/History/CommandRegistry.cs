using System.Collections.Frozen;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

public static partial class CommandRegistry
{
    private static readonly FrozenDictionary<int, ProtocolIdHistory> IdChanges = new Dictionary<int, ProtocolIdHistory>
    {
        [SetAngusInteractionCommandType] = new(baselineId: null, [new(Characters.SetAngusInteractionCommand.SinceVersion, Id: 582)]),
        [CollectAngusPearlsCommandType] = new(baselineId: null, [new(Characters.SetAngusInteractionCommand.SinceVersion, Id: 545)]),
        [CollectMolluscCommandType] = new(baselineId: null, [new(Characters.SetAngusInteractionCommand.SinceVersion, Id: 393)]),
        [OpenMolluscCommandType] = new(baselineId: null, [new(Characters.SetAngusInteractionCommand.SinceVersion, Id: 677)]),
        [MarkChronosEventUserInterfaceOpenedCommandType] = new(MarkChronosEventUserInterfaceOpenedCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 514)]),
        [MarkChainOfferSeenCommandType] = new(MarkChainOfferSeenCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 628)]),
        [EnterBoyIntervalRestCommandType] = new(EnterBoyIntervalRestCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 667)]),
        [MarkCountyFairOpenedCommandType] = new(MarkCountyFairOpenedCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 646)]),
        [Boats.MarkBoatSeenCommand.CommandType] = new(Boats.MarkBoatSeenCommand.CommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 560)]),
        [SelectBoatOrderCommandType] = new(SelectBoatOrderCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 585)]),
        [FillBoatCrateCommandType] = new(FillBoatCrateCommandType, [new(Boats.FillBoatCrateCommand.ReorderedFieldsVersion, Id: 551)]),
        [CompleteBoatOrderCommandType] = new(CompleteBoatOrderCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 590)]),
        [MarkBoatTrackCycleIntroSeenCommandType] = new(baselineId: null, [new(new Version(major: 1, minor: 73, build: 81), Id: 627)]),
        [ClaimDecoStickerBookCollectionRewardCommandType] = new(ClaimDecoStickerBookCollectionRewardCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 693)]),
        [StartHarvestFieldCommandType] = new(StartHarvestFieldCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 696)]),
        [HarvestFieldGainCommandType] = new(HarvestFieldGainCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 589)]),
        [HarvestFieldCommandType] = new(HarvestFieldCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 572)]),
        [PostmanStateCommandType] = new(PostmanStateCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 535)]),
        [PlantFieldCommandType] = new(PlantFieldCommandType, [new(CropFields.PlantFieldCommand.ReorderedFieldsVersion, Id: 565)]),
        [BuyCropSeedsCommandType] = new(BuyCropSeedsCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 542)]),
        [StartBuildingProductionCommandType] = new(StartBuildingProductionCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 664)]),
        [CollectBuildingProductCommandType] = new(CollectBuildingProductCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 534)]),
        [CollectAnimalProductCommandType] = new(CollectAnimalProductCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 518)]),
        [FeedLivestockAnimalCommandType] = new(FeedLivestockAnimalCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 521)]),
        [CollectFruitCommandType] = new(CollectFruitCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 511)]),
        [CollectTruckDeliveryRewardsCommandType] = new(CollectTruckDeliveryRewardsCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 528)]),
        [StartTruckDeliveryCommandType] = new(StartTruckDeliveryCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 617)]),
        [ConstructGameObjectCommandType] = new(ConstructGameObjectCommandType, [new(GameObjects.ConstructGameObjectCommand.ReorderedFieldsVersion, Id: 574)]),
        [CatchCreatureCommandType] = new(CatchCreatureCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 563)]),
        [CollectAllLettersCommandType] = new(CollectAllLettersCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 501)]),
        [ActivateMovieTicketCommandType] = new(ActivateMovieTicketCommandType, [new(MovieTickets.ActivateMovieTicketCommand.ReorderedFieldsVersion, Id: 604)]),
        [CollectMovieTicketRewardCommandType] = new(CollectMovieTicketRewardCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 550)]),
        [ClaimMapGameFuelPrizeCommandType] = new(ClaimMapGameFuelPrizeCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 698)]),
        [ClaimDecisionBoxCommandType] = new(ClaimDecisionBoxCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 577)]),
        [ClaimEventBoardSeenRewardCommandType] = new(ClaimEventBoardSeenRewardCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 678)]),
        [CollectCalendarGiftCommandType] = new(CollectCalendarGiftCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 645)]),
        [ClaimChainOfferStepCommandType] = new(ClaimChainOfferStepCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 671)]),
        [SpinMapGameFuelWheelCommandType] = new(SpinMapGameFuelWheelCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 644)]),
        [CollectNeighborhoodDonationCommandType] = new(CollectNeighborhoodDonationCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 616)]),
        [CollectSanctuaryVisitorGiftCommandType] = new(CollectSanctuaryVisitorGiftCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 586)]),
        [ResetWheelCarCommandType] = new(ResetWheelCarCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 686)]),
        [CloseWheelCarCommandType] = new(CloseWheelCarCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 680)]),
        [StartFarmPassSeasonCommandType] = new(StartFarmPassSeasonCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 520)]),
        [StartWheelSpinCommandType] = new(StartWheelSpinCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 670)]),
        [AcknowledgePerformanceProfilingCommandType] = new(AcknowledgePerformanceProfilingCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 622)]),
        [RequestNewspaperCommandType] = new(RequestNewspaperCommandType, [new(Newspapers.RequestNewspaperCommand.ReorderedFieldsVersion, Id: 608)]),
        [CollectRoadsideSaleProceedsCommandType] = new(CollectRoadsideSaleProceedsCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 662)]),
        [RequestRoadsidePurchaseCommandType] = new(RequestRoadsidePurchaseCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 512)]),
        [CreateRoadsideListingCommandType] = new(CreateRoadsideListingCommandType, [new(RoadsideShops.CreateRoadsideListingCommand.ReorderedFieldsVersion, Id: 533)]),
        [AdvertiseRoadsideListingCommandType] = new(AdvertiseRoadsideListingCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 648)]),
        [CancelRoadsideListingCommandType] = new(CancelRoadsideListingCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 592)]),
        [UnlockRoadsideStandCommandType] = new(UnlockRoadsideStandCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 513)]),
        [PurchaseRoadsideAdvertisementCreditCommandType] = new(PurchaseRoadsideAdvertisementCreditCommandType, [new(new Version(major: 1, minor: 73, build: 81), Id: 561)]),
        [VisitedBoatStateServerCommandType] = new(VisitedBoatStateServerCommandType, [new(Boats.VisitedBoatStateServerCommand.ReorderedFieldsVersion, Id: 826)]),
    }.ToFrozenDictionary();

    private static readonly FrozenDictionary<int, int[]> TypesByWireId = IndexWireIds();

    /// <summary>Gets the command's native wire id for the connection's game release.</summary>
    public static int GetId(Command command, Version? gameVersion = null)
    {
        ArgumentNullException.ThrowIfNull(command);

        int? id = IdChanges.TryGetValue(command.Type, out ProtocolIdHistory? history)
            ? history.GetId(gameVersion) : command.Type;

        return id is { } wireId && GetCommandType(wireId, gameVersion) == command.Type
            ? wireId
            : throw new NotSupportedException($"Command {command.Type} is not registered for game version {gameVersion}.");
    }

    private static int GetCommandType(int id, Version? gameVersion)
    {
        if (gameVersion is null)
            return id;

        int? commandType = null;

        if (TypesByWireId.TryGetValue(id, out int[]? candidates))
        {
            foreach (int candidate in candidates)
            {
                if (IdChanges[candidate].GetId(gameVersion) != id)
                    continue;

                if (commandType is not null)
                    throw new InvalidDataException($"Command id {id} has multiple contracts in game version {gameVersion}.");

                commandType = candidate;
            }
        }

        return commandType ?? (IdChanges.ContainsKey(id)
            ? throw new NotSupportedException($"Command id {id} is not registered for game version {gameVersion}.")
            : id);
    }


    private static FrozenDictionary<int, int[]> IndexWireIds()
    {
        Dictionary<int, List<int>> index = [];

        foreach (KeyValuePair<int, ProtocolIdHistory> change in IdChanges)
        {
            foreach (int id in change.Value.Ids.Distinct())
            {
                if (!index.TryGetValue(id, out List<int>? types))
                {
                    types = [];
                    index.Add(id, types);
                }

                types.Add(change.Key);
            }
        }

        return index.ToFrozenDictionary(static pair => pair.Key, static pair => pair.Value.ToArray());
    }
}
