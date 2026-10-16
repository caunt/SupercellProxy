using System.Text.Json.Serialization;
namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>
/// Where a game data entry lives: its global id and the table, row and name it resolves to, plus the file version it was read from.
/// </summary>
/// <param name="GlobalId">Number the game uses for the entry on the wire and in saves: table id times 100000 plus row index.</param>
/// <param name="TableId">Native number of the entry's table, such as 13 for animals.</param>
/// <param name="RowIndex">Position of the entry among the table's named lines, starting at 0.</param>
/// <param name="Name">Entry name from the table's first column, such as <c>Chicken</c>.</param>
/// <param name="File">Path of the table inside the asset bundle.</param>
/// <param name="FileSha">Content hash of the table file the entry was read from.</param>
public sealed record DataTableReference(
    [property: JsonPropertyName("GlobalId")] int GlobalId,
    [property: JsonPropertyName("TableId")] int TableId,
    int RowIndex,
    string Name,
    string File,
    string FileSha
);
