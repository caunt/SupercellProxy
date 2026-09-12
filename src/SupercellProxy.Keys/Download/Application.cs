using System.IO.Compression;

using SupercellProxy.Keys.AppStore;
using SupercellProxy.Keys.Models;

namespace SupercellProxy.Keys;

internal static partial class Application
{

    private static async Task DownloadAsync(DecryptDayClient decryptDayClient, IpaDownload download, string outputPath, CancellationToken cancellationToken)
    {
        const int bufferSize = 131_072;

        string outputDirectory =
            Path.GetDirectoryName(outputPath)
            ?? throw new InvalidOperationException(message: "Invalid output path.");

        string partialPath = outputPath + ".part";

        new DirectoryInfo(outputDirectory).Create();

        try
        {
            FileStream output = new(
                partialPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize,
                FileOptions.Asynchronous | FileOptions.SequentialScan
            );

            await using (output.ConfigureAwait(continueOnCapturedContext: false))
            {
                await DecryptDayClient
                    .DownloadAsync(download, output, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }

            if (!await IsValidIpaAsync(partialPath, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
                throw new InvalidDataException(message: "Downloaded file is not a valid IPA/ZIP.");

            File.Move(partialPath, outputPath, overwrite: true);
        }
        finally
        {
            File.Delete(partialPath);
        }
    }

    private static async Task<bool> IsValidIpaAsync(string path, CancellationToken cancellationToken)
    {
        if (!File.Exists(path))
            return false;

        try
        {
            ZipArchive archive = await ZipFile
                .OpenReadAsync(path, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            await using (archive.ConfigureAwait(continueOnCapturedContext: false))
            {
                return archive.Entries.Any(
                    static entry =>
                    !string.IsNullOrEmpty(entry.Name)
                    && entry.FullName.StartsWith(value: "Payload/", StringComparison.Ordinal)
                    && entry.FullName.Contains(value: ".app/", StringComparison.Ordinal)
                );
            }
        }
        catch (InvalidDataException)
        {
            return false;
        }
    }

    private static (List<string> PositionalArguments, string? OutputOption) ParseDownloadArguments(string[] arguments)
    {
        List<string> positionalArguments = new(capacity: 2);
        string? outputOption = null;

        for (int index = 0; index < arguments.Length; index++)
        {
            string argument = arguments[index];

            if (string.Equals(argument, b: "-o", StringComparison.Ordinal) || string.Equals(argument, b: "--output", StringComparison.Ordinal))
            {
                if (outputOption is not null)
                    throw new ArgumentException(message: "--output may only be specified once.", nameof(arguments));

                if (++index >= arguments.Length || string.IsNullOrWhiteSpace(arguments[index]))
                    throw new ArgumentException(message: "--output requires a path.", nameof(arguments));

                outputOption = arguments[index];
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

        return positionalArguments.Count is < 1 or > 2 || positionalArguments.Exists(string.IsNullOrWhiteSpace)
            ? throw new ArgumentException(message: "Usage: SupercellProxy.Keys download APP [VERSION] [--output PATH]", nameof(arguments))
            : ((List<string> PositionalArguments, string? OutputOption))(positionalArguments, outputOption);
    }

    private static async Task<(IpaApp App, IpaDownload Download)> ResolveDownloadAsync(DecryptDayClient decryptDayClient, string appStoreIdentifier, string? requestedVersion, CancellationToken cancellationToken)
    {
        IpaApp app = await decryptDayClient
            .GetAppAsync(appStoreIdentifier, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        IReadOnlyList<AppVersion> candidateVersions;

        if (requestedVersion is null)
        {
            candidateVersions = app.Versions;
        }
        else
        {
            string normalizedVersion = AppVersion.Normalize(requestedVersion);

            AppVersion? candidateVersion = app.Versions.SingleOrDefault(version => string.Equals(version.Value, normalizedVersion, StringComparison.Ordinal));

            candidateVersions = candidateVersion is null
                ? throw new InvalidOperationException($"Version {requestedVersion} not found.")
                : [candidateVersion];
        }

        foreach (AppVersion version in candidateVersions)
        {
            IpaDownload? download = await decryptDayClient
                .TryAuthorizeAsync(appStoreIdentifier, version, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (download is not null)
                return (app, download);
        }

        throw new InvalidOperationException(requestedVersion is null ? "No downloadable version was found." : $"Version {requestedVersion} is not downloadable.");
    }

    private static async Task<int> RunDownloadAsync(string[] arguments, CancellationToken cancellationToken)
    {
        if (arguments.Any(IsHelp))
            return PrintDownloadHelp();

        (List<string>? positionalArguments, string? outputOption) = ParseDownloadArguments(arguments);
        AppStoreClient appStoreClient = new(WebClient);
        DecryptDayClient decryptDayClient = new(WebClient);

        string appStoreIdentifier = await ResolveAppStoreIdentifierAsync(positionalArguments[index: 0], appStoreClient, decryptDayClient, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        (IpaApp? app, IpaDownload? download) = await ResolveDownloadAsync(decryptDayClient, appStoreIdentifier, positionalArguments.Count is 2 ? positionalArguments[index: 1] : null, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        string outputPath = Path.GetFullPath(outputOption ?? $"{app.BundleIdentifier}-{download.Version}.ipa");

        if (Directory.Exists(outputPath))
            throw new ArgumentException($"The output path is a directory: {outputPath}", nameof(arguments));

        if (await IsValidIpaAsync(outputPath, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
        {
            Console.WriteLine($"Using cached IPA: {outputPath}");
        }
        else
        {
            await DownloadAsync(decryptDayClient, download, outputPath, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            Console.WriteLine($"Downloaded: {outputPath}");
        }

        Console.WriteLine($"Bundle ID: {app.BundleIdentifier}");
        Console.WriteLine($"Version: {download.Version}");

        return 0;
    }
}
