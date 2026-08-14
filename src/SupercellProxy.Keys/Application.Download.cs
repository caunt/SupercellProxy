using System.IO.Compression;

namespace SupercellProxy.Keys;

internal static partial class Application
{
    private static async Task<int> RunDownloadAsync(
        string[] args,
        CancellationToken cancellationToken)
    {
        if (args.Any(IsHelp))
            return PrintDownloadHelp();

        var positionalArguments = new List<string>(2);
        string? outputOption = null;

        for (var index = 0; index < args.Length; index++)
        {
            var argument = args[index];

            if (argument is "-o" or "--output")
            {
                if (outputOption is not null)
                    throw new ArgumentException("--output may only be specified once.");

                index++;

                if (index >= args.Length || string.IsNullOrWhiteSpace(args[index]))
                    throw new ArgumentException("--output requires a path.");

                outputOption = args[index];
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

        if (positionalArguments.Count is < 1 or > 2 ||
            positionalArguments.Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException(
                "Usage: SupercellProxy.Keys download APP [VERSION] [--output PATH]");
        }

        var appStoreClient = new AppStoreClient(HttpClient);
        var decryptDayClient = new DecryptDayClient(HttpClient);
        var appStoreId = await ResolveAppStoreIdAsync(
            positionalArguments[0],
            appStoreClient,
            decryptDayClient,
            cancellationToken);
        var app = await decryptDayClient.GetAppAsync(appStoreId, cancellationToken);
        var requestedVersion = positionalArguments.Count == 2 ? positionalArguments[1] : null;
        IReadOnlyList<string> candidateVersions;

        if (requestedVersion is null)
        {
            candidateVersions = app.Versions;
        }
        else if (app.Versions.Contains(requestedVersion, StringComparer.Ordinal))
        {
            candidateVersions = [requestedVersion];
        }
        else
        {
            throw new InvalidOperationException($"Version {requestedVersion} not found.");
        }

        IpaDownload? download = null;

        foreach (var version in candidateVersions)
        {
            download = await decryptDayClient.TryAuthorizeAsync(
                appStoreId,
                version,
                cancellationToken);

            if (download is not null)
                break;
        }

        if (download is null)
        {
            throw new InvalidOperationException(requestedVersion is null
                ? "No downloadable version was found."
                : $"Version {requestedVersion} is not downloadable.");
        }

        var outputPath = Path.GetFullPath(outputOption ?? $"{app.BundleId}-{download.Version}.ipa");

        if (Directory.Exists(outputPath))
            throw new ArgumentException($"The output path is a directory: {outputPath}");

        if (IsValidIpa(outputPath))
        {
            Console.WriteLine($"Using cached IPA: {outputPath}");
        }
        else
        {
            await DownloadAsync(decryptDayClient, download, outputPath, cancellationToken);
            Console.WriteLine($"Downloaded: {outputPath}");
        }

        Console.WriteLine($"Bundle ID: {app.BundleId}");
        Console.WriteLine($"Version: {download.Version}");

        return 0;
    }

    private static async Task DownloadAsync(
        DecryptDayClient decryptDayClient,
        IpaDownload download,
        string outputPath,
        CancellationToken cancellationToken)
    {
        const int BufferSize = 131_072;

        var outputDirectory = Path.GetDirectoryName(outputPath)
                              ?? throw new InvalidOperationException("Invalid output path.");
        var partialPath = outputPath + ".part";

        Directory.CreateDirectory(outputDirectory);

        try
        {
            await using (var output = new FileStream(
                             partialPath,
                             FileMode.Create,
                             FileAccess.Write,
                             FileShare.None,
                             BufferSize,
                             FileOptions.Asynchronous | FileOptions.SequentialScan))
            {
                await decryptDayClient.DownloadAsync(download, output, cancellationToken);
            }

            if (!IsValidIpa(partialPath))
                throw new InvalidDataException("Downloaded file is not a valid IPA/ZIP.");

            File.Move(partialPath, outputPath, overwrite: true);
        }
        finally
        {
            File.Delete(partialPath);
        }
    }

    private static bool IsValidIpa(string path)
    {
        if (!File.Exists(path))
            return false;

        try
        {
            using var archive = ZipFile.OpenRead(path);

            return archive.Entries.Any(entry =>
                !string.IsNullOrEmpty(entry.Name) &&
                entry.FullName.StartsWith("Payload/", StringComparison.Ordinal) &&
                entry.FullName.Contains(".app/", StringComparison.Ordinal));
        }
        catch (InvalidDataException)
        {
            return false;
        }
    }
}
