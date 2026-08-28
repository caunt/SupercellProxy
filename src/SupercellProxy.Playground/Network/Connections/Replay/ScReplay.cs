using System.Buffers.Binary;
using System.Text.Json;
using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Network.Connections.Client;
using SupercellProxy.Playground.Network.Connections.Proxy;
using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Serverbound;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Connections.Replay;

internal sealed class ScReplay(string captureDirectory, string assetDirectory)
{
    private readonly string _assetDirectory = RequireDirectory(assetDirectory, "Asset");
    private readonly string _captureRoot = RequireDirectory(captureDirectory, "Capture");

    /// Runs batch replay using explicit paths or the current output directories.
    public static async Task RunAsync(
        string[] arguments,
        CancellationToken cancellationToken = default
    )
    {
        var captureRoot = arguments.ElementAtOrDefault(0) ?? ProxyTrafficCapture.RootDirectoryPath;
        var assetDirectory = arguments.ElementAtOrDefault(1) ?? ResolveAssetDirectory();
        var replay = new ScReplay(captureRoot, assetDirectory);
        var batch = await replay.RunAsync(cancellationToken).ConfigureAwait(false);
        Console.WriteLine(JsonSerializer.Serialize(batch));
    }

    /// <summary>
    /// Feeds captured clientbound frames to the local client and compares every generated
    /// serverbound frame with the capture.
    /// </summary>
    public async Task<ReplayBatch> RunAsync(CancellationToken cancellationToken = default)
    {
        var resources = await LoadReplayAssetsAsync(_assetDirectory, cancellationToken)
            .ConfigureAwait(false);
        var captureDirectories = ResolveCaptureDirectories(_captureRoot);
        var captures = new CaptureReplay[captureDirectories.Length];

        for (var i = 0; i < captureDirectories.Length; i++)
        {
            captures[i] = await RunCaptureAsync(
                    _captureRoot,
                    captureDirectories[i],
                    resources,
                    cancellationToken
                )
                .ConfigureAwait(false);
        }

        return new ReplayBatch(captures);
    }

    private static async Task<CaptureReplay> RunCaptureAsync(
        string captureRoot,
        string capturePath,
        GameAsset[] resources,
        CancellationToken cancellationToken
    )
    {
        using var messageStream = MessageStream.Create();
        messageStream.CommandDataResolver = new DataTableResolver(resources);
        var client = new ScClient(messageStream);
        var pendingResponses =
            new Queue<SupercellProxy.Playground.Network.Messages.Serverbound.EndClientTurnMessage>();
        var clientboundCount = 0;
        var serverboundCount = 0;
        var generatedResponseCount = 0;
        var logicalSequence = -1;
        string? failureDirection = null;
        ushort? messageId = null;
        ushort? messageVersion = null;
        var issues = new List<ReplayIssue>();

        try
        {
            await ReplayFramesAsync().ConfigureAwait(false);
            ValidateCompletedReplay(clientboundCount, serverboundCount, pendingResponses.Count);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            issues.Add(
                CreateIssue(
                    failureDirection ?? "capture",
                    logicalSequence,
                    messageId,
                    messageVersion,
                    exception
                )
            );
        }
        finally
        {
            await client.DisposeAsync().ConfigureAwait(false);
        }

        return new CaptureReplay(
            Path.GetRelativePath(captureRoot, capturePath),
            issues.Count is 0,
            clientboundCount,
            serverboundCount,
            generatedResponseCount,
            issues.ToArray()
        );

        async Task ReplayFramesAsync()
        {
            foreach (
                var filePath in Directory
                    .EnumerateFiles(capturePath, "*.bin", SearchOption.TopDirectoryOnly)
                    .Order(StringComparer.Ordinal)
            )
            {
                cancellationToken.ThrowIfCancellationRequested();
                await ReplayFrameAsync(filePath).ConfigureAwait(false);
            }
        }

        async Task ReplayFrameAsync(string filePath)
        {
            var isClientbound = HasDirection(filePath, Direction.Clientbound);
            var isServerbound = HasDirection(filePath, Direction.Serverbound);

            if (!isClientbound && !isServerbound)
                return;

            logicalSequence++;
            failureDirection = ProxyTrafficCapture.GetDirectionName(
                isClientbound ? Direction.Clientbound : Direction.Serverbound
            );
            messageId = null;
            messageVersion = null;

            try
            {
                var frame = await ReadReplayFrameAsync(filePath, cancellationToken)
                    .ConfigureAwait(false);
                messageId = frame.Id;
                messageVersion = frame.Version;

                if (isClientbound)
                {
                    clientboundCount++;
                    foreach (
                        var response in client.ApplyClientboundFrame(
                            frame.Id,
                            frame.Version,
                            frame.Payload
                        )
                    )
                    {
                        pendingResponses.Enqueue(response);
                        generatedResponseCount++;
                    }

                    return;
                }

                serverboundCount++;
                var generated =
                    frame.Id == MessageRegistry.GetId<EndClientTurnMessage>()
                    && pendingResponses.TryDequeue(out var pendingResponse)
                        ? pendingResponse.ToContainer(frame.Id, frame.Version)
                        : client.GenerateServerbound(frame.Id, frame.Version, frame.Payload);
                RequireExactServerbound(frame, generated, serverboundCount - 1, messageStream);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                issues.Add(
                    CreateIssue(
                        failureDirection,
                        logicalSequence,
                        messageId,
                        messageVersion,
                        exception
                    )
                );
            }
        }
    }

    private static bool HasDirection(string filePath, Direction direction)
    {
        return Path.GetFileName(filePath)
            .Contains(
                $"-incoming-{ProxyTrafficCapture.GetDirectionName(direction)}-",
                StringComparison.Ordinal
            );
    }

    private static ReplayIssue CreateIssue(
        string direction,
        int sequence,
        ushort? messageId,
        ushort? messageVersion,
        Exception exception
    )
    {
        return new ReplayIssue(
            direction,
            sequence,
            messageId,
            messageVersion,
            exception.GetType().FullName ?? exception.GetType().Name,
            exception.ToString()
        );
    }

    private static string[] ResolveCaptureDirectories(string captureRoot)
    {
        if (ContainsLogicalFrames(captureRoot))
            return [captureRoot];

        var captures = Directory
            .EnumerateDirectories(captureRoot, "*", SearchOption.AllDirectories)
            .Where(ContainsLogicalFrames)
            .Order(StringComparer.Ordinal)
            .ToArray();

        return captures.Length > 0
            ? captures
            : throw new InvalidDataException(
                $"Capture root contains no logical traffic: {captureRoot}"
            );
    }

    private static bool ContainsLogicalFrames(string path)
    {
        return Directory
                .EnumerateFiles(
                    path,
                    $"*-incoming-{ProxyTrafficCapture.GetDirectionName(Direction.Clientbound)}-*.bin",
                    SearchOption.TopDirectoryOnly
                )
                .Any()
            && Directory
                .EnumerateFiles(
                    path,
                    $"*-incoming-{ProxyTrafficCapture.GetDirectionName(Direction.Serverbound)}-*.bin",
                    SearchOption.TopDirectoryOnly
                )
                .Any();
    }

    private static void ValidateCompletedReplay(
        int clientboundCount,
        int serverboundCount,
        int pendingResponseCount
    )
    {
        if (clientboundCount is 0 || serverboundCount is 0)
            throw new InvalidDataException(
                "Capture must contain clientbound and serverbound logical frames."
            );

        if (pendingResponseCount > 0)
            throw new InvalidDataException(
                "Client generated serverbound responses missing from the capture."
            );
    }

    private static void RequireExactServerbound(
        (ushort Id, ushort Version, Memory<byte> Payload) captured,
        SupercellProxy.Playground.Network.Messages.MessageContainer generated,
        int sequence,
        MessageStream decoder
    )
    {
        if (
            captured.Id == generated.Id
            && captured.Version == generated.Version
            && captured.Payload.Span.SequenceEqual(generated.Payload.ToArray())
        )
            return;

        var checksumDetails = ResolveChecksumDetails(captured, generated, decoder);
        throw new InvalidDataException(
            string.Create(
                System.Globalization.CultureInfo.InvariantCulture,
                $"Generated serverbound frame {sequence} (message {captured.Id}, version {captured.Version}) does not match the capture.{checksumDetails}"
            )
        );
    }

    private static string ResolveChecksumDetails(
        (ushort Id, ushort Version, Memory<byte> Payload) captured,
        SupercellProxy.Playground.Network.Messages.MessageContainer generated,
        MessageStream decoder
    )
    {
        if (captured.Id != MessageRegistry.GetId<EndClientTurnMessage>())
            return string.Empty;

        var capturedTurn = ResolveTurn(captured.Id, captured.Version, captured.Payload, decoder);
        var generatedTurn = ResolveTurn(
            generated.Id,
            generated.Version,
            generated.Payload.ToArray(),
            decoder
        );
        var mismatches = Enumerable
            .Range(0, capturedTurn.SubChecksums.Length)
            .Where(index =>
                capturedTurn.SubChecksums.Span[index] != generatedTurn.SubChecksums.Span[index]
            )
            .ToArray();
        return string.Create(
            System.Globalization.CultureInfo.InvariantCulture,
            $" Checksums: captured={capturedTurn.Checksum}, generated={generatedTurn.Checksum}; mismatched sub-checksums=[{string.Join(',', mismatches)}]."
        );
    }

    private static EndClientTurnMessage ResolveTurn(
        ushort id,
        ushort version,
        ReadOnlyMemory<byte> payload,
        MessageStream decoder
    )
    {
        using var payloadStream = MessageStream.Create(payload);
        var container = new SupercellProxy.Playground.Network.Messages.MessageContainer(
            id,
            version,
            payloadStream
        );
        return decoder.ResolveMessage(container) as EndClientTurnMessage
            ?? throw new InvalidDataException("Turn frame did not decode as EndClientTurnMessage.");
    }

    private static string RequireDirectory(string path, string description)
    {
        var fullPath = Path.GetFullPath(path);
        return Directory.Exists(fullPath)
            ? fullPath
            : throw new DirectoryNotFoundException(
                $"{description} directory does not exist: {fullPath}"
            );
    }

    private static string ResolveAssetDirectory()
    {
        var assetRoot = GameAsset.RootDirectoryPath;
        var candidates = Directory.Exists(assetRoot)
            ? Directory
                .EnumerateDirectories(assetRoot, "*", SearchOption.AllDirectories)
                .Where(static path =>
                    File.Exists(Path.Combine(path, GameAssetFiles.ProductionBuildingsGoods))
                )
                .OrderByDescending(Directory.GetLastWriteTimeUtc)
                .ThenBy(static path => path, StringComparer.Ordinal)
                .ToArray()
            : [];

        return candidates.FirstOrDefault()
            ?? throw new DirectoryNotFoundException(
                $"Replay assets were not found beneath {assetRoot}."
            );
    }

    private static async Task<GameAsset[]> LoadReplayAssetsAsync(
        string assetDirectory,
        CancellationToken cancellationToken
    )
    {
        var files = Directory
            .EnumerateFiles(assetDirectory, "*", SearchOption.AllDirectories)
            .Order(StringComparer.Ordinal)
            .ToArray();
        var resources = new GameAsset[files.Length];

        for (var i = 0; i < files.Length; i++)
        {
            var relativePath = Path.GetRelativePath(assetDirectory, files[i])
                .Replace(Path.DirectorySeparatorChar, '/');
            resources[i] = new GameAsset(
                new GameAssetFingerprintEntry(relativePath, string.Empty),
                await File.ReadAllBytesAsync(files[i], cancellationToken).ConfigureAwait(false)
            );
        }

        return resources;
    }

    private static async Task<(
        ushort Id,
        ushort Version,
        Memory<byte> Payload
    )> ReadReplayFrameAsync(string filePath, CancellationToken cancellationToken)
    {
        var data = await File.ReadAllBytesAsync(filePath, cancellationToken).ConfigureAwait(false);

        if (data.Length < 7)
            throw new InvalidDataException($"Retained frame is truncated: {filePath}");

        var payloadLength = (data[2] << 16) | (data[3] << 8) | data[4];
        if (payloadLength != data.Length - 7)
            throw new InvalidDataException($"Retained frame length is invalid: {filePath}");

        return (
            BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(0, 2)),
            BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(5, 2)),
            data.AsMemory(7)
        );
    }
}
