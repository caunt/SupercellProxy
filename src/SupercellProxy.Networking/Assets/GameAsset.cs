using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

using SupercellProxy.Networking.Assets.Tables;

namespace SupercellProxy.Networking.Assets;

/// <summary>
/// One downloaded game file (a CSV table, JSON document, map or other resource) together with the fingerprint entry that names it and
/// the hash the server published for it.
/// </summary>
public sealed record GameAsset(GameAssetFingerprintEntry Fingerprint, Memory<byte> Content)
{
    // Identity keys keep record copies with changed CSV content separate. Weak keys
    // let session-specific assets and their parsed tables be collected together.
    private static readonly ConditionalWeakTable<GameAsset, Lazy<GameDataTable>> ParsedTables =
        [];

    /// <summary>
    /// The file's bytes decoded as 7-bit text; bytes outside the ASCII range become question marks.
    /// </summary>
    public string AsAscii => AsString(Encoding.ASCII);

    /// <summary>
    /// The file's bytes decoded as UTF-8 text, which is how the game's CSV tables and JSON files are stored.
    /// </summary>
    [JsonPropertyName("AsUtf8")]
    public string AsUnicodeTransformationFormat8 => AsString(Encoding.UTF8);

    /// <summary>
    /// Whether the file name ends in <c>.csv</c>, judged from the fingerprint path alone without reading the content.
    /// </summary>
    public bool IsCsv =>
        Path.GetExtension(Fingerprint.File).Equals(value: ".csv", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Whether the file is a CSV game data table: a column-name row and a column-type row followed by entries.
    /// </summary>
    /// <exception cref="FormatException">The file is a CSV in some other layout.</exception>
    public bool IsStandardCsv => IsCsv && TryGetTable(out _);

    /// <summary>
    /// Decodes the file's bytes as text in the given encoding.
    /// </summary>
    /// <param name="encoding">Character encoding the file was written in.</param>
    /// <returns>The whole file as one string.</returns>
    public string AsString(Encoding encoding)
    {
        ArgumentNullException.ThrowIfNull(encoding);

        return encoding.GetString(Content.Span);
    }

    /// <summary>
    /// Describes the asset by its fingerprint, content size and CSV kind, leaving out the content itself so logs stay short.
    /// </summary>
    public override string ToString()
    {
        return $"{nameof(GameAsset)} {{ Fingerprint: {Fingerprint}, Content: {Content.Length} bytes, IsCsv: {IsCsv}, IsStandardCsv: {IsStandardCsv} }}";
    }

    /// <summary>
    /// Parses a CSV file into a game data table. The parse happens once per asset instance and is shared by later calls.
    /// </summary>
    /// <param name="supercellCsvTable">The parsed table when the file is a CSV; otherwise null.</param>
    /// <returns>False when the file is not a CSV.</returns>
    /// <exception cref="FormatException">The file is a CSV without the column-name and column-type header rows of a game table.</exception>
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
