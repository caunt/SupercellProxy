using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

using SupercellProxy.Networking.Assets.Tables;

namespace SupercellProxy.Networking.Assets;

/// <summary>
/// Represents <c language="csharp">GameAsset</c>.
/// </summary>
public sealed record GameAsset(GameAssetFingerprintEntry Fingerprint, Memory<byte> Content)
{
    // Identity keys keep record copies with changed CSV content separate. Weak keys
    // let session-specific assets and their parsed tables be collected together.
    private static readonly ConditionalWeakTable<GameAsset, Lazy<GameDataTable>> ParsedTables =
        [];

    /// <summary>
    /// Gets the <c language="csharp">AsAscii</c> value.
    /// </summary>
    public string AsAscii => AsString(Encoding.ASCII);

    /// <summary>
    /// Gets the <c language="csharp">AsUtf8</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("AsUtf8")]
    public string AsUnicodeTransformationFormat8 => AsString(Encoding.UTF8);

    /// <summary>
    /// Gets the <c language="csharp">IsCsv</c> value.
    /// </summary>
    public bool IsCsv =>
        Path.GetExtension(Fingerprint.File).Equals(value: ".csv", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the <c language="csharp">IsStandardCsv</c> value.
    /// </summary>
    public bool IsStandardCsv => IsCsv && TryGetTable(out _);

    /// <summary>
    /// Executes the <c language="csharp">AsString</c> operation.
    /// </summary>
    public string AsString(Encoding encoding)
    {
        ArgumentNullException.ThrowIfNull(encoding);

        return encoding.GetString(Content.Span);
    }

    /// <summary>
    /// Executes the <c language="csharp">ToString</c> operation.
    /// </summary>
    public override string ToString()
    {
        return $"{nameof(GameAsset)} {{ Fingerprint: {Fingerprint}, Content: {Content.Length} bytes, IsCsv: {IsCsv}, IsStandardCsv: {IsStandardCsv} }}";
    }

    /// <summary>
    /// Attempts the <c language="csharp">GetTable</c> operation.
    /// </summary>
    public bool TryGetTable([MaybeNullWhen(false)] out GameDataTable supercellCsvTable)
    {
        supercellCsvTable = null;

        if (IsCsv)
        {
            supercellCsvTable = ParsedTables
                .GetValue(this, static asset => new Lazy<GameDataTable>(() => GameDataTableParser.Parse(asset.AsUnicodeTransformationFormat8)))
                .Value;
        }

        return supercellCsvTable is not null;
    }
}
