using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

internal static class GameObjectDimensionsResolver
{
    private static readonly HashSet<string> DefaultDimensionFiles = new(StringComparer.Ordinal)
    {
        GameAssetFiles.AmbientAnimalSpawners,
        GameAssetFiles.AmbientAnimals,
        GameAssetFiles.Boy,
        GameAssetFiles.BuilderSpawners,
        GameAssetFiles.EasterEggSpawners,
        GameAssetFiles.GathererNests,
        GameAssetFiles.MovieTickets,
        GameAssetFiles.MysteryBoxSpawners,
        GameAssetFiles.PeopleSpawners,
        GameAssetFiles.PhotographerSpawners,
        GameAssetFiles.Postman,
    };

    private static readonly HashSet<string> ZeroDimensionFiles = new(StringComparer.Ordinal)
    {
        GameAssetFiles.Animals,
        GameAssetFiles.Gatherers,
        GameAssetFiles.HelperCharacters,
    };

    public static (int? Width, int? Height) Resolve(
        DataTableReference data,
        DataTableResolver dataTableResolver
    )
    {
        var hasWidth = dataTableResolver.TryResolveInt(data.GlobalId, "TileWidth", out var width);
        var hasHeight = dataTableResolver.TryResolveInt(
            data.GlobalId,
            "TileHeight",
            out var height
        );

        if (hasWidth != hasHeight)
            throw new InvalidDataException(
                $"Incomplete dimensions for {data.File} entry {data.Name}."
            );

        if (hasWidth)
            return (width, height);

        if (ZeroDimensionFiles.Contains(data.File))
            return (0, 0);

        if (DefaultDimensionFiles.Contains(data.File))
            return (-1, -1);

        return (null, null);
    }
}
