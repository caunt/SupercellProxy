using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

internal sealed record ExpansionReadyDataState(DataTableReference ExpansionData, int ReadyBits)
{
    public static ExpansionReadyDataState[] Resolve(
        HomeSnapshot home,
        DataTableResolver dataTableResolver
    )
    {
        if (!dataTableResolver.TryGetTableId(GameAssetFiles.Expansions, out var expansionTableId))
            throw new InvalidDataException($"Unable to resolve {GameAssetFiles.Expansions}.");

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
