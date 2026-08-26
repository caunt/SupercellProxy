using System.Globalization;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using SupercellProxy.Playground.Commands;
using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Home;
using SupercellProxy.Playground.Home.Simulation;
using SupercellProxy.Playground.Json;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Configuration;
using SupercellProxy.Playground.Network.Connections.Client.Exceptions;
using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;
using SupercellProxy.Playground.Network.Protocol;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Connections.Client;

public partial class ScClient
{
    private static void HandleGoods(GameAsset[] resources)
    {
        const string ProcessingBuildingsFileName = "processing_buildings.csv";

        var processingBuildingsResource =
            resources.FirstOrDefault(static resource =>
                resource.Fingerprint.File.EndsWith(
                    ProcessingBuildingsFileName,
                    StringComparison.Ordinal
                )
            )
            ?? throw new InvalidOperationException(
                $"{ProcessingBuildingsFileName} not found in resources."
            );

        if (!processingBuildingsResource.TryGetTable(out var processingBuildings))
            throw new InvalidOperationException(
                $"Failed to parse {ProcessingBuildingsFileName} from resources."
            );

        for (var i = 0; i < processingBuildings.Entries.Count; i++)
        {
            var processingBuilding = processingBuildings.Entries[i];
            var processingBuildingNameValue = processingBuilding
                .BaseRow.First(static field =>
                    field.Key.Equals("Name", StringComparison.OrdinalIgnoreCase)
                )
                .Value;
            if (processingBuildingNameValue is not string processingBuildingName)
                throw new InvalidDataException("Processing building name must be a string.");

            DescribeProcessingBuilding(resources, i, processingBuildingName);
        }
    }

    private static void DescribeProcessingBuilding(
        GameAsset[] resources,
        int index,
        string processingBuildingName
    )
    {
        var processingBuildingGoodsResource = resources.FirstOrDefault(resource =>
            resource.Fingerprint.File.EndsWith(
                $"{processingBuildingName}_goods.csv",
                StringComparison.OrdinalIgnoreCase
            )
        );

        if (processingBuildingGoodsResource is null)
        {
            Console.WriteLine(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"[{index}] {processingBuildingName} has no goods."
                )
            );
            return;
        }

        if (!processingBuildingGoodsResource.TryGetTable(out var goods))
        {
            Console.WriteLine(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"[{index}] {processingBuildingName} can't parse goods."
                )
            );
            return;
        }

        Console.WriteLine(
            string.Create(
                CultureInfo.InvariantCulture,
                $"[{index}] {processingBuildingName} has {goods.Entries.Count} goods => {string.Join(", ", goods.Entries.Select(static good => good.BaseRow.First(static field => field.Key.Equals("Name", StringComparison.OrdinalIgnoreCase)).Value))}"
            )
        );
    }

    private async Task<GameAsset[]> GetAssetsAsync(
        GameAssetFingerprint fingerprint,
        IEnumerable<string> downloadUrls,
        CancellationToken cancellationToken = default
    )
    {
        var assetsDirectory = Directory.CreateDirectory(
            Path.Combine(AppContext.BaseDirectory, "Assets", fingerprint.Version, fingerprint.Sha)
        );
        var resources = new List<GameAsset>();

        foreach (var file in fingerprint.Files)
        {
            var filePath = Path.Combine(assetsDirectory.FullName, file.File);
            var resource = await GetAssetAsync(
                    fingerprint,
                    file,
                    filePath,
                    downloadUrls,
                    cancellationToken
                )
                .ConfigureAwait(false);
            if (resource is not null)
                resources.Add(resource);
        }

        return resources.ToArray();
    }

    private async Task<GameAsset?> GetAssetAsync(
        GameAssetFingerprint fingerprint,
        GameAssetFingerprintEntry file,
        string filePath,
        IEnumerable<string> downloadUrls,
        CancellationToken cancellationToken
    )
    {
        if (File.Exists(filePath))
            return await ReadAssetAsync(file, filePath, cancellationToken).ConfigureAwait(false);

        if (Path.GetDirectoryName(filePath) is { } directoryName)
            _ = Directory.CreateDirectory(directoryName);

        foreach (var downloadUrl in downloadUrls)
        {
            if (
                !await TryDownloadAssetAsync(
                        fingerprint,
                        file,
                        filePath,
                        downloadUrl,
                        cancellationToken
                    )
                    .ConfigureAwait(false)
            )
                continue;

            Console.WriteLine($"Downloaded {file.File} from {downloadUrl}");
            return await ReadAssetAsync(file, filePath, cancellationToken).ConfigureAwait(false);
        }

        return null;
    }

    private async Task<bool> TryDownloadAssetAsync(
        GameAssetFingerprint fingerprint,
        GameAssetFingerprintEntry file,
        string filePath,
        string downloadUrl,
        CancellationToken cancellationToken
    )
    {
        try
        {
            using var response = await _httpClient
                .GetAsync(
                    $"{downloadUrl.Trim('/')}/{fingerprint.Sha.Trim('/')}/{file.File.Trim('/')}",
                    cancellationToken
                )
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var fileStream = File.Create(filePath);
            await using (fileStream.ConfigureAwait(false))
            {
                await response
                    .Content.CopyToAsync(fileStream, cancellationToken)
                    .ConfigureAwait(false);
            }
            return true;
        }
        catch (Exception exception)
            when (exception is HttpRequestException || exception is IOException)
        {
            Console.WriteLine(
                $"Failed to download {file.File} from {downloadUrl}: {exception.Message}"
            );
            return false;
        }
    }

    private static async Task<GameAsset> ReadAssetAsync(
        GameAssetFingerprintEntry file,
        string filePath,
        CancellationToken cancellationToken
    )
    {
        return new GameAsset(
            file,
            await File.ReadAllBytesAsync(filePath, cancellationToken).ConfigureAwait(false)
        );
    }
}
