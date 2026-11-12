using System.Text.Json.Serialization;
namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>
/// One edit a live table-change event makes to a game CSV while it runs. It either sets one cell, replaces whole columns from a
/// partial CSV, or swaps the entire file.
/// </summary>
public sealed record DataTableChange
{
    /// <summary>
    /// Gets or sets the element of an array column the change sets: the row's line at this index, counting the named line as 0 and its
    /// unnamed continuation lines after it.
    /// </summary>
    /// <value>A zero-based line offset within the entry; <see langword="null"/> or a negative index sets the named line, and an index past
    /// the entry's last continuation line adds empty lines up to it.</value>
    public int? ArrayIndex { get; init; }

    /// <summary>
    /// Gets or sets the header name of the edited column, or several comma-separated names: a single-cell edit writes its value to each of
    /// them, and with <see cref="NewFullCommaSeparatedValues"/> each is copied from the replacement CSV.
    /// </summary>
    /// <value>Names as written in the table's first header row; names the table lacks are skipped. <see langword="null"/> or empty
    /// together with a replacement CSV means the whole file is swapped, and without one the change is refused.</value>
    public string? Column { get; init; }
    /// <summary>
    /// Gets or sets the path of the edited table inside the asset bundle, such as <c>data/animals.csv</c>.
    /// </summary>
    /// <value>A bundle-relative CSV path, empty by default; applying the change fails when the bundle has no CSV file at this path.</value>
    public string File { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the replacement CSV text. Without a <see cref="Column"/> it becomes the whole file; with one, only the named columns
    /// are copied from it, line by line; table lines past the replacement's end keep their own values.
    /// </summary>
    /// <value>The event's <c>NewFullCSV</c> payload; <see langword="null"/> for a single-cell edit, which writes <see cref="NewValue"/>
    /// instead.</value>
    [JsonPropertyName("NewFullCSV")]
    public DataTableReplacement? NewFullCommaSeparatedValues { get; init; }

    /// <summary>
    /// Gets or sets the cell content written by a single-cell edit; unused when a replacement CSV is given.
    /// </summary>
    /// <value>The JSON literal as sent. Only a string is written as given; a missing, <see langword="null"/>, numeric or Boolean value
    /// empties the cell.</value>
    public TableCellValue NewValue { get; init; }

    /// <summary>
    /// Gets or sets the entry name the single-cell edit applies to, as it appears in the table's first column.
    /// </summary>
    /// <value>One name, several comma-separated names that get the same value, or <c>*</c> for every entry; names the table lacks are
    /// skipped. A single-cell edit with a <see langword="null"/> or empty row is refused.</value>
    public string? Row { get; init; }
}
