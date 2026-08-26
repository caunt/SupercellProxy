using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

internal sealed record ExpansionReadyDataState(DataTableReference ExpansionData, int ReadyBits)
{
    public static ExpansionReadyDataState[] Resolve(
        HomeSnapshot home,
        DataTableResolver dataTableResolver
    )
    {
        const string expansionsFile = "data/expansions.csv";

        if (!dataTableResolver.TryGetTableId(expansionsFile, out var expansionTableId))
            throw new InvalidDataException($"Unable to resolve {expansionsFile}.");

        var states = new List<ExpansionReadyDataState>();

        foreach (var snapshot in home.ExpansionReadyDatas)
        {
            if (snapshot.ReadyBits <= 0)
                continue;

            if (
                !dataTableResolver.TryResolve(snapshot.ExpansionDataGlobalId, out var expansionData)
                || expansionData.TableId != expansionTableId
            )
            {
                continue;
            }

            states.Add(new ExpansionReadyDataState(expansionData, snapshot.ReadyBits));
        }

        return states.ToArray();
    }
}
