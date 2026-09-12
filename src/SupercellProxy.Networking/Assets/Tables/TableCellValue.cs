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

    /// <summary>Converts a supported scalar literal to CSV cell text.</summary>
    public string ToCellText()
    {
        return Kind switch
        {
            TableCellValueKind.Null => string.Empty,
            TableCellValueKind.Text or TableCellValueKind.Number or TableCellValueKind.Boolean => Text ?? string.Empty,
            TableCellValueKind.Missing or TableCellValueKind.Structured => throw new NotSupportedException(message: "Structured table-cell changes are not implemented."),
            _ => throw new InvalidOperationException(message: "Unknown table-cell literal kind."),
        };
    }
}
