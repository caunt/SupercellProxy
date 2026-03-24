namespace SupercellProxy.Playground.Resources.Csv;

public sealed record SupercellCsvTable(IReadOnlyList<string> Headers, IReadOnlyList<string> Types, IReadOnlyList<SupercellCsvEntry> Entries);
