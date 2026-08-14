using SupercellProxy.Playground.Exceptions;
using SupercellProxy.Playground.Network.Captures;
using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;
using SupercellProxy.Playground.Network.Sides.Configuration;
using SupercellProxy.Playground.Network.Streams;
using SupercellProxy.Playground.Resources;
using SupercellProxy.Playground.Resources.Csv;
using SupercellProxy.Playground.Supercell;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.Json;

namespace SupercellProxy.Playground.Network.Sides;

public class ScClient(ClientConfiguration configuration) : IAsyncDisposable
{
    private static readonly TimeSpan KeepAliveInterval = TimeSpan.FromSeconds(5);
    private static readonly JsonSerializerOptions CaptureJsonSerializerOptions = new() { WriteIndented = true };

    private readonly HttpClient _httpClient = new();
    private TcpClient? tcpClient;
    private NetworkStream? _networkStream;
    private SupercellStream? _supercellStream;

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var loginOkResult = await LoginAsync(cancellationToken);

            Console.WriteLine(loginOkResult);

            if (loginOkResult.Resources.Length > 0)
                (await GetStreamAsync(cancellationToken)).CommandDataResolver = new LogicDataTableResolver(loginOkResult.Resources);

            var keepAliveTask = Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(KeepAliveInterval);

                    var stream = await GetStreamAsync(cancellationToken);
                    await stream.WriteMessageAsync(new KeepAliveMessage(), cancellationToken);
                }
            }, cancellationToken);

            HandleGoods(loginOkResult.Resources);

            while (!keepAliveTask.IsCompleted)
                await HandleIncomingMessageAsync(cancellationToken);

            await keepAliveTask;
        }
        catch (LoginException loginException)
        {
            Console.WriteLine($"Login failed: {loginException.Message}");
        }
        catch (EndOfStreamException)
        {
            Console.WriteLine("Connection closed by remote host.");
        }
    }

    public async ValueTask DisposeAsync()
    {
        await DisconnectAsync();
        GC.SuppressFinalize(this);
    }

    public async Task CaptureOtherFishingHomeAsync(LogicLong target, string outputPath, CancellationToken cancellationToken = default)
    {
        var loginOkResult = await LoginAsync(cancellationToken);
        var fingerprint = loginOkResult.Fingerprint ?? throw new InvalidOperationException("The server did not provide a content fingerprint.");
        var dataTableResolver = new LogicDataTableResolver(loginOkResult.Resources);
        var stream = await GetStreamAsync(cancellationToken);
        stream.CommandDataResolver = dataTableResolver;

        await stream.WriteMessageAsync(new VisitHomeMessage
        {
            Unknown0 = 0x01,
            Unknown1 = 0x02
        }, cancellationToken);

        await stream.WriteMessageAsync(new VisitHomeTargetMessage
        {
            Unknown0 = 0x00,
            Target = target
        }, cancellationToken);

        var otherHomeDataMessage = await stream.ReadUntilMessageAsync<OtherHomeDataMessage>(cancellationToken);

        if (!otherHomeDataMessage.Fallback.IsEmpty)
            throw new InvalidDataException($"Failed to decode {nameof(OtherHomeDataMessage)}.");

        await stream.WriteMessageAsync(new VisitHomeMessage
        {
            Unknown0 = 0x01,
            Unknown1 = 0x02
        }, cancellationToken);

        await stream.WriteMessageAsync(new VisitOtherFishingHomeMessage
        {
            Target = target
        }, cancellationToken);

        var message = await stream.ReadUntilMessageAsync<OtherFishingHomeDataMessage>(cancellationToken);

        if (!message.Fallback.IsEmpty)
            throw new InvalidDataException($"Failed to decode {nameof(OtherFishingHomeDataMessage)}.");

        var encodedPayload = message.UnknownData.ToArray();

        if (!message.RawPayload.Span.SequenceEqual(encodedPayload))
            throw new InvalidDataException($"{nameof(OtherFishingHomeDataMessage)} did not encode back to the received payload.");

        var roadsideShop = ResolveRoadsideShop(message, dataTableResolver);
        var capture = new RoadsideShopCapture(
            MessageRegistry.GetId<OtherFishingHomeDataMessage>(),
            nameof(OtherFishingHomeDataMessage),
            target.ToFormattedString(),
            message.HomeOwnerAvatar?.Name,
            $"{configuration.Protocol.MajorVersion}.{configuration.Protocol.MinorVersion}.{configuration.Protocol.PatchVersion}",
            fingerprint.Version,
            fingerprint.Sha,
            encodedPayload.Length,
            Convert.ToHexStringLower(SHA256.HashData(encodedPayload)),
            roadsideShop.Length,
            roadsideShop);

        var fullOutputPath = Path.GetFullPath(outputPath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullOutputPath) ?? throw new InvalidOperationException("The capture output path has no directory."));
        await File.WriteAllTextAsync(fullOutputPath, JsonSerializer.Serialize(capture, CaptureJsonSerializerOptions), cancellationToken);

        Console.WriteLine($"Captured {nameof(OtherFishingHomeDataMessage)} for {target} to {fullOutputPath}");
    }

    private static RoadsideShopSlotCapture[] ResolveRoadsideShop(OtherFishingHomeDataMessage message, LogicDataTableResolver dataTableResolver)
    {
        var roadsideShop = message.HomeOwnerAvatar?.RoadsideShop
            ?? throw new InvalidDataException($"{nameof(OtherFishingHomeDataMessage)} has no home owner avatar.");

        return roadsideShop.Select((entry, slot) =>
        {
            var isEmpty = entry.BuyerId is null &&
                          !entry.IsAdvertised &&
                          entry.Price is 0 &&
                          entry.Quantity is 0 &&
                          entry.ItemGlobalId is 0;

            if (!dataTableResolver.TryResolve(entry.ItemGlobalId, out var item) && !isEmpty)
                throw new InvalidDataException($"Roadside shop slot {slot} has unresolved item global ID {entry.ItemGlobalId}.");

            return new RoadsideShopSlotCapture(
                slot,
                isEmpty,
                entry.BuyerId?.ToFormattedString(),
                entry.IsAdvertised,
                entry.Price,
                entry.Quantity,
                entry.ItemGlobalId,
                item?.Name,
                item?.TableId,
                item?.RowIndex,
                item?.File);
        }).ToArray();
    }

    private async Task HandleIncomingMessageAsync(CancellationToken cancellationToken = default)
    {
        var stream = await GetStreamAsync(cancellationToken);
        var message = await stream.ReadMessageAsync(cancellationToken);
        Console.WriteLine($"Received message: {message}");
    }

    private async Task<SupercellStream> GetStreamAsync(CancellationToken cancellationToken = default)
    {
        if (_supercellStream is null)
        {
            tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(configuration.UpstreamHost, configuration.UpstreamPort, cancellationToken);

            _networkStream = tcpClient.GetStream();
            _supercellStream = new SupercellStream(_networkStream);
        }

        return _supercellStream;
    }

    private async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        if (_supercellStream is not null)
        {
            await _supercellStream.DisposeAsync();
            _supercellStream = null;
        }

        if (_networkStream is not null)
        {
            await _networkStream.DisposeAsync();
            _networkStream = null;
        }

        tcpClient?.Dispose();
        tcpClient = null;
    }

    private static void HandleGoods(Resource[] resources)
    {
        const string ProcessingBuildingsFileName = "processing_buildings.csv";

        var processingBuildingsResource = resources.FirstOrDefault(resource => resource.Fingerprint.File.EndsWith(ProcessingBuildingsFileName))
            ?? throw new InvalidOperationException($"{ProcessingBuildingsFileName} not found in resources.");

        if (!processingBuildingsResource.TryGetTable(out var processingBuildings))
            throw new InvalidOperationException($"Failed to parse {ProcessingBuildingsFileName} from resources.");

        for (var i = 0; i < processingBuildings.Entries.Count; i++)
        {
            var processingBuilding = processingBuildings.Entries[i];
            var processingBuildingName = processingBuilding.BaseRow.First(field => field.Key.Equals("Name", StringComparison.OrdinalIgnoreCase)).Value;
            var processingBuildingGoodsResource = resources.FirstOrDefault(resource => resource.Fingerprint.File.EndsWith($"{processingBuildingName}_goods.csv", StringComparison.OrdinalIgnoreCase));

            if (processingBuildingGoodsResource is null)
            {
                Console.WriteLine($"[{i}] {processingBuildingName} has no goods.");
                continue;
            }

            if (!processingBuildingGoodsResource.TryGetTable(out var goods))
            {
                Console.WriteLine($"[{i}] {processingBuildingName} can't parse goods.");
                continue;
            }

            Console.WriteLine($"[{i}] {processingBuildingName} has {goods.Entries.Count} goods => {string.Join(", ", goods.Entries.Select(good => good.BaseRow.First(field => field.Key.Equals("Name", StringComparison.OrdinalIgnoreCase)).Value))}");
        }
    }

    private async Task<LoginOkResult> LoginAsync(CancellationToken cancellationToken = default)
    {
        var session = await ScClientSession.LoadAsync(cancellationToken);
        var appStore = session?.AppStore ?? ScClientSession.DefaultAppStore;

        try
        {
            // 1.67.170 => be514e02b198d18287af1405089a0e72b849ac69
            // 1.67.175 => fdb648cea5e3494c3cafc32eca103331d85c5bfd
            // 1.69.89  => 0c95746ec8ced89978f4b9fded2fdbc95b3daf18
            // Since then, we detect the fingerprint dynamically from the login failed message
            return new LoginOkResult(await LoginCoreAsync(fingerprintSha1: string.Empty, cancellationToken));
        }
        catch (LoginException loginException) when (loginException.LoginFailedMessage.ErrorCode is LoginFailedMessage.Type.OutdatedContent)
        {
            var fingerprint = loginException.LoginFailedMessage.ResourceFingerprint;
            var resources = await GetAssetsAsync(fingerprint, loginException.LoginFailedMessage.AssetsUrlsFiltered, cancellationToken).ToArrayAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(fingerprint.Sha))
                throw new InvalidOperationException($"Failed to parse fingerprint from login failed message:\n{loginException.LoginFailedMessage.ResourceFingerprintData}", loginException);

            return new LoginOkResult(await LoginCoreAsync(fingerprint.Sha, cancellationToken), fingerprint, resources);
        }

        async Task<LoginOkMessage> LoginCoreAsync(string fingerprintSha1, CancellationToken cancellationToken = default)
        {
            try
            {
                var stream = await GetStreamAsync(cancellationToken);

                await stream.WriteMessageAsync(new ClientHelloMessage
                {
                    ProtocolVersion = configuration.Protocol.ProtocolVersion,
                    KeyVersion = configuration.Protocol.KeyVersion,

                    MajorVersion = configuration.Protocol.MajorVersion,
                    MinorVersion = configuration.Protocol.MinorVersion,
                    PatchVersion = configuration.Protocol.PatchVersion,

                    FingerprintSha1 = fingerprintSha1,

                    DeviceType = 2,
                    AppStore = appStore,
                    Unknown1 = -1
                }, cancellationToken);

                var message = await stream.ReadMessageAsync(cancellationToken);
                LoginException.ThrowIfFailed(message);

                if (message is not ServerHelloMessage serverHello)
                    throw new InvalidOperationException($"Expected {nameof(ServerHelloMessage)}, but received {message}.");

                await stream.SetupEncryptionAsync(Side.Server, serverHello.SessionKey, cancellationToken);

                await stream.WriteMessageAsync(new LoginMessage
                {
                    AccountId = session?.ParsedAccountId ?? LogicLong.Empty,
                    PassToken = session?.PassToken,
                    ResourceSha = fingerprintSha1,
                    LoginVersion = 1122388,
                    UdId = "",
                    OpenUdId = "",
                    MacAddress = "",
                    DeviceModel = "iPad9,1",
                    AdId = "",
                    IsAdTracking = false,
                    OsVersion = "18.2",
                    Locale = "",
                    Idfv = "",
                    PreferredLanguage = "",
                    ScidString = "",
                    Unknown0 = true,
                    ScIdToken = "",
                    Unknown1 = uint.MaxValue,
                    DataReference = -1,
                    Unknown2 = new byte[8]
                }, cancellationToken);

                message = await stream.ReadMessageAsync(cancellationToken);
                LoginException.ThrowIfFailed(message);

                if (message is not LoginOkMessage loginOkMessage)
                    throw new InvalidOperationException($"Expected {nameof(LoginOkMessage)}, but received {message}.");

                await ScClientSession.SaveAsync(loginOkMessage.AccountId, loginOkMessage.PassToken, appStore, cancellationToken);
                return loginOkMessage;
            }
            catch
            {
                await DisconnectAsync(cancellationToken);
                throw;
            }
        }

        async IAsyncEnumerable<Resource> GetAssetsAsync(ResourceFingerprint fingerprint, IEnumerable<string> downloadUrls, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var assetsDirectory = Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "Assets", fingerprint.Version, fingerprint.Sha));

            foreach (var file in fingerprint.Files)
            {
                var filePath = Path.Combine(assetsDirectory.FullName, file.File);

                if (File.Exists(filePath))
                {
                    yield return new Resource(file, await File.ReadAllBytesAsync(filePath, cancellationToken));
                    continue;
                }

                if (Path.GetDirectoryName(filePath) is { } directoryName)
                    _ = Directory.CreateDirectory(directoryName);

                var downloaded = false;

                foreach (var downloadUrl in downloadUrls)
                {
                    try
                    {
                        var response = await _httpClient.GetAsync($"{downloadUrl.Trim('/')}/{fingerprint.Sha.Trim('/')}/{file.File.Trim('/')}", cancellationToken);
                        response.EnsureSuccessStatusCode();

                        await using var fileStream = File.Create(filePath);
                        await using var httpStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                        await httpStream.CopyToAsync(fileStream, cancellationToken);

                        downloaded = true;
                    }
                    catch (Exception exception) when (exception is HttpRequestException || exception is IOException)
                    {
                        Console.WriteLine($"Failed to download {file.File} from {downloadUrl}: {exception.Message}");
                    }

                    if (!downloaded)
                        continue;

                    Console.WriteLine($"Downloaded {file.File} from {downloadUrl}");
                    yield return new Resource(file, await File.ReadAllBytesAsync(filePath, cancellationToken));
                    break;
                }
            }
        }
    }

    private record LoginOkResult(LoginOkMessage LoginOkMessage, ResourceFingerprint? Fingerprint, Resource[] Resources)
    {
        public LoginOkResult(LoginOkMessage loginOkMessage) : this(loginOkMessage, null, [])
        {
            // Empty
        }
    }
}
