namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>
/// A parsed game CSV table.
/// </summary>
/// <param name="Headers">Column names from the first row, in file order.</param>
/// <param name="Types">Declared column types from the second row, such as <c>int</c>, <c>String</c> or <c>Boolean</c>.</param>
/// <param name="Entries">Entries in file order; their position is the row index used in global ids.</param>
public sealed record GameDataTable(IReadOnlyList<string> Headers, IReadOnlyList<string> Types, IReadOnlyList<GameDataTableEntry> Entries);
