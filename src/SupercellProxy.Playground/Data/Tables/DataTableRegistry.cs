using System.Globalization;
using SupercellProxy.Playground.Data.Assets;

namespace SupercellProxy.Playground.Data.Tables;

/// <summary>
/// Represents <c>DataTableRegistry</c>.
/// </summary>
public static class DataTableRegistry
{
    private const string ProductionBuildingsGoodsFile = "data/production_buildings_goods.csv";

    private static readonly IReadOnlyDictionary<int, string> NativeDataTableFiles = new Dictionary<
        int,
        string
    >
    {
        [2] = "data/cars.csv",
        [3] = "data/decorations.csv",
        [4] = "data/fields.csv",
        [5] = "data/forests.csv",
        [6] = "data/processing_buildings.csv",
        [10] = "data/houses.csv",
        [11] = "data/animal_habitats.csv",
        [12] = "data/animal_habitat_pieces.csv",
        [13] = "data/animals.csv",
        [14] = "data/warehouses.csv",
        [15] = "data/animal_goods.csv",
        [16] = "data/animal_feed.csv",
        [17] = "data/boosters.csv",
        [18] = "data/silos.csv",
        [19] = "data/money.csv",
        [21] = "data/construction_buildings.csv",
        [22] = "data/tools.csv",
        [23] = "data/dairy_goods.csv",
        [24] = "data/bakery_goods.csv",
        [25] = "data/cake_oven_goods.csv",
        [26] = "data/pie_oven_goods.csv",
        [27] = "data/sugar_mill_goods.csv",
        [28] = "data/loom_goods.csv",
        [29] = "data/popcorn_pot_goods.csv",
        [30] = "data/barbecue_grill_goods.csv",
        [32] = "data/order_tables.csv",
        [34] = "data/orders.csv",
        [35] = "data/roadside_shop.csv",
        [36] = "data/tutorials.csv",
        [38] = "data/collection_tools.csv",
        [40] = "data/mailboxes.csv",
        [45] = "data/ambient_animals.csv",
        [46] = "data/ambient_animal_spawners.csv",
        [48] = "data/deco_fences.csv",
        [49] = "data/people.csv",
        [50] = "data/people_spawners.csv",
        [53] = "data/mystery_boxes.csv",
        [54] = "data/mystery_box_spawners.csv",
        [56] = "data/docks.csv",
        [58] = "data/boats.csv",
        [60] = "data/juice_press_goods.csv",
        [61] = "data/fruits.csv",
        [62] = "data/fruit_trees.csv",
        [63] = "data/mines.csv",
        [64] = "data/mine_goods.csv",
        [65] = "data/smelter_goods.csv",
        [66] = "data/jam_maker_goods.csv",
        [69] = "data/ice_cream_maker_goods.csv",
        [70] = "data/boy.csv",
        [71] = "data/boy_box.csv",
        [73] = "data/movie_tickets.csv",
        [74] = "data/wheel_cars.csv",
        [75] = "data/jeweler_goods.csv",
        [77] = "data/vouchers.csv",
        [80] = "data/expansions.csv",
        [82] = "data/cafe_goods.csv",
        [84] = "data/candy_machine_goods.csv",
        [88] = "data/score_boards.csv",
        [89] = "data/balloons.csv",
        [90] = "data/postman.csv",
        [91] = "data/giftmailbox.csv",
        [95] = "data/fishing_boat.csv",
        [100] = "data/bait_maker.csv",
        [101] = "data/fishing_goods.csv",
        [103] = "data/eventboard.csv",
        [112] = "data/easter_egg_spawners.csv",
        [109] = "data/soup_kitchen_goods.csv",
        [117] = "data/sushi_bar_goods.csv",
        [118] = "data/salad_bar_goods.csv",
        [119] = "data/sauce_mixer_goods.csv",
        [121] = "data/sandwich_bar_goods.csv",
        [124] = "data/neighborhood_buildings.csv",
        [129] = "data/smoothie_mixer_goods.csv",
        [130] = "data/lobster_pool.csv",
        [145] = "data/sewing_machine_goods.csv",
        [146] = "data/gatherer_habitats.csv",
        [147] = "data/gatherer_nests.csv",
        [148] = "data/gatherer_mines.csv",
        [149] = "data/gatherers.csv",
        [150] = "data/gatherer_nest_goods.csv",
        [152] = "data/honey_extractor_goods.csv",
        [153] = "data/candle_maker_goods.csv",
        [163] = "data/derby.csv",
        [166] = "data/hat_maker_goods.csv",
        [167] = "data/hotdog_stand_goods.csv",
        [168] = "data/pasta_maker_goods.csv",
        [169] = "data/pasta_kitchen_goods.csv",
        [170] = "data/deco_ditches.csv",
        [174] = "data/neighborhood_donations_area.csv",
        [175] = "data/neighborhood_request_board.csv",
        [176] = "data/taco_kitchen_goods.csv",
        [177] = "data/tea_stand_goods.csv",
        [178] = "data/helper_house.csv",
        [181] = "data/helper_characters.csv",
        [182] = "data/helper_collection_area.csv",
        [184] = "data/limited_collection_tools.csv",
        [202] = "data/flowershop_goods.csv",
        [204] = "data/renovator_stand.csv",
        [208] = "data/builder_spawners.csv",
        [218] = "data/mapgame_tasks.csv",
        [223] = "data/mapgame_stand.csv",
        [238] = "data/wok_kitchen_goods.csv",
        [239] = "data/fondue_pot_goods.csv",
        [240] = "data/deep_fryer_goods.csv",
        [241] = "data/bath_kiosk_goods.csv",
        [245] = "data/pop_collection_tools.csv",
        [247] = "data/squirrel_nest_goods.csv",
        [248] = "data/donut_maker_goods.csv",
        [252] = "data/farm_pass_perks.csv",
        [256] = "data/farm_pass_building.csv",
        [261] = "data/preservation_station_goods.csv",
        [262] = "data/pottery_studio_goods.csv",
        [263] = "data/fudge_shop_goods.csv",
        [267] = "data/yoghurt_maker_goods.csv",
        [268] = "data/cupcake_maker_goods.csv",
        [270] = "data/seasonal_goods.csv",
        [285] = "data/countyfair_dummy_goods.csv",
        [296] = "data/photographer_spawners.csv",
        [297] = "data/photographer.csv",
        [321] = "data/neighborhood_objects.csv",
        [353] = "data/mollusc_goods.csv",
    };

    /// <summary>
    /// Creates a <c>DataTableRegistry</c> from the supplied data.
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
                ProductionBuildingsGoodsFile,
                out var productionBuildingsGoodsResource
            )
        )
            throw new InvalidOperationException(
                $"GameAsset {ProductionBuildingsGoodsFile} was not downloaded."
            );

        if (!productionBuildingsGoodsResource.TryGetTable(out var productionBuildingsGoods))
            throw new InvalidOperationException($"Failed to parse {ProductionBuildingsGoodsFile}.");

        foreach (var entry in productionBuildingsGoods.Entries)
        {
            if (
                !entry.BaseRow.TryGetValue("CsvName", out var csvNameValue)
                || csvNameValue is not string csvName
                || !entry.BaseRow.TryGetValue("ExportNum", out var exportNumValue)
                || exportNumValue is not int exportNum
            )
            {
                throw new InvalidDataException($"Invalid entry in {ProductionBuildingsGoodsFile}.");
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
