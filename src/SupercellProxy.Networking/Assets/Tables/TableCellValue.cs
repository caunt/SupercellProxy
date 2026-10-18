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
    public TableCellValueKind Kind { get; }

    /// <summary>Gets the decoded text or exact numeric/boolean literal.</summary>
    public string? Text { get; }

    internal EncodedDocumentValue? Structured { get; }

    /// <summary>
    /// Converts the literal to CSV cell text the way the game reads it: only text is kept, while a missing, null, numeric or boolean
    /// value empties the cell.
    /// </summary>
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
