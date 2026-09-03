using System.Globalization;
using System.Text;
using SupercellProxy.PublicKeyExtractor;

namespace SupercellProxy.Keys;

internal static partial class Application
{
    private const int MaximumDownloadRetries = 5;

    private static async Task<int> RunUpdateAsync(
        string[] args,
        CancellationToken cancellationToken
    )
    {
        if (args.Any(IsHelp))
            return PrintUpdateHelp();

        var (keysPath, summaryPath, appOption) = ParseUpdateArguments(args);
        var report = new KeysUpdateReport();

        try
        {
            await UpdateKeysAsync(keysPath, appOption, report, cancellationToken)
                .ConfigureAwait(false);
            return 0;
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            report.Add(
                new KeysUpdateResult(
                    "Updater",
                    Version: null,
                    KeysUpdateOutcome.NotUpdated,
                    Key: null,
                    NormalizeReason(exception.Message),
                    IsWarning: true
                )
            );
            throw;
        }
        finally
        {
            if (summaryPath is not null)
                await AppendSummaryAsync(summaryPath, report, CancellationToken.None)
                    .ConfigureAwait(false);
        }
    }

    private static (string KeysPath, string? SummaryPath, string? AppOption) ParseUpdateArguments(
        string[] args
    )
    {
        var positionalArguments = new List<string>(1);
        string? summaryOption = null;
        string? appOption = null;
        for (var index = 0; index < args.Length; index++)
        {
            var argument = args[index];
            if (string.Equals(argument, "--summary", StringComparison.Ordinal))
            {
                if (summaryOption is not null)
                    throw new ArgumentException(
                        "--summary may only be specified once.",
                        nameof(args)
                    );
                if (++index >= args.Length || string.IsNullOrWhiteSpace(args[index]))
                    throw new ArgumentException("--summary requires a path.", nameof(args));
                summaryOption = args[index];
            }
            else if (string.Equals(argument, "--app", StringComparison.Ordinal))
            {
                if (appOption is not null)
                    throw new ArgumentException("--app may only be specified once.", nameof(args));
                if (++index >= args.Length || string.IsNullOrWhiteSpace(args[index]))
                    throw new ArgumentException("--app requires an App Store ID.", nameof(args));
                appOption = NormalizeAppOption(args[index]);
            }
            else if (argument.StartsWith('-'))
            {
                throw new ArgumentException($"Unknown option: {argument}", nameof(args));
            }
            else
            {
                positionalArguments.Add(argument);
            }
        }

        if (positionalArguments.Count > 1)
            throw new ArgumentException(
                "Usage: SupercellProxy.Keys update [FILE] [--app APP_STORE_ID] [--summary PATH]",
                nameof(args)
            );

        var keysPath = Path.GetFullPath(positionalArguments.FirstOrDefault() ?? "KEYS.md");
        return (
            keysPath,
            summaryOption is null ? null : Path.GetFullPath(summaryOption),
            appOption
        );
    }

    private static async Task UpdateKeysAsync(
        string keysPath,
        string? appStoreId,
        KeysUpdateReport report,
        CancellationToken cancellationToken
    )
    {
        if (!File.Exists(keysPath))
            throw new FileNotFoundException("The keys document was not found.", keysPath);

        var original = await File.ReadAllTextAsync(keysPath, cancellationToken)
            .ConfigureAwait(false);
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
            var section =
                document.Sections.SingleOrDefault(candidate =>
                    string.Equals(candidate.AppStoreId, appStoreId, StringComparison.Ordinal)
                )
                ?? throw new InvalidOperationException(
                    $"KEYS.md does not contain an app section for ID {appStoreId}."
                );

            sections = [section];
        }

        foreach (var section in sections)
            await UpdateSectionAsync(section).ConfigureAwait(false);

        async Task UpdateSectionAsync(KeysSection section)
        {
            IpaApp app;

            try
            {
                app = await decryptDayClient
                    .GetAppAsync(section.AppStoreId, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception exception)
                when (IsRecoverableUpdateFailure(exception, cancellationToken))
            {
                var reason = $"Metadata request failed: {NormalizeReason(exception.Message)}";
                AddWarning(report, section.Name, version: null, reason);
                updates[section.AppStoreId] = new KeysSectionUpdate([]);
                return;
            }

            var sourceVersions = app.Versions;
            var existing = section.Entries.ToDictionary(
                static entry => entry.Version,
                StringComparer.Ordinal
            );
            var generated = new List<GeneratedKeyEntry>();

            if (sourceVersions.Count is 0)
            {
                AddWarning(
                    report,
                    section.Name,
                    version: null,
                    "decrypt.day returned no versions."
                );
                updates[section.AppStoreId] = new KeysSectionUpdate(generated);
                return;
            }

            foreach (var version in sourceVersions)
                await UpdateVersionAsync(version).ConfigureAwait(false);

            async Task UpdateVersionAsync(AppVersion version)
            {
                if (existing.TryGetValue(version.Value, out var existingEntry))
                {
                    report.Add(
                        new KeysUpdateResult(
                            section.Name,
                            version.Value,
                            KeysUpdateOutcome.NotUpdated,
                            existingEntry.Key,
                            "Already present"
                        )
                    );
                    return;
                }

                var download = await TryAuthorizeVersionAsync().ConfigureAwait(false);
                if (download is null)
                    return;
                var key = await TryExtractVersionKeyAsync(download).ConfigureAwait(false);
                if (key is null)
                    return;

                generated.Add(new GeneratedKeyEntry(version.Value, key));
                report.Add(
                    new KeysUpdateResult(
                        section.Name,
                        version.Value,
                        KeysUpdateOutcome.Updated,
                        key,
                        "Added to KEYS.md"
                    )
                );

                async Task<IpaDownload?> TryAuthorizeVersionAsync()
                {
                    try
                    {
                        var authorized = await decryptDayClient
                            .TryAuthorizeAsync(section.AppStoreId, version, cancellationToken)
                            .ConfigureAwait(false);
                        if (authorized is null)
                            AddWarning(
                                report,
                                section.Name,
                                version.Value,
                                "No free, login-free IPA is available."
                            );
                        return authorized;
                    }
                    catch (Exception exception)
                        when (IsRecoverableUpdateFailure(exception, cancellationToken))
                    {
                        AddWarning(
                            report,
                            section.Name,
                            version.Value,
                            $"Authorization failed: {NormalizeReason(exception.Message)}"
                        );
                        return null;
                    }
                }

                async Task<string?> TryExtractVersionKeyAsync(IpaDownload authorizedDownload)
                {
                    var temporaryPath = Path.Combine(
                        Path.GetTempPath(),
                        $"supercell-proxy-key-{section.AppStoreId}-{Guid.NewGuid():N}.ipa"
                    );
                    try
                    {
                        try
                        {
                            await DownloadWithRetryAsync(
                                    decryptDayClient,
                                    authorizedDownload,
                                    temporaryPath,
                                    section.Name,
                                    version.Value,
                                    cancellationToken
                                )
                                .ConfigureAwait(false);
                        }
                        catch (Exception exception)
                            when (IsRecoverableUpdateFailure(exception, cancellationToken))
                        {
                            AddWarning(
                                report,
                                section.Name,
                                version.Value,
                                $"Download failed {NormalizeReason(exception.Message)}"
                            );
                            return null;
                        }

                        try
                        {
                            return Convert.ToHexString(
                                await ServerPublicKeyExtractor
                                    .ExtractFileAsync(temporaryPath, cancellationToken)
                                    .ConfigureAwait(false)
                            );
                        }
                        catch (Exception exception)
                            when (IsRecoverableUpdateFailure(exception, cancellationToken))
                        {
                            AddWarning(
                                report,
                                section.Name,
                                version.Value,
                                $"Extraction failed: {NormalizeReason(exception.Message)}"
                            );
                            return null;
                        }
                    }
                    finally
                    {
                        File.Delete(temporaryPath);
                        File.Delete(temporaryPath + ".part");
                    }
                }
            }

            updates[section.AppStoreId] = new KeysSectionUpdate(generated);
        }

        var updated = document.Render(updates);

        if (string.Equals(updated, original, StringComparison.Ordinal))
        {
            Console.WriteLine("No new keys or key-table maintenance changes were found.");
            return;
        }

        await WriteAtomicallyAsync(keysPath, updated, cancellationToken).ConfigureAwait(false);
        var addedKeyCount = report.Results.Count(static result =>
            result.Outcome is KeysUpdateOutcome.Updated
        );
        Console.WriteLine(
            string.Create(
                CultureInfo.InvariantCulture,
                $"Updated {keysPath}; added {addedKeyCount} new key(s)."
            )
        );
    }

    private static async Task DownloadWithRetryAsync(
        DecryptDayClient decryptDayClient,
        IpaDownload download,
        string temporaryPath,
        string appName,
        string version,
        CancellationToken cancellationToken
    )
    {
        for (var retry = 0; ; retry++)
        {
            try
            {
                await DownloadAsync(decryptDayClient, download, temporaryPath, cancellationToken)
                    .ConfigureAwait(false);
                return;
            }
            catch (Exception exception)
                when (IsRecoverableUpdateFailure(exception, cancellationToken))
            {
                File.Delete(temporaryPath);
                File.Delete(temporaryPath + ".part");

                if (retry == MaximumDownloadRetries)
                {
                    throw new InvalidOperationException(
                        $"after {MaximumDownloadRetries} retries. Last error: "
                            + NormalizeReason(exception.Message),
                        exception
                    );
                }

                await Console
                    .Error.WriteLineAsync(
                        $"Download for {appName} {version} failed: {NormalizeReason(exception.Message)} "
                            + "Retrying from the beginning with a fresh browser session "
                            + string.Create(
                                CultureInfo.InvariantCulture,
                                $"(retry {retry + 1}/{MaximumDownloadRetries})..."
                            )
                    )
                    .ConfigureAwait(false);
                await Task.Delay(
                        TimeSpan.FromSeconds((retry + 1) * 2),
                        TimeProvider.System,
                        cancellationToken
                    )
                    .ConfigureAwait(false);
            }
        }
    }

    private static bool IsRecoverableUpdateFailure(
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        return exception is not OutOfMemoryException
            && !(
                exception is OperationCanceledException && cancellationToken.IsCancellationRequested
            );
    }

    private static void AddWarning(
        KeysUpdateReport report,
        string appName,
        string? version,
        string reason
    )
    {
        var message = version is null ? $"{appName}: {reason}" : $"{appName} {version}: {reason}";

        Console.Error.WriteLine($"Warning: {message}");

        if (
            string.Equals(
                Environment.GetEnvironmentVariable("GITHUB_ACTIONS"),
                "true",
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            Console.WriteLine(
                $"::warning title=Server public key update::{EscapeWorkflowCommand(message)}"
            );
        }

        report.Add(
            new KeysUpdateResult(
                appName,
                version,
                KeysUpdateOutcome.NotUpdated,
                Key: null,
                reason,
                IsWarning: true
            )
        );
    }

    private static string NormalizeReason(string value)
    {
        var normalized = string.Join(
            ' ',
            value.Split(default(char[]?), StringSplitOptions.RemoveEmptyEntries)
        );

        return normalized.Length <= 500 ? normalized : normalized[..497] + "...";
    }

    private static string NormalizeAppOption(string value)
    {
        var normalized = value.Trim();

        if (normalized.StartsWith("id", StringComparison.OrdinalIgnoreCase))
            normalized = normalized[2..];

        if (normalized.Length is 0 || !normalized.All(char.IsAsciiDigit))
            throw new ArgumentException($"Invalid App Store ID: {value}", nameof(value));

        return normalized;
    }

    private static string EscapeWorkflowCommand(string value)
    {
        return value
            .Replace("%", "%25", StringComparison.Ordinal)
            .Replace("\r", "%0D", StringComparison.Ordinal)
            .Replace("\n", "%0A", StringComparison.Ordinal);
    }

    private static async Task AppendSummaryAsync(
        string summaryPath,
        KeysUpdateReport report,
        CancellationToken cancellationToken
    )
    {
        var directory =
            Path.GetDirectoryName(summaryPath)
            ?? throw new InvalidOperationException("Invalid summary path.");

        Directory.CreateDirectory(directory);
        await File.AppendAllTextAsync(
                summaryPath,
                report.ToMarkdown(),
                new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
                cancellationToken
            )
            .ConfigureAwait(false);
    }

    private static async Task WriteAtomicallyAsync(
        string path,
        string content,
        CancellationToken cancellationToken
    )
    {
        var directory =
            Path.GetDirectoryName(path)
            ?? throw new InvalidOperationException("Invalid keys document path.");
        var temporaryPath = Path.Combine(
            directory,
            $".{Path.GetFileName(path)}.{Guid.NewGuid():N}.tmp"
        );

        try
        {
            await File.WriteAllTextAsync(
                    temporaryPath,
                    content,
                    new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
                    cancellationToken
                )
                .ConfigureAwait(false);
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
            "Fill missing KEYS.md entries from decrypt.day IPAs"
        );
    }
}
