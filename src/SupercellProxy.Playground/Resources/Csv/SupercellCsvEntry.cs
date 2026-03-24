namespace SupercellProxy.Playground.Resources.Csv;

public sealed record SupercellCsvEntry(string Name, IReadOnlyDictionary<string, object?> BaseRow, IReadOnlyList<IReadOnlyDictionary<string, object?>> ContinuationRows, IReadOnlyList<IReadOnlyDictionary<string, object?>> Snapshots);
