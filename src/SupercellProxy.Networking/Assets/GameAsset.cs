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
/// <param name="Fingerprint">Manifest entry for the file: its bundle-relative path (for example <c>data/animals.csv</c>), which decides
/// <see cref="IsCsv"/>, and the content hash; assets loaded from a plain local directory carry an empty hash.</param>
/// <param name="Content">The file's bytes exactly as stored on disk or served by the asset server; table parsing and the text
/// properties read them without copying.</param>
public sealed record GameAsset(GameAssetFingerprintEntry Fingerprint, Memory<byte> Content)
{
    // Identity keys keep record copies with changed CSV content separate. Weak keys
    // let session-specific assets and their parsed tables be collected together.
    private static readonly ConditionalWeakTable<GameAsset, Lazy<GameDataTable>> ParsedTables =
        [];

    /// <summary>
    /// Gets the file's bytes decoded as 7-bit text; bytes outside the ASCII range become question marks.
    /// </summary>
    /// <value>A new string decoded from <see cref="Content"/> on every read; empty when the file is empty.</value>
    public string AsAscii => AsString(Encoding.ASCII);

    /// <summary>
    /// Gets the file's bytes decoded as UTF-8 text, which is how the game's CSV tables and JSON files are stored.
    /// </summary>
    /// <value>A new string decoded from <see cref="Content"/> on every read, serialized as <c>AsUtf8</c>; invalid byte sequences
    /// become the Unicode replacement character.</value>
    [JsonPropertyName("AsUtf8")]
    public string AsUnicodeTransformationFormat8 => AsString(Encoding.UTF8);

    /// <summary>
    /// Gets whether the file name ends in <c>.csv</c>, judged from the fingerprint path alone without reading the content.
    /// </summary>
    /// <value><see langword="true"/> if the extension of <see cref="GameAssetFingerprintEntry.File"/> is <c>.csv</c> in any letter case;
    /// otherwise, <see langword="false"/>.</value>
    public bool IsCsv =>
        Path.GetExtension(Fingerprint.File).Equals(value: ".csv", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets whether the file is a CSV game data table: a column-name row and a column-type row followed by entries.
    /// </summary>
    /// <value><see langword="true"/> if <see cref="IsCsv"/> holds and the content parses as a game table, which is parsed once and cached
    /// for <see cref="TryGetTable"/>; otherwise, <see langword="false"/>.</value>
    /// <exception cref="FormatException">The file is a CSV in some other layout.</exception>
    public bool IsStandardCsv => IsCsv && TryGetTable(out _);

    /// <summary>
    /// Decodes the file's bytes as text in the given encoding.
    /// </summary>
    /// <param name="encoding">Character encoding the file was written in.</param>
    /// <returns>The whole file as one string.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="encoding"/> is <see langword="null"/>.</exception>
    public string AsString(Encoding encoding)
    {
        ArgumentNullException.ThrowIfNull(encoding);

        return encoding.GetString(Content.Span);
    }

    /// <summary>
    /// Describes the asset by its fingerprint, content size and CSV kind, leaving out the content itself so logs stay short.
    /// </summary>
    /// <returns>A record-style line naming the fingerprint, the content length in bytes and the two CSV flags; building it parses a CSV
    /// file if that has not happened yet.</returns>
    public override string ToString()
    {
        return $"{nameof(GameAsset)} {{ Fingerprint: {Fingerprint}, Content: {Content.Length} bytes, IsCsv: {IsCsv}, IsStandardCsv: {IsStandardCsv} }}";
    }

    /// <summary>
    /// Parses a CSV file into a game data table. The parse happens once per asset instance and is shared by later calls.
    /// </summary>
    /// <param name="supercellCsvTable">The parsed table when the file is a CSV; otherwise <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the file name ends in <c>.csv</c> and its content was parsed into a table; otherwise,
    /// <see langword="false"/>.</returns>
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
