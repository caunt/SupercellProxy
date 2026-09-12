using System.Text;

using SupercellProxy.Keys.Models;

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
        int updated = _results.Count(static result => result.Outcome is KeysUpdateOutcome.Updated);
        int notUpdated = _results.Count - updated;
        int warnings = _results.Count(static result => result.IsWarning);
        StringBuilder markdown = new();

        string[] appNames = [.. _results
            .Where(static result => result.AppName is not "Updater")
            .Select(static result => result.AppName)
            .Distinct(StringComparer.Ordinal)];

        string title = appNames.Length is 1 ? appNames[0] : "Server public key update";

        markdown = markdown
            .Append(value: "## ")
            .AppendLine(EscapeMarkdown(title))
            .AppendLine()
            .Append(value: "Updated **")
            .Append(updated)
            .Append(value: "**; not updated **")
            .Append(notUpdated)
            .Append(value: "**; warnings **")
            .Append(warnings)
            .AppendLine(value: "**.")
            .AppendLine()
            .AppendLine(value: "| App | Version | Outcome | Key | Reason |")
            .AppendLine(value: "| --- | --- | --- | --- | --- |");

        foreach (KeysUpdateResult result in _results)
        {
            markdown = markdown
                .Append(value: "| ")
                .Append(EscapeMarkdown(result.AppName))
                .Append(value: " | ")
                .Append(EscapeMarkdown(result.Version ?? "—"))
                .Append(value: " | ")
                .Append(result.Outcome is KeysUpdateOutcome.Updated ? "Updated" : "Not updated")
                .Append(value: " | ")
                .Append(result.Key is null ? "—" : $"`{result.Key}`")
                .Append(value: " | ")
                .Append(EscapeMarkdown(result.Reason))
                .AppendLine(value: " |");
        }

        if (_results.Count is 0)
            markdown = markdown.AppendLine(value: "| — | — | Not updated | — | No apps were processed. |");

        return markdown.ToString();
    }

    private static string EscapeMarkdown(string value)
    {
        return value
            .Replace(oldValue: "\\", newValue: "\\\\", StringComparison.Ordinal)
            .Replace(oldValue: "|", newValue: "\\|", StringComparison.Ordinal)
            .Replace(oldChar: '\r', newChar: ' ')
            .Replace(oldChar: '\n', newChar: ' ');
    }
}
