namespace SupercellProxy.Playground.Resources.Csv;

public static class LogicDataTableRegistry
{
    private const string ProductionBuildingsGoodsFile = "data/production_buildings_goods.csv";

    private static readonly IReadOnlyDictionary<int, string> NativeDataTableFiles = new Dictionary<int, string>
    {
        [4] = "data/fields.csv",
        [15] = "data/animal_goods.csv",
        [16] = "data/animal_feed.csv",
        [22] = "data/tools.csv",
        [23] = "data/dairy_goods.csv",
        [24] = "data/bakery_goods.csv",
        [25] = "data/cake_oven_goods.csv",
        [26] = "data/pie_oven_goods.csv",
        [27] = "data/sugar_mill_goods.csv",
        [28] = "data/loom_goods.csv",
        [29] = "data/popcorn_pot_goods.csv",
        [30] = "data/barbecue_grill_goods.csv",
        [38] = "data/collection_tools.csv",
        [60] = "data/juice_press_goods.csv",
        [61] = "data/fruits.csv",
        [64] = "data/mine_goods.csv",
        [65] = "data/smelter_goods.csv",
        [66] = "data/jam_maker_goods.csv",
        [69] = "data/ice_cream_maker_goods.csv",
        [75] = "data/jeweler_goods.csv",
        [82] = "data/cafe_goods.csv",
        [84] = "data/candy_machine_goods.csv",
        [101] = "data/fishing_goods.csv",
        [109] = "data/soup_kitchen_goods.csv",
        [117] = "data/sushi_bar_goods.csv",
        [118] = "data/salad_bar_goods.csv",
        [119] = "data/sauce_mixer_goods.csv",
        [121] = "data/sandwich_bar_goods.csv",
        [129] = "data/smoothie_mixer_goods.csv",
        [145] = "data/sewing_machine_goods.csv",
        [150] = "data/gatherer_nest_goods.csv",
        [152] = "data/honey_extractor_goods.csv",
        [153] = "data/candle_maker_goods.csv",
        [166] = "data/hat_maker_goods.csv",
        [167] = "data/hotdog_stand_goods.csv",
        [168] = "data/pasta_maker_goods.csv",
        [169] = "data/pasta_kitchen_goods.csv",
        [176] = "data/taco_kitchen_goods.csv",
        [177] = "data/tea_stand_goods.csv",
        [184] = "data/limited_collection_tools.csv",
        [202] = "data/flowershop_goods.csv",
        [218] = "data/mapgame_tasks.csv",
        [238] = "data/wok_kitchen_goods.csv",
        [239] = "data/fondue_pot_goods.csv",
        [240] = "data/deep_fryer_goods.csv",
        [241] = "data/bath_kiosk_goods.csv",
        [245] = "data/pop_collection_tools.csv",
        [247] = "data/squirrel_nest_goods.csv",
        [248] = "data/donut_maker_goods.csv",
        [261] = "data/preservation_station_goods.csv",
        [262] = "data/pottery_studio_goods.csv",
        [263] = "data/fudge_shop_goods.csv",
        [267] = "data/yoghurt_maker_goods.csv",
        [268] = "data/cupcake_maker_goods.csv",
        [270] = "data/seasonal_goods.csv",
        [285] = "data/countyfair_dummy_goods.csv",
        [353] = "data/mollusc_goods.csv"
    };

    public static IReadOnlyDictionary<int, string> Create(IEnumerable<Resource> resources)
    {
        var resourcesByFile = resources.ToDictionary(resource => resource.Fingerprint.File, StringComparer.Ordinal);
        var dataTableFiles = new Dictionary<int, string>(NativeDataTableFiles);

        if (!resourcesByFile.TryGetValue(ProductionBuildingsGoodsFile, out var productionBuildingsGoodsResource))
            throw new InvalidOperationException($"Resource {ProductionBuildingsGoodsFile} was not downloaded.");

        if (!productionBuildingsGoodsResource.TryGetTable(out var productionBuildingsGoods))
            throw new InvalidOperationException($"Failed to parse {ProductionBuildingsGoodsFile}.");

        foreach (var entry in productionBuildingsGoods.Entries)
        {
            if (!entry.BaseRow.TryGetValue("CsvName", out var csvNameValue) || csvNameValue is not string csvName ||
                !entry.BaseRow.TryGetValue("ExportNum", out var exportNumValue) || exportNumValue is not int exportNum)
            {
                throw new InvalidDataException($"Invalid entry in {ProductionBuildingsGoodsFile}.");
            }

            if (dataTableFiles.TryGetValue(exportNum, out var existingCsvName) && !existingCsvName.Equals(csvName, StringComparison.Ordinal))
                throw new InvalidDataException($"Data table {exportNum} maps to both {existingCsvName} and {csvName}.");

            dataTableFiles[exportNum] = csvName;
        }

        return dataTableFiles;
    }
}
