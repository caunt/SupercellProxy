using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>A concrete table-cell literal that preserves its original representation.</summary>
[StructLayout(LayoutKind.Auto)]
[JsonConverter(typeof(TableCellValueConverter))]
public readonly record struct TableCellValue
{
    internal TableCellValue(TableCellValueKind kind, string? text, EncodedDocumentValue? structured = null)
    {
        Kind = kind;
        Text = text;
        Structured = structured;
    }

    /// <summary>Gets the literal kind.</summary>
    /// <value>Which JSON token the change carried; <see cref="TableCellValueKind.Missing"/> for a default instance, when the change had
    /// no value at all.</value>
    public TableCellValueKind Kind { get; }

    /// <summary>Gets the decoded text or exact numeric/boolean literal.</summary>
    /// <value>The unescaped string for <see cref="TableCellValueKind.Text"/>, the number exactly as written (such as <c>1.50</c>) for
    /// <see cref="TableCellValueKind.Number"/>, the lowercase word <see langword="true"/> or <see langword="false"/> for
    /// <see cref="TableCellValueKind.Boolean"/>, and <see langword="null"/> for every other kind.</value>
    public string? Text { get; }

    internal EncodedDocumentValue? Structured { get; }

    /// <summary>
    /// Converts the literal to CSV cell text: only text is kept, while a missing, <see langword="null"/>, numeric or boolean
    /// value empties the cell.
    /// </summary>
    /// <returns>The <see cref="Text"/> of a text literal (empty when it is <see langword="null"/>); an empty string for every other
    /// supported kind.</returns>
    /// <exception cref="NotSupportedException"><see cref="Kind"/> is <see cref="TableCellValueKind.Structured"/>, such as a localized
    /// value, which is not implemented.</exception>
    /// <exception cref="InvalidOperationException"><see cref="Kind"/> holds a value outside the defined <see cref="TableCellValueKind"/>
    /// members.</exception>
    public string ToCellText()
    {
        return Kind switch
        {
            TableCellValueKind.Text => Text ?? string.Empty,
            TableCellValueKind.Missing or TableCellValueKind.Null or TableCellValueKind.Number or TableCellValueKind.Boolean => string.Empty,
            TableCellValueKind.Structured => throw new NotSupportedException(message: "Localized table-cell changes are not implemented."),
            _ => throw new InvalidOperationException(message: "Unknown table-cell literal kind."),
        };
    }
}
