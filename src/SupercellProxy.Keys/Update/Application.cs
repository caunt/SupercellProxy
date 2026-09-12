using System.Globalization;
using System.Text;

using SupercellProxy.Keys.Models;
using SupercellProxy.PublicKeyExtractor;

namespace SupercellProxy.Keys;

internal static partial class Application
{
    private const int MaximumDownloadRetries = 5;

    private static void AddWarning(KeysUpdateReport report, string appName, string? version, string reason)
    {
        string message = version is null ? $"{appName}: {reason}" : $"{appName} {version}: {reason}";

        Console.Error.WriteLine($"Warning: {message}");

        if (string.Equals(Environment.GetEnvironmentVariable(variable: "GITHUB_ACTIONS"), b: "true", StringComparison.OrdinalIgnoreCase))
            Console.WriteLine($"::warning title=Server public key update::{EscapeWorkflowCommand(message)}");

        report.Add(new KeysUpdateResult(appName, version, KeysUpdateOutcome.NotUpdated, Key: null, reason, IsWarning: true));
    }

    private static async Task AppendSummaryAsync(string summaryPath, KeysUpdateReport report, CancellationToken cancellationToken)
    {
        string directory =
            Path.GetDirectoryName(summaryPath)
            ?? throw new InvalidOperationException(message: "Invalid summary path.");

        new DirectoryInfo(directory).Create();
        await File.AppendAllTextAsync(summaryPath, report.ToMarkdown(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: false), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
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
        for (int retry = 0; retry <= MaximumDownloadRetries; retry++)
        {
            try
            {
                await DownloadAsync(decryptDayClient, download, temporaryPath, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);

                return;
            }
            catch (Exception exception)
                when (IsRecoverableUpdateFailure(exception, cancellationToken))
            {
                File.Delete(temporaryPath);
                File.Delete(temporaryPath + ".part");

                if (retry == MaximumDownloadRetries)
                    throw new InvalidOperationException($"after {MaximumDownloadRetries} retries. Last error: " + NormalizeReason(exception.Message), exception);

                await Console
                    .Error.WriteLineAsync(
                        $"Download for {appName} {version} failed: {NormalizeReason(exception.Message)} "
                            + "Retrying from the beginning with a fresh browser session "
                            + string.Create(CultureInfo.InvariantCulture, $"(retry {retry + 1}/{MaximumDownloadRetries})...")
                    )
                    .ConfigureAwait(continueOnCapturedContext: false);
                await Task.Delay(TimeSpan.FromSeconds((retry + 1) * 2), TimeProvider.System, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
        }
    }

    private static string EscapeWorkflowCommand(string value)
    {
        return value
            .Replace(oldValue: "%", newValue: "%25", StringComparison.Ordinal)
            .Replace(oldValue: "\r", newValue: "%0D", StringComparison.Ordinal)
            .Replace(oldValue: "\n", newValue: "%0A", StringComparison.Ordinal);
    }

    private static bool IsRecoverableUpdateFailure(Exception exception, CancellationToken cancellationToken)
    {
        return exception is not OutOfMemoryException
            && !(
                exception is OperationCanceledException && cancellationToken.IsCancellationRequested
            );
    }

    private static string NormalizeAppOption(string value)
    {
        string normalized = value.Trim();

        if (normalized.StartsWith(value: "id", StringComparison.OrdinalIgnoreCase))
            normalized = normalized[2..];

        return normalized.Length is 0 || !normalized.All(char.IsAsciiDigit)
            ? throw new ArgumentException($"Invalid App Store ID: {value}", nameof(value))
            : normalized;
    }

    private static string NormalizeReason(string value)
    {
        string normalized = string.Join(separator: ' ', value.Split(separator: default(char[]?), StringSplitOptions.RemoveEmptyEntries));

        return normalized.Length <= 500 ? normalized : normalized[..497] + "...";
    }

    private static (string KeysPath, string? SummaryPath, string? AppOption) ParseUpdateArguments(string[] arguments)
    {
        List<string> positionalArguments = new(capacity: 1);
        string? summaryOption = null;
        string? appOption = null;

        for (int index = 0; index < arguments.Length; index++)
        {
            string argument = arguments[index];

            if (string.Equals(argument, b: "--summary", StringComparison.Ordinal))
            {
                if (summaryOption is not null)
                    throw new ArgumentException(message: "--summary may only be specified once.", nameof(arguments));

                if (++index >= arguments.Length || string.IsNullOrWhiteSpace(arguments[index]))
                    throw new ArgumentException(message: "--summary requires a path.", nameof(arguments));

                summaryOption = arguments[index];
            }
            else if (string.Equals(argument, b: "--app", StringComparison.Ordinal))
            {
                if (appOption is not null)
                    throw new ArgumentException(message: "--app may only be specified once.", nameof(arguments));

                if (++index >= arguments.Length || string.IsNullOrWhiteSpace(arguments[index]))
                    throw new ArgumentException(message: "--app requires an App Store ID.", nameof(arguments));

                appOption = NormalizeAppOption(arguments[index]);
            }
            else if (argument.StartsWith(value: '-'))
            {
                throw new ArgumentException($"Unknown option: {argument}", nameof(arguments));
            }
            else
            {
                positionalArguments.Add(argument);
            }
        }

        if (positionalArguments.Count > 1)
            throw new ArgumentException(message: "Usage: SupercellProxy.Keys update [FILE] [--app APP_STORE_ID] [--summary PATH]", nameof(arguments));

        string keysPath = Path.GetFullPath(positionalArguments.FirstOrDefault() ?? "KEYS.md");

        return (
            keysPath,
            summaryOption is null ? null : Path.GetFullPath(summaryOption),
            appOption
        );
    }

    private static int PrintUpdateHelp()
    {
        return PrintCommandHelp(usage: "update [FILE] [--app APP_STORE_ID] [--summary PATH]", description: "Fill missing KEYS.md entries from decrypt.day IPAs");
    }

    private static async Task<int> RunUpdateAsync(string[] arguments, CancellationToken cancellationToken)
    {
        if (arguments.Any(IsHelp))
            return PrintUpdateHelp();

        (string? keysPath, string? summaryPath, string? appOption) = ParseUpdateArguments(arguments);
        KeysUpdateReport report = new();

        try
        {
            await UpdateKeysAsync(keysPath, appOption, report, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            return 0;
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            report.Add(
                new KeysUpdateResult(AppName: "Updater", Version: null, KeysUpdateOutcome.NotUpdated, Key: null, NormalizeReason(exception.Message), IsWarning: true)
            );

            throw;
        }
        finally
        {
            if (summaryPath is not null)
            {
                await AppendSummaryAsync(summaryPath, report, CancellationToken.None)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
        }
    }

    private static async Task UpdateKeysAsync(string keysPath, string? appStoreIdentifier, KeysUpdateReport report, CancellationToken cancellationToken)
    {
        if (!File.Exists(keysPath))
            throw new FileNotFoundException(message: "The keys document was not found.", keysPath);

        string original = await File.ReadAllTextAsync(keysPath, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        KeysDocument document = KeysDocument.Parse(original);
        DecryptDayClient decryptDayClient = new(WebClient);
        Dictionary<string, KeysSectionUpdate> updates = new(StringComparer.Ordinal);
        IReadOnlyList<KeysSection> sections;

        if (appStoreIdentifier is null)
        {
            sections = document.Sections;
        }
        else
        {
            KeysSection section =
                document.Sections.SingleOrDefault(candidate => string.Equals(candidate.AppStoreIdentifier, appStoreIdentifier, StringComparison.Ordinal))
                ?? throw new InvalidOperationException($"KEYS.md does not contain an app section for ID {appStoreIdentifier}.");

            sections = [section];
        }

        foreach (KeysSection section in sections)
            await UpdateSectionAsync(section).ConfigureAwait(continueOnCapturedContext: false);

        async Task UpdateSectionAsync(KeysSection section)
        {
            IpaApp app;

            try
            {
                app = await decryptDayClient
                    .GetAppAsync(section.AppStoreIdentifier, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception exception)
                when (IsRecoverableUpdateFailure(exception, cancellationToken))
            {
                string reason = $"Metadata request failed: {NormalizeReason(exception.Message)}";
                AddWarning(report, section.Name, version: null, reason);
                updates[section.AppStoreIdentifier] = new KeysSectionUpdate([]);

                return;
            }

            IReadOnlyList<AppVersion> sourceVersions = app.Versions;

            Dictionary<string, ExistingKeyEntry> existing = section.Entries.ToDictionary(static entry => entry.Version, StringComparer.Ordinal);

            List<GeneratedKeyEntry> generated = [];

            if (sourceVersions.Count is 0)
            {
                AddWarning(report, section.Name, version: null, reason: "decrypt.day returned no versions.");
                updates[section.AppStoreIdentifier] = new KeysSectionUpdate(generated);

                return;
            }

            foreach (AppVersion version in sourceVersions)
                await UpdateVersionAsync(version).ConfigureAwait(continueOnCapturedContext: false);

            async Task UpdateVersionAsync(AppVersion version)
            {
                if (existing.TryGetValue(version.Value, out ExistingKeyEntry? existingEntry))
                {
                    report.Add(new KeysUpdateResult(section.Name, version.Value, KeysUpdateOutcome.NotUpdated, existingEntry.Key, Reason: "Already present"));

                    return;
                }

                IpaDownload? download = await TryAuthorizeVersionAsync().ConfigureAwait(continueOnCapturedContext: false);

                if (download is null)
                    return;

                string? key = await TryExtractVersionKeyAsync(download).ConfigureAwait(continueOnCapturedContext: false);

                if (key is null)
                    return;

                generated.Add(new GeneratedKeyEntry(version.Value, key));
                report.Add(new KeysUpdateResult(section.Name, version.Value, KeysUpdateOutcome.Updated, key, Reason: "Added to KEYS.md"));

                async Task<IpaDownload?> TryAuthorizeVersionAsync()
                {
                    try
                    {
                        IpaDownload? authorized = await decryptDayClient
                            .TryAuthorizeAsync(section.AppStoreIdentifier, version, cancellationToken)
                            .ConfigureAwait(continueOnCapturedContext: false);

                        if (authorized is null)
                            AddWarning(report, section.Name, version.Value, reason: "No free, login-free IPA is available.");

                        return authorized;
                    }
                    catch (Exception exception)
                        when (IsRecoverableUpdateFailure(exception, cancellationToken))
                    {
                        AddWarning(report, section.Name, version.Value, $"Authorization failed: {NormalizeReason(exception.Message)}");

                        return null;
                    }
                }

                async Task<string?> TryExtractVersionKeyAsync(IpaDownload authorizedDownload)
                {
                    string temporaryPath = Path.Combine(Path.GetTempPath(), $"supercell-proxy-key-{section.AppStoreIdentifier}-{Guid.NewGuid():N}.ipa");

                    try
                    {
                        try
                        {
                            await DownloadWithRetryAsync(decryptDayClient, authorizedDownload, temporaryPath, section.Name, version.Value, cancellationToken)
                                .ConfigureAwait(continueOnCapturedContext: false);
                        }
                        catch (Exception exception)
                            when (IsRecoverableUpdateFailure(exception, cancellationToken))
                        {
                            AddWarning(report, section.Name, version.Value, $"Download failed {NormalizeReason(exception.Message)}");

                            return null;
                        }

                        try
                        {
                            return Convert.ToHexString(
                                await ServerPublicKeyExtractor
                                    .ExtractFileAsync(temporaryPath, cancellationToken)
                                    .ConfigureAwait(continueOnCapturedContext: false)
                            );
                        }
                        catch (Exception exception)
                            when (IsRecoverableUpdateFailure(exception, cancellationToken))
                        {
                            AddWarning(report, section.Name, version.Value, $"Extraction failed: {NormalizeReason(exception.Message)}");

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

            updates[section.AppStoreIdentifier] = new KeysSectionUpdate(generated);
        }

        string updated = document.Render(updates);

        if (string.Equals(updated, original, StringComparison.Ordinal))
        {
            Console.WriteLine(value: "No new keys or key-table maintenance changes were found.");

            return;
        }

        await WriteAtomicallyAsync(keysPath, updated, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        int addedKeyCount = report.Results.Count(static result => result.Outcome is KeysUpdateOutcome.Updated);

        Console.WriteLine(string.Create(CultureInfo.InvariantCulture, $"Updated {keysPath}; added {addedKeyCount} new key(s)."));
    }

    private static async Task WriteAtomicallyAsync(string path, string content, CancellationToken cancellationToken)
    {
        string directory =
            Path.GetDirectoryName(path)
            ?? throw new InvalidOperationException(message: "Invalid keys document path.");

        string temporaryPath = Path.Combine(directory, $".{Path.GetFileName(path)}.{Guid.NewGuid():N}.tmp");

        try
        {
            await File.WriteAllTextAsync(temporaryPath, content, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false), cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            File.Move(temporaryPath, path, overwrite: true);
        }
        finally
        {
            File.Delete(temporaryPath);
        }
    }
}
