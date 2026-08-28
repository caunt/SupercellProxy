using System.Text;

namespace SupercellProxy.Keys;

internal sealed class KeysUpdateReport
{
    private readonly List<KeysUpdateResult> _results = [];

    public IReadOnlyList<KeysUpdateResult> Results => _results;

    public void Add(KeysUpdateResult result)
    {
        ArgumentNullException.ThrowIfNull(result);
        _results.Add(result);
    }

    public string ToMarkdown()
    {
        var updated = _results.Count(static result => result.Outcome is KeysUpdateOutcome.Updated);
        var notUpdated = _results.Count - updated;
        var warnings = _results.Count(static result => result.IsWarning);
        var markdown = new StringBuilder();
        var appNames = _results
            .Where(static result => result.AppName is not "Updater")
            .Select(static result => result.AppName)
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        var title = appNames.Length is 1 ? appNames[0] : "Server public key update";

        markdown
            .Append("## ")
            .AppendLine(EscapeMarkdown(title))
            .AppendLine()
            .Append("Updated **")
            .Append(updated)
            .Append("**; not updated **")
            .Append(notUpdated)
            .Append("**; warnings **")
            .Append(warnings)
            .AppendLine("**.")
            .AppendLine()
            .AppendLine("| App | Version | Outcome | Key | Reason |")
            .AppendLine("| --- | --- | --- | --- | --- |");

        foreach (var result in _results)
        {
            markdown
                .Append("| ")
                .Append(EscapeMarkdown(result.AppName))
                .Append(" | ")
                .Append(EscapeMarkdown(result.Version ?? "—"))
                .Append(" | ")
                .Append(result.Outcome is KeysUpdateOutcome.Updated ? "Updated" : "Not updated")
                .Append(" | ")
                .Append(result.Key is null ? "—" : $"`{result.Key}`")
                .Append(" | ")
                .Append(EscapeMarkdown(result.Reason))
                .AppendLine(" |");
        }

        if (_results.Count is 0)
            markdown.AppendLine("| — | — | Not updated | — | No apps were processed. |");

        return markdown.ToString();
    }

    private static string EscapeMarkdown(string value)
    {
        return value
            .Replace("\\", "\\\\", StringComparison.Ordinal)
            .Replace("|", "\\|", StringComparison.Ordinal)
            .Replace('\r', ' ')
            .Replace('\n', ' ');
    }
}
