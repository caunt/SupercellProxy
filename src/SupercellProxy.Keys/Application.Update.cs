using System.Text;
using SupercellProxy.PublicKeyExtractor;

namespace SupercellProxy.Keys;

internal static partial class Application
{
    private static async Task<int> RunUpdateAsync(
        string[] args,
        CancellationToken cancellationToken)
    {
        if (args.Any(IsHelp))
            return PrintUpdateHelp();

        var positionalArguments = new List<string>(1);
        string? summaryOption = null;
        string? appOption = null;

        for (var index = 0; index < args.Length; index++)
        {
            var argument = args[index];

            if (argument == "--summary")
            {
                if (summaryOption is not null)
                    throw new ArgumentException("--summary may only be specified once.");

                index++;

                if (index >= args.Length || string.IsNullOrWhiteSpace(args[index]))
                    throw new ArgumentException("--summary requires a path.");

                summaryOption = args[index];
            }
            else if (argument == "--app")
            {
                if (appOption is not null)
                    throw new ArgumentException("--app may only be specified once.");

                index++;

                if (index >= args.Length || string.IsNullOrWhiteSpace(args[index]))
                    throw new ArgumentException("--app requires an App Store ID.");

                appOption = NormalizeAppOption(args[index]);
            }
            else if (argument.StartsWith("-", StringComparison.Ordinal))
            {
                throw new ArgumentException($"Unknown option: {argument}");
            }
            else
            {
                positionalArguments.Add(argument);
            }
        }

        if (positionalArguments.Count > 1)
        {
            throw new ArgumentException(
                "Usage: SupercellProxy.Keys update [FILE] [--app APP_STORE_ID] [--summary PATH]");
        }

        var keysPath = Path.GetFullPath(positionalArguments.FirstOrDefault() ?? "KEYS.md");
        var summaryPath = summaryOption is null ? null : Path.GetFullPath(summaryOption);
        var report = new KeysUpdateReport();

        try
        {
            await UpdateKeysAsync(keysPath, appOption, report, cancellationToken);
            return 0;
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            report.Add(new KeysUpdateResult(
                "Updater",
                null,
                KeysUpdateOutcome.NotUpdated,
                null,
                NormalizeReason(exception.Message),
                IsWarning: true));
            throw;
        }
        finally
        {
            if (summaryPath is not null)
                await AppendSummaryAsync(summaryPath, report, CancellationToken.None);
        }
    }

    private static async Task UpdateKeysAsync(
        string keysPath,
        string? appStoreId,
        KeysUpdateReport report,
        CancellationToken cancellationToken)
    {
        if (!File.Exists(keysPath))
            throw new FileNotFoundException("The keys document was not found.", keysPath);

        var original = await File.ReadAllTextAsync(keysPath, cancellationToken);
        var document = KeysDocument.Parse(original);
        var decryptDayClient = new DecryptDayClient(HttpClient);
        var updates = new Dictionary<string, KeysSectionUpdate>(StringComparer.Ordinal);
        IReadOnlyList<KeysSection> sections;

        if (appStoreId is null)
        {
            sections = document.Sections;
        }
        else
        {
            var section = document.Sections.SingleOrDefault(
                candidate => candidate.AppStoreId == appStoreId)
                ?? throw new InvalidOperationException(
                    $"KEYS.md does not contain an app section for ID {appStoreId}.");

            sections = [section];
        }

        foreach (var section in sections)
        {
            IpaApp app;

            try
            {
                app = await decryptDayClient.GetAppAsync(section.AppStoreId, cancellationToken);
            }
            catch (Exception exception) when (IsRecoverableUpdateFailure(exception, cancellationToken))
            {
                var reason = $"Metadata request failed: {NormalizeReason(exception.Message)}";
                AddWarning(report, section.Name, null, reason);
                updates[section.AppStoreId] = new KeysSectionUpdate([], []);
                continue;
            }

            var sourceVersions = app.Versions.Distinct(StringComparer.Ordinal).ToArray();
            var existing = section.Entries.ToDictionary(entry => entry.Version, StringComparer.Ordinal);
            var generated = new List<GeneratedKeyEntry>();

            if (sourceVersions.Length == 0)
            {
                AddWarning(report, section.Name, null, "decrypt.day returned no versions.");
                updates[section.AppStoreId] = new KeysSectionUpdate(sourceVersions, generated);
                continue;
            }

            for (var sourceIndex = 0; sourceIndex < sourceVersions.Length; sourceIndex++)
            {
                var version = sourceVersions[sourceIndex];

                if (existing.TryGetValue(version, out var existingEntry))
                {
                    report.Add(new KeysUpdateResult(
                        section.Name,
                        version,
                        KeysUpdateOutcome.NotUpdated,
                        existingEntry.Key,
                        "Already present"));
                    continue;
                }

                IpaDownload? download;

                try
                {
                    download = await decryptDayClient.TryAuthorizeAsync(
                        section.AppStoreId,
                        version,
                        cancellationToken);
                }
                catch (Exception exception) when (IsRecoverableUpdateFailure(exception, cancellationToken))
                {
                    AddWarning(
                        report,
                        section.Name,
                        version,
                        $"Authorization failed: {NormalizeReason(exception.Message)}");
                    continue;
                }

                if (download is null)
                {
                    AddWarning(
                        report,
                        section.Name,
                        version,
                        "No free, login-free IPA is available.");
                    continue;
                }

                var temporaryPath = Path.Combine(
                    Path.GetTempPath(),
                    $"supercell-proxy-key-{section.AppStoreId}-{Guid.NewGuid():N}.ipa");

                try
                {
                    try
                    {
                        await DownloadAsync(
                            decryptDayClient,
                            download,
                            temporaryPath,
                            cancellationToken);
                    }
                    catch (Exception exception) when (IsRecoverableUpdateFailure(exception, cancellationToken))
                    {
                        AddWarning(
                            report,
                            section.Name,
                            version,
                            $"Download failed: {NormalizeReason(exception.Message)}");
                        continue;
                    }

                    string key;

                    try
                    {
                        key = Convert.ToHexString(ServerPublicKeyExtractor.ExtractFile(temporaryPath));
                    }
                    catch (Exception exception) when (IsRecoverableUpdateFailure(exception, cancellationToken))
                    {
                        AddWarning(
                            report,
                            section.Name,
                            version,
                            $"Extraction failed: {NormalizeReason(exception.Message)}");
                        continue;
                    }

                    generated.Add(new GeneratedKeyEntry(
                        section.AppStoreId,
                        version,
                        key,
                        sourceIndex));
                    report.Add(new KeysUpdateResult(
                        section.Name,
                        version,
                        KeysUpdateOutcome.Updated,
                        key,
                        "Added to KEYS.md"));
                }
                finally
                {
                    File.Delete(temporaryPath);
                    File.Delete(temporaryPath + ".part");
                }
            }

            updates[section.AppStoreId] = new KeysSectionUpdate(sourceVersions, generated);
        }

        var updated = document.Render(updates);

        if (string.Equals(updated, original, StringComparison.Ordinal))
        {
            Console.WriteLine("No new keys were found; KEYS.md was not changed.");
            return;
        }

        await WriteAtomicallyAsync(keysPath, updated, cancellationToken);
        Console.WriteLine($"Added {report.Results.Count(result => result.Outcome == KeysUpdateOutcome.Updated)} key(s) to {keysPath}.");
    }

    private static bool IsRecoverableUpdateFailure(
        Exception exception,
        CancellationToken cancellationToken)
    {
        return exception is not OutOfMemoryException &&
               !(exception is OperationCanceledException && cancellationToken.IsCancellationRequested);
    }

    private static void AddWarning(
        KeysUpdateReport report,
        string appName,
        string? version,
        string reason)
    {
        var message = version is null
            ? $"{appName}: {reason}"
            : $"{appName} {version}: {reason}";

        Console.Error.WriteLine($"Warning: {message}");

        if (string.Equals(
                Environment.GetEnvironmentVariable("GITHUB_ACTIONS"),
                "true",
                StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"::warning title=Server public key update::{EscapeWorkflowCommand(message)}");
        }

        report.Add(new KeysUpdateResult(
            appName,
            version,
            KeysUpdateOutcome.NotUpdated,
            null,
            reason,
            IsWarning: true));
    }

    private static string NormalizeReason(string value)
    {
        var normalized = string.Join(
            ' ',
            value.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));

        return normalized.Length <= 500 ? normalized : normalized[..497] + "...";
    }

    private static string NormalizeAppOption(string value)
    {
        var normalized = value.Trim();

        if (normalized.StartsWith("id", StringComparison.OrdinalIgnoreCase))
            normalized = normalized[2..];

        if (normalized.Length == 0 || !normalized.All(char.IsAsciiDigit))
            throw new ArgumentException($"Invalid App Store ID: {value}", nameof(value));

        return normalized;
    }

    private static string EscapeWorkflowCommand(string value)
    {
        return value.Replace("%", "%25", StringComparison.Ordinal)
            .Replace("\r", "%0D", StringComparison.Ordinal)
            .Replace("\n", "%0A", StringComparison.Ordinal);
    }

    private static async Task AppendSummaryAsync(
        string summaryPath,
        KeysUpdateReport report,
        CancellationToken cancellationToken)
    {
        var directory = Path.GetDirectoryName(summaryPath)
                        ?? throw new InvalidOperationException("Invalid summary path.");

        Directory.CreateDirectory(directory);
        await File.AppendAllTextAsync(
            summaryPath,
            report.ToMarkdown(),
            new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
            cancellationToken);
    }

    private static async Task WriteAtomicallyAsync(
        string path,
        string content,
        CancellationToken cancellationToken)
    {
        var directory = Path.GetDirectoryName(path)
                        ?? throw new InvalidOperationException("Invalid keys document path.");
        var temporaryPath = Path.Combine(
            directory,
            $".{Path.GetFileName(path)}.{Guid.NewGuid():N}.tmp");

        try
        {
            await File.WriteAllTextAsync(
                temporaryPath,
                content,
                new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
                cancellationToken);
            File.Move(temporaryPath, path, overwrite: true);
        }
        finally
        {
            File.Delete(temporaryPath);
        }
    }

    private static int PrintUpdateHelp()
    {
        return PrintCommandHelp(
            "update [FILE] [--app APP_STORE_ID] [--summary PATH]",
            "Fill missing KEYS.md entries from decrypt.day IPAs");
    }
}
