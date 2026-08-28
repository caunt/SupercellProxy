using System.Globalization;
using SupercellProxy.Playground.Data.Assets;

namespace SupercellProxy.Playground.Data.Tables;

/// <summary>
/// Represents <c language="csharp">DataTableRegistry</c>.
/// </summary>
internal static class DataTableRegistry
{
    private static readonly IReadOnlyDictionary<int, string> NativeDataTableFiles = new Dictionary<
        int,
        string
    >
    {
        [2] = GameAssetFiles.Cars,
        [3] = GameAssetFiles.Decorations,
        [4] = GameAssetFiles.Fields,
        [5] = GameAssetFiles.Forests,
        [6] = GameAssetFiles.ProcessingBuildings,
        [10] = GameAssetFiles.Houses,
        [11] = GameAssetFiles.AnimalHabitats,
        [12] = GameAssetFiles.AnimalHabitatPieces,
        [13] = GameAssetFiles.Animals,
        [14] = GameAssetFiles.Warehouses,
        [15] = GameAssetFiles.AnimalGoods,
        [16] = GameAssetFiles.AnimalFeed,
        [17] = GameAssetFiles.Boosters,
        [18] = GameAssetFiles.Silos,
        [19] = GameAssetFiles.Money,
        [21] = GameAssetFiles.ConstructionBuildings,
        [22] = GameAssetFiles.Tools,
        [23] = GameAssetFiles.DairyGoods,
        [24] = GameAssetFiles.BakeryGoods,
        [25] = GameAssetFiles.CakeOvenGoods,
        [26] = GameAssetFiles.PieOvenGoods,
        [27] = GameAssetFiles.SugarMillGoods,
        [28] = GameAssetFiles.LoomGoods,
        [29] = GameAssetFiles.PopcornPotGoods,
        [30] = GameAssetFiles.BarbecueGrillGoods,
        [32] = GameAssetFiles.OrderTables,
        [34] = GameAssetFiles.Orders,
        [35] = GameAssetFiles.RoadsideShop,
        [36] = GameAssetFiles.Tutorials,
        [38] = GameAssetFiles.CollectionTools,
        [40] = GameAssetFiles.Mailboxes,
        [45] = GameAssetFiles.AmbientAnimals,
        [46] = GameAssetFiles.AmbientAnimalSpawners,
        [48] = GameAssetFiles.DecoFences,
        [49] = GameAssetFiles.People,
        [50] = GameAssetFiles.PeopleSpawners,
        [53] = GameAssetFiles.MysteryBoxes,
        [54] = GameAssetFiles.MysteryBoxSpawners,
        [56] = GameAssetFiles.Docks,
        [58] = GameAssetFiles.Boats,
        [60] = GameAssetFiles.JuicePressGoods,
        [61] = GameAssetFiles.Fruits,
        [62] = GameAssetFiles.FruitTrees,
        [63] = GameAssetFiles.Mines,
        [64] = GameAssetFiles.MineGoods,
        [65] = GameAssetFiles.SmelterGoods,
        [66] = GameAssetFiles.JamMakerGoods,
        [69] = GameAssetFiles.IceCreamMakerGoods,
        [70] = GameAssetFiles.Boy,
        [71] = GameAssetFiles.BoyBox,
        [73] = GameAssetFiles.MovieTickets,
        [74] = GameAssetFiles.WheelCars,
        [75] = GameAssetFiles.JewelerGoods,
        [77] = GameAssetFiles.Vouchers,
        [80] = GameAssetFiles.Expansions,
        [82] = GameAssetFiles.CafeGoods,
        [84] = GameAssetFiles.CandyMachineGoods,
        [88] = GameAssetFiles.ScoreBoards,
        [89] = GameAssetFiles.Balloons,
        [90] = GameAssetFiles.Postman,
        [91] = GameAssetFiles.GiftMailbox,
        [95] = GameAssetFiles.FishingBoat,
        [100] = GameAssetFiles.BaitMaker,
        [101] = GameAssetFiles.FishingGoods,
        [103] = GameAssetFiles.EventBoard,
        [112] = GameAssetFiles.EasterEggSpawners,
        [109] = GameAssetFiles.SoupKitchenGoods,
        [117] = GameAssetFiles.SushiBarGoods,
        [118] = GameAssetFiles.SaladBarGoods,
        [119] = GameAssetFiles.SauceMixerGoods,
        [121] = GameAssetFiles.SandwichBarGoods,
        [124] = GameAssetFiles.NeighborhoodBuildings,
        [129] = GameAssetFiles.SmoothieMixerGoods,
        [130] = GameAssetFiles.LobsterPool,
        [145] = GameAssetFiles.SewingMachineGoods,
        [146] = GameAssetFiles.GathererHabitats,
        [147] = GameAssetFiles.GathererNests,
        [148] = GameAssetFiles.GathererMines,
        [149] = GameAssetFiles.Gatherers,
        [150] = GameAssetFiles.GathererNestGoods,
        [152] = GameAssetFiles.HoneyExtractorGoods,
        [153] = GameAssetFiles.CandleMakerGoods,
        [163] = GameAssetFiles.Derby,
        [166] = GameAssetFiles.HatMakerGoods,
        [167] = GameAssetFiles.HotdogStandGoods,
        [168] = GameAssetFiles.PastaMakerGoods,
        [169] = GameAssetFiles.PastaKitchenGoods,
        [170] = GameAssetFiles.DecoDitches,
        [174] = GameAssetFiles.NeighborhoodDonationsArea,
        [175] = GameAssetFiles.NeighborhoodRequestBoard,
        [176] = GameAssetFiles.TacoKitchenGoods,
        [177] = GameAssetFiles.TeaStandGoods,
        [178] = GameAssetFiles.HelperHouse,
        [181] = GameAssetFiles.HelperCharacters,
        [182] = GameAssetFiles.HelperCollectionArea,
        [184] = GameAssetFiles.LimitedCollectionTools,
        [202] = GameAssetFiles.FlowerShopGoods,
        [204] = GameAssetFiles.RenovatorStand,
        [208] = GameAssetFiles.BuilderSpawners,
        [218] = GameAssetFiles.MapGameTasks,
        [223] = GameAssetFiles.MapGameStand,
        [238] = GameAssetFiles.WokKitchenGoods,
        [239] = GameAssetFiles.FonduePotGoods,
        [240] = GameAssetFiles.DeepFryerGoods,
        [241] = GameAssetFiles.BathKioskGoods,
        [245] = GameAssetFiles.PopCollectionTools,
        [247] = GameAssetFiles.SquirrelNestGoods,
        [248] = GameAssetFiles.DonutMakerGoods,
        [252] = GameAssetFiles.FarmPassPerks,
        [256] = GameAssetFiles.FarmPassBuilding,
        [261] = GameAssetFiles.PreservationStationGoods,
        [262] = GameAssetFiles.PotteryStudioGoods,
        [263] = GameAssetFiles.FudgeShopGoods,
        [267] = GameAssetFiles.YoghurtMakerGoods,
        [268] = GameAssetFiles.CupcakeMakerGoods,
        [270] = GameAssetFiles.SeasonalGoods,
        [285] = GameAssetFiles.CountyFairDummyGoods,
        [293] = GameAssetFiles.DecoEventBuilding,
        [296] = GameAssetFiles.PhotographerSpawners,
        [297] = GameAssetFiles.Photographer,
        [321] = GameAssetFiles.NeighborhoodObjects,
        [353] = GameAssetFiles.MolluscGoods,
    };

    /// <summary>
    /// Creates a <c language="csharp">DataTableRegistry</c> from the supplied data.
    /// </summary>
    public static IReadOnlyDictionary<int, string> Create(IEnumerable<GameAsset> resources)
    {
        var resourcesByFile = resources.ToDictionary(
            static resource => resource.Fingerprint.File,
            StringComparer.Ordinal
        );
        var dataTableFiles = new Dictionary<int, string>(NativeDataTableFiles);

        if (
            !resourcesByFile.TryGetValue(
                GameAssetFiles.ProductionBuildingsGoods,
                out var productionBuildingsGoodsResource
            )
        )
            throw new InvalidOperationException(
                $"GameAsset {GameAssetFiles.ProductionBuildingsGoods} was not downloaded."
            );

        if (!productionBuildingsGoodsResource.TryGetTable(out var productionBuildingsGoods))
            throw new InvalidOperationException(
                $"Failed to parse {GameAssetFiles.ProductionBuildingsGoods}."
            );

        foreach (var entry in productionBuildingsGoods.Entries)
        {
            if (
                !entry.BaseRow.TryGetValue("CsvName", out var csvNameValue)
                || csvNameValue is not string csvName
                || !entry.BaseRow.TryGetValue("ExportNum", out var exportNumValue)
                || exportNumValue is not int exportNum
            )
            {
                throw new InvalidDataException(
                    $"Invalid entry in {GameAssetFiles.ProductionBuildingsGoods}."
                );
            }

            if (
                dataTableFiles.TryGetValue(exportNum, out var existingCsvName)
                && !existingCsvName.Equals(csvName, StringComparison.Ordinal)
            )
                throw new InvalidDataException(
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"Data table {exportNum} maps to both {existingCsvName} and {csvName}."
                    )
                );

            dataTableFiles[exportNum] = csvName;
        }

        return dataTableFiles;
    }
}
