using System.Globalization;
using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

internal sealed record GathererNestState(
    GameObjectState GameObject,
    DataTableReference GathererData,
    int GathererCount
)
{
    public static GathererNestState[] Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver
    )
    {
        const string gathererNestsFile = "data/gatherer_nests.csv";
        const string gatherersFile = "data/gatherers.csv";

        if (!dataTableResolver.TryGetTableId(gathererNestsFile, out var gathererNestTableId))
            throw new InvalidOperationException(
                $"{gathererNestsFile} is not registered as a native data table."
            );

        if (!dataTableResolver.TryGetTableId(gatherersFile, out var gathererTableId))
            throw new InvalidOperationException(
                $"{gatherersFile} is not registered as a native data table."
            );

        var gatherers = gameObjects
            .Where(gameObject => gameObject.Data.TableId == gathererTableId)
            .ToLookup(static gameObject => gameObject.Data.GlobalId);
        var nests = gameObjects
            .Where(gameObject => gameObject.Data.TableId == gathererNestTableId)
            .Select(gameObject => new
            {
                GameObject = gameObject,
                GathererData = ResolveGathererData(gameObject.Data, dataTableResolver),
            })
            .ToArray();
        var nestCounts = nests
            .GroupBy(static nest => nest.GathererData.GlobalId)
            .ToDictionary(static group => group.Key, static group => group.Count());
        var states = new GathererNestState[nests.Length];

        for (var i = 0; i < nests.Length; i++)
        {
            var nest = nests[i];
            var gathererCount = gatherers[nest.GathererData.GlobalId].Count();
            states[i] = CreateState(
                nest.GameObject,
                nest.GathererData,
                gathererCount,
                nestCounts[nest.GathererData.GlobalId],
                dataTableResolver
            );
        }

        return states;
    }

    private static GathererNestState CreateState(
        GameObjectState gameObject,
        DataTableReference gathererData,
        int gathererCount,
        int nestCount,
        DataTableResolver resolver
    )
    {
        if (gathererCount > 0 && nestCount is not 1)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Cannot associate {gathererCount} {gathererData.Name} gatherers with multiple nests."
                )
            );

        if (
            !resolver.TryResolveInt(
                gameObject.Data.GlobalId,
                "MaxGatherer",
                out var maximumGathererCount
            )
        )
            throw new InvalidDataException(
                $"Gatherer nest {gameObject.Data.Name} has no MaxGatherer value."
            );
        if (gathererCount > maximumGathererCount)
            throw new InvalidDataException(
                $"Gatherer nest {gameObject.Data.Name} exceeds its MaxGatherer value."
            );
        return new GathererNestState(gameObject, gathererData, gathererCount);
    }

    private static DataTableReference ResolveGathererData(
        DataTableReference nestData,
        DataTableResolver dataTableResolver
    )
    {
        const string gatherersFile = "data/gatherers.csv";

        if (
            !dataTableResolver.TryResolveString(nestData.GlobalId, "Gatherer", out var gathererName)
            || !dataTableResolver.TryResolve(gatherersFile, gathererName, out var gathererData)
        )
        {
            throw new InvalidDataException(
                $"Unable to resolve the gatherer for nest {nestData.Name}."
            );
        }

        return gathererData;
    }
}
