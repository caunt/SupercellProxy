namespace SupercellProxy.Playground.Resources.Csv;

public sealed record LogicDataTableReference(int GlobalId, int TableId, int RowIndex, string Name, string File, string FileSha);
