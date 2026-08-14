namespace SupercellProxy.Playground.Resources.Csv;

public sealed class LogicDataTableResolver
{
    private const int GlobalIdTableSize = 100000;

    private readonly IReadOnlyDictionary<int, (Resource Resource, SupercellCsvTable Table)> dataTables;

    public LogicDataTableResolver(IEnumerable<Resource> resources)
    {
        var resourcesByFile = resources.ToDictionary(resource => resource.Fingerprint.File, StringComparer.Ordinal);
        var resolvedDataTables = new Dictionary<int, (Resource Resource, SupercellCsvTable Table)>();

        foreach (var (tableId, file) in LogicDataTableRegistry.Create(resources))
        {
            if (!resourcesByFile.TryGetValue(file, out var resource))
                throw new InvalidOperationException($"Resource {file} was not downloaded.");

            if (!resource.TryGetTable(out var table))
                throw new InvalidOperationException($"Failed to parse {file}.");

            resolvedDataTables.Add(tableId, (resource, table));
        }

        dataTables = resolvedDataTables;
    }

    public bool TryResolve(int globalId, out LogicDataTableReference? reference)
    {
        if (globalId < GlobalIdTableSize)
        {
            reference = null;
            return false;
        }

        var tableId = globalId / GlobalIdTableSize;
        var rowIndex = globalId % GlobalIdTableSize;

        if (!dataTables.TryGetValue(tableId, out var dataTable) || rowIndex >= dataTable.Table.Entries.Count)
        {
            reference = null;
            return false;
        }

        var name = dataTable.Table.Entries[rowIndex].Name;

        if (string.IsNullOrWhiteSpace(name))
        {
            reference = null;
            return false;
        }

        reference = new LogicDataTableReference(
            globalId,
            tableId,
            rowIndex,
            name,
            dataTable.Resource.Fingerprint.File,
            dataTable.Resource.Fingerprint.Sha);
        return true;
    }
}
