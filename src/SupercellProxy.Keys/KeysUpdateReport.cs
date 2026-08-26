using System.Text;

namespace SupercellProxy.Keys;

internal sealed class KeysUpdateReport
{
    private readonly List<KeysUpdateResult> results = [];

    public IReadOnlyList<KeysUpdateResult> Results => results;

    public void Add(KeysUpdateResult result)
    {
        ArgumentNullException.ThrowIfNull(result);
        results.Add(result);
    }

    public string ToMarkdown()
    {
        var updated = results.Count(static result => result.Outcome is KeysUpdateOutcome.Updated);
        var notUpdated = results.Count - updated;
        var warnings = results.Count(static result => result.IsWarning);
        var markdown = new StringBuilder();
        var appNames = results
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

        foreach (var result in results)
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

        if (results.Count is 0)
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
