namespace SupercellProxy.Keys.Models;

internal sealed record ExistingKeyEntry(string Version, string Key, int LineIndex, IReadOnlyList<string> Cells);
