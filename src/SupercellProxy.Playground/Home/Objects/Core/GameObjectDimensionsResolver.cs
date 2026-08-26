using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

internal static class GameObjectDimensionsResolver
{
    private static readonly IReadOnlySet<string> DefaultDimensionFiles = new HashSet<string>(
        StringComparer.Ordinal
    )
    {
        "data/ambient_animal_spawners.csv",
        "data/ambient_animals.csv",
        "data/boy.csv",
        "data/builder_spawners.csv",
        "data/easter_egg_spawners.csv",
        "data/gatherer_nests.csv",
        "data/movie_tickets.csv",
        "data/mystery_box_spawners.csv",
        "data/people_spawners.csv",
        "data/photographer_spawners.csv",
        "data/postman.csv",
    };

    private static readonly IReadOnlySet<string> ZeroDimensionFiles = new HashSet<string>(
        StringComparer.Ordinal
    )
    {
        "data/animals.csv",
        "data/gatherers.csv",
        "data/helper_characters.csv",
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
