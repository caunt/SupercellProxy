using System.IO.Compression;

namespace SupercellProxy.Keys;

internal static partial class Application
{
    private static async Task<int> RunDownloadAsync(
        string[] args,
        CancellationToken cancellationToken
    )
    {
        if (args.Any(IsHelp))
            return PrintDownloadHelp();

        var (positionalArguments, outputOption) = ParseDownloadArguments(args);
        var appStoreClient = new AppStoreClient(HttpClient);
        var decryptDayClient = new DecryptDayClient(HttpClient);
        var appStoreId = await ResolveAppStoreIdAsync(
                positionalArguments[0],
                appStoreClient,
                decryptDayClient,
                cancellationToken
            )
            .ConfigureAwait(false);
        var (app, download) = await ResolveDownloadAsync(
                decryptDayClient,
                appStoreId,
                positionalArguments.Count is 2 ? positionalArguments[1] : null,
                cancellationToken
            )
            .ConfigureAwait(false);

        var outputPath = Path.GetFullPath(outputOption ?? $"{app.BundleId}-{download.Version}.ipa");

        if (Directory.Exists(outputPath))
            throw new ArgumentException(
                $"The output path is a directory: {outputPath}",
                nameof(args)
            );

        if (await IsValidIpaAsync(outputPath, cancellationToken).ConfigureAwait(false))
        {
            Console.WriteLine($"Using cached IPA: {outputPath}");
        }
        else
        {
            await DownloadAsync(decryptDayClient, download, outputPath, cancellationToken)
                .ConfigureAwait(false);
            Console.WriteLine($"Downloaded: {outputPath}");
        }

        Console.WriteLine($"Bundle ID: {app.BundleId}");
        Console.WriteLine($"Version: {download.Version}");

        return 0;
    }

    private static (List<string> PositionalArguments, string? OutputOption) ParseDownloadArguments(
        string[] args
    )
    {
        var positionalArguments = new List<string>(2);
        string? outputOption = null;
        for (var index = 0; index < args.Length; index++)
        {
            var argument = args[index];
            if (
                string.Equals(argument, "-o", StringComparison.Ordinal)
                || string.Equals(argument, "--output", StringComparison.Ordinal)
            )
            {
                if (outputOption is not null)
                    throw new ArgumentException(
                        "--output may only be specified once.",
                        nameof(args)
                    );
                if (++index >= args.Length || string.IsNullOrWhiteSpace(args[index]))
                    throw new ArgumentException("--output requires a path.", nameof(args));

                outputOption = args[index];
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

        if (
            positionalArguments.Count is < 1 or > 2
            || positionalArguments.Exists(string.IsNullOrWhiteSpace)
        )
            throw new ArgumentException(
                "Usage: SupercellProxy.Keys download APP [VERSION] [--output PATH]",
                nameof(args)
            );

        return (positionalArguments, outputOption);
    }

    private static async Task<(IpaApp App, IpaDownload Download)> ResolveDownloadAsync(
        DecryptDayClient decryptDayClient,
        string appStoreId,
        string? requestedVersion,
        CancellationToken cancellationToken
    )
    {
        var app = await decryptDayClient
            .GetAppAsync(appStoreId, cancellationToken)
            .ConfigureAwait(false);
        IReadOnlyList<AppVersion> candidateVersions;
        if (requestedVersion is null)
            candidateVersions = app.Versions;
        else
        {
            var normalizedVersion = AppVersion.Normalize(requestedVersion);
            var candidateVersion = app.Versions.SingleOrDefault(version =>
                string.Equals(version.Value, normalizedVersion, StringComparison.Ordinal)
            );
            candidateVersions = candidateVersion is null
                ? throw new InvalidOperationException($"Version {requestedVersion} not found.")
                : [candidateVersion];
        }

        foreach (var version in candidateVersions)
        {
            var download = await decryptDayClient
                .TryAuthorizeAsync(appStoreId, version, cancellationToken)
                .ConfigureAwait(false);
            if (download is not null)
                return (app, download);
        }

        throw new InvalidOperationException(
            requestedVersion is null
                ? "No downloadable version was found."
                : $"Version {requestedVersion} is not downloadable."
        );
    }

    private static async Task DownloadAsync(
        DecryptDayClient decryptDayClient,
        IpaDownload download,
        string outputPath,
        CancellationToken cancellationToken
    )
    {
        const int bufferSize = 131_072;

        var outputDirectory =
            Path.GetDirectoryName(outputPath)
            ?? throw new InvalidOperationException("Invalid output path.");
        var partialPath = outputPath + ".part";

        Directory.CreateDirectory(outputDirectory);

        try
        {
            var output = new FileStream(
                partialPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize,
                FileOptions.Asynchronous | FileOptions.SequentialScan
            );
            await using (output.ConfigureAwait(false))
            {
                await DecryptDayClient
                    .DownloadAsync(download, output, cancellationToken)
                    .ConfigureAwait(false);
            }

            if (!await IsValidIpaAsync(partialPath, cancellationToken).ConfigureAwait(false))
                throw new InvalidDataException("Downloaded file is not a valid IPA/ZIP.");

            File.Move(partialPath, outputPath, overwrite: true);
        }
        finally
        {
            File.Delete(partialPath);
        }
    }

    private static async Task<bool> IsValidIpaAsync(
        string path,
        CancellationToken cancellationToken
    )
    {
        if (!File.Exists(path))
            return false;

        try
        {
            var archive = await ZipFile
                .OpenReadAsync(path, cancellationToken)
                .ConfigureAwait(false);
            await using (archive.ConfigureAwait(false))
            {
                return archive.Entries.Any(static entry =>
                    !string.IsNullOrEmpty(entry.Name)
                    && entry.FullName.StartsWith("Payload/", StringComparison.Ordinal)
                    && entry.FullName.Contains(".app/", StringComparison.Ordinal)
                );
            }
        }
        catch (InvalidDataException)
        {
            return false;
        }
    }
}
