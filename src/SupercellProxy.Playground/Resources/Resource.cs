using SupercellProxy.Playground.Resources.Csv;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SupercellProxy.Playground.Resources;

public record Resource(ResourceFingerprintFile Fingerprint, Memory<byte> Content)
{
    public bool IsCsv => Path.GetExtension(Fingerprint.File).Equals(".csv", StringComparison.OrdinalIgnoreCase);
    public bool IsStandardCsv => IsCsv && TryGetTable(out _);
    public string AsUtf8 => AsString(Encoding.UTF8);
    public string AsAscii => AsString(Encoding.ASCII);

    public bool TryGetTable([MaybeNullWhen(false)] out SupercellCsvTable supercellCsvTable)
    {
        supercellCsvTable = null;

        if (IsCsv)
            supercellCsvTable = SupercellCsvParser.Parse(AsUtf8);

        return supercellCsvTable is not null;
    }

    public string AsString(Encoding encoding)
    {
        return encoding.GetString(Content.Span);
    }

    public override string ToString()
    {
        return $"{nameof(Resource)} {{ Fingerprint: {Fingerprint}, Content: {Content.Length} bytes, IsCsv: {IsCsv}, IsStandardCsv: {IsStandardCsv} }}";
    }
}
