using System.Text.Json.Serialization;
namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>
/// One edit a live table-change event makes to a game CSV while it runs. It either sets one cell, replaces whole columns from a
/// partial CSV, or swaps the entire file.
/// </summary>
public sealed record DataTableChange
{
    /// <summary>
    /// Gets the element of an array column the change sets: the row's line at this index, counting the named line as 0 and its unnamed
    /// continuation lines after it. Null or a negative index sets the named line.
    /// </summary>
    public int? ArrayIndex { get; init; }

    /// <summary>
    /// Header name of the edited column. With <see cref="NewFullCommaSeparatedValues"/> it may list several comma-separated columns,
    /// all copied from the replacement CSV. Empty together with a replacement CSV means the whole file is swapped.
    /// </summary>
    public string? Column { get; init; }
    /// <summary>
    /// Path of the edited table inside the asset bundle, such as <c>data/animals.csv</c>.
    /// </summary>
    public string File { get; init; } = string.Empty;

    /// <summary>
    /// Replacement CSV text. Without a <see cref="Column"/> it becomes the whole file; with one, only the named columns are copied
    /// from it, row by row, so it must have exactly as many rows as the table.
    /// </summary>
    [JsonPropertyName("NewFullCSV")]
    public DataTableReplacement? NewFullCommaSeparatedValues { get; init; }

    /// <summary>
    /// Cell content written by a single-cell edit; unused when a replacement CSV is given.
    /// </summary>
    public TableCellValue NewValue { get; init; }

    /// <summary>
    /// Entry name the single-cell edit applies to, as it appears in the table's first column. Several comma-separated names get the
    /// same value.
    /// </summary>
    public string? Row { get; init; }
}
