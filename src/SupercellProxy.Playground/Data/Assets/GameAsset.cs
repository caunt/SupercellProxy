using System.Diagnostics.CodeAnalysis;
using System.Text;
using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Data.Assets;

/// <summary>
/// Represents <c>GameAsset</c>.
/// </summary>
public record GameAsset(GameAssetFingerprintEntry Fingerprint, Memory<byte> Content)
{
    /// <summary>
    /// Gets the <c>IsCsv</c> value.
    /// </summary>
    public bool IsCsv =>
        Path.GetExtension(Fingerprint.File).Equals(".csv", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the <c>IsStandardCsv</c> value.
    /// </summary>
    public bool IsStandardCsv => IsCsv && TryGetTable(out _);

    /// <summary>
    /// Gets the <c>AsUtf8</c> value.
    /// </summary>
    public string AsUtf8 => AsString(Encoding.UTF8);

    /// <summary>
    /// Gets the <c>AsAscii</c> value.
    /// </summary>
    public string AsAscii => AsString(Encoding.ASCII);

    /// <summary>
    /// Attempts the <c>GetTable</c> operation.
    /// </summary>
    public bool TryGetTable([MaybeNullWhen(false)] out GameDataTable supercellCsvTable)
    {
        supercellCsvTable = null;

        if (IsCsv)
            supercellCsvTable = GameDataTableParser.Parse(AsUtf8);

        return supercellCsvTable is not null;
    }

    /// <summary>
    /// Executes the <c>AsString</c> operation.
    /// </summary>
    public string AsString(Encoding encoding)
    {
        return encoding.GetString(Content.Span);
    }

    /// <summary>
    /// Executes the <c>ToString</c> operation.
    /// </summary>
    public override string ToString()
    {
        return $"{nameof(GameAsset)} {{ Fingerprint: {Fingerprint}, Content: {Content.Length} bytes, IsCsv: {IsCsv}, IsStandardCsv: {IsStandardCsv} }}";
    }
}
