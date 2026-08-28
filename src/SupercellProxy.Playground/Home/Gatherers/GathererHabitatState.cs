using System.Globalization;
using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

internal sealed record GathererHabitatState(
    GameObjectState GameObject,
    DataTableReference NestData,
    int NestCount
)
{
    public static GathererHabitatState[] Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver
    )
    {
        if (
            !dataTableResolver.TryGetTableId(
                GameAssetFiles.GathererHabitats,
                out var gathererHabitatTableId
            )
        )
            throw new InvalidOperationException(
                $"{GameAssetFiles.GathererHabitats} is not registered as a native data table."
            );

        if (
            !dataTableResolver.TryGetTableId(
                GameAssetFiles.GathererNests,
                out var gathererNestTableId
            )
        )
            throw new InvalidOperationException(
                $"{GameAssetFiles.GathererNests} is not registered as a native data table."
            );

        var nests = gameObjects
            .Where(gameObject => gameObject.Data.TableId == gathererNestTableId)
            .ToLookup(static gameObject => gameObject.Data.GlobalId);
        var habitats = gameObjects
            .Where(gameObject => gameObject.Data.TableId == gathererHabitatTableId)
            .Select(gameObject => new
            {
                GameObject = gameObject,
                NestData = ResolveNestData(gameObject.Data, dataTableResolver),
            })
            .ToArray();
        var habitatCounts = habitats
            .GroupBy(static habitat => habitat.NestData.GlobalId)
            .ToDictionary(static group => group.Key, static group => group.Count());
        var states = new GathererHabitatState[habitats.Length];

        for (var i = 0; i < habitats.Length; i++)
        {
            var habitat = habitats[i];
            var nestCount = nests[habitat.NestData.GlobalId].Count();
            states[i] = CreateState(
                habitat.GameObject,
                habitat.NestData,
                nestCount,
                habitatCounts[habitat.NestData.GlobalId],
                dataTableResolver
            );
        }

        return states;
    }

    private static GathererHabitatState CreateState(
        GameObjectState gameObject,
        DataTableReference nestData,
        int nestCount,
        int habitatCount,
        DataTableResolver resolver
    )
    {
        if (nestCount > 0 && habitatCount is not 1)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Cannot associate {nestCount} {nestData.Name} nests with multiple habitats."
                )
            );

        if (
            !resolver.TryResolveInt(
                gameObject.Data.GlobalId,
                "NestCount",
                gameObject.Snapshot.Rank - 1,
                out var maximumNestCount
            )
        )
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Gatherer habitat {gameObject.Data.Name} has no NestCount value for rank {gameObject.Snapshot.Rank}."
                )
            );

        if (nestCount > maximumNestCount)
            throw new InvalidDataException(
                $"Gatherer habitat {gameObject.Data.Name} exceeds its NestCount value."
            );
        return new GathererHabitatState(gameObject, nestData, nestCount);
    }

    private static DataTableReference ResolveNestData(
        DataTableReference habitatData,
        DataTableResolver dataTableResolver
    )
    {
        if (
            !dataTableResolver.TryResolveString(habitatData.GlobalId, "Nest", out var nestName)
            || !dataTableResolver.TryResolve(
                GameAssetFiles.GathererNests,
                nestName,
                out var nestData
            )
        )
        {
            throw new InvalidDataException(
                $"Unable to resolve the nest for gatherer habitat {habitatData.Name}."
            );
        }

        return nestData;
    }
}
