using System.Buffers.Binary;
using System.Globalization;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SupercellProxy.Networking.Client;
using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.Homes;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Proxy;
using SupercellProxy.Networking.Sessions;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Capture;

internal sealed class CaptureSessionImportService : IHostedLifecycleService
{
    private readonly CaptureAccountSelection _accountSelection;
    private readonly ILogger<CaptureSessionImportService> _logger;
    private readonly ProxyOptions _options;
    private readonly ProtocolClientFactory _protocolClients;

    internal CaptureSessionImportService(
        IOptions<ProxyOptions> options,
        ProtocolClientFactory protocolClients,
        CaptureAccountSelection accountSelection,
        ILogger<CaptureSessionImportService> logger
    )
    {
        _accountSelection = accountSelection;
        _logger = logger;
        _options = options.Value;
        _protocolClients = protocolClients;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StartedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task StartingAsync(CancellationToken cancellationToken)
    {
        ClientSessionLedger ledger = new(_options.SessionLedgerPath);

        string? archivePath = await ledger.ArchiveUnversionedAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (archivePath is not null)
            CaptureSessionImportLog.Archived(_logger, ledger.FilePath, archivePath);

        ClientSession[] initialSessions = await ledger.GetSessionsAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        int importedSessionCount = 0;

        importedSessionCount += await ImportRetainedCapturesAsync(ledger, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (_accountSelection.Specified)
        {
            ClientSessionSelector selector = new(Console.In, Console.Out);

            ClientSession selected = await selector
                .SelectAsync(
                    ledger,
                    accountOptionSpecified: true,
                    _accountSelection.FarmNameFragment,
                    selectionRequired: false,
                    Console.IsInputRedirected,
                    cancellationToken
                )
                .ConfigureAwait(continueOnCapturedContext: false)
                ?? throw new InvalidOperationException(message: "Account selection did not return a session.");

            _options.SessionAccountIdentifier = selected.AccountIdentifier.ToFormattedString();
        }

        await ValidateSelectedProxySessionAsync(ledger, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        CaptureSessionImportLog.Completed(_logger, ledger.FilePath, initialSessions.Length, importedSessionCount);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StoppedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StoppingAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private static string? FindOutcomeFile(string[] orderedFiles, int loginIndex)
    {
        ushort loginIdentifier = MessageRegistry.GetIdentifier<LoginMessage>();
        ushort loginOkIdentifier = MessageRegistry.GetIdentifier<LoginOkMessage>();
        ushort loginFailedIdentifier = MessageRegistry.GetIdentifier<LoginFailedMessage>();
        string loginMarker = string.Create(CultureInfo.InvariantCulture, $"-outgoing-serverbound-{loginIdentifier}-");
        string loginOkMarker = string.Create(CultureInfo.InvariantCulture, $"-outgoing-clientbound-{loginOkIdentifier}-");
        string loginFailedMarker = string.Create(CultureInfo.InvariantCulture, $"-outgoing-clientbound-{loginFailedIdentifier}-");

        for (int index = loginIndex + 1; index < orderedFiles.Length; index++)
        {
            string name = Path.GetFileName(orderedFiles[index]);

            if (name.Contains(loginMarker, StringComparison.Ordinal))
                return null;

            bool isLoginOutcome = name.Contains(loginOkMarker, StringComparison.Ordinal)
                || name.Contains(loginFailedMarker, StringComparison.Ordinal);

            if (isLoginOutcome)
                return orderedFiles[index];
        }

        return null;
    }

    private static string? FindOwnHomeFile(string[] orderedFiles, int outcomeIndex)
    {
        ushort loginIdentifier = MessageRegistry.GetIdentifier<LoginMessage>();
        ushort ownHomeIdentifier = MessageRegistry.GetIdentifier<OwnHomeDataMessage>();
        string loginMarker = string.Create(CultureInfo.InvariantCulture, $"-outgoing-serverbound-{loginIdentifier}-");
        string ownHomeMarker = string.Create(CultureInfo.InvariantCulture, $"-outgoing-clientbound-{ownHomeIdentifier}-");
        string ownHomeName = MessageRegistry.Registrations[ownHomeIdentifier].CaptureName;

        for (int index = outcomeIndex + 1; index < orderedFiles.Length; index++)
        {
            string name = Path.GetFileName(orderedFiles[index]);

            if (name.Contains(loginMarker, StringComparison.Ordinal))
                return null;

            if (IsCapturedMessage(orderedFiles[index], ownHomeMarker, ownHomeName))
                return orderedFiles[index];
        }

        return null;
    }

    private static bool IsCapturedMessage(string path, string marker, string captureName)
    {
        string name = Path.GetFileName(path);

        return name.Contains(marker, StringComparison.Ordinal)
            && name.EndsWith($"-{captureName}.bin", StringComparison.Ordinal);
    }

    private static async Task<CapturedLoginExchange> ReadLoginExchangeAsync(string loginFile, CancellationToken cancellationToken)
    {
        string directory = Path.GetDirectoryName(loginFile)
            ?? throw new InvalidDataException($"Retained login frame has no capture directory: {loginFile}");

        string[] orderedFiles = [.. Directory
            .EnumerateFiles(directory, searchPattern: "*.bin", SearchOption.TopDirectoryOnly)
            .Order(StringComparer.Ordinal)];

        int loginIndex = Array.IndexOf(orderedFiles, loginFile);

        if (loginIndex < 0)
            throw new InvalidDataException($"Retained login frame disappeared during import: {loginFile}");

        string? outcomeFile = FindOutcomeFile(orderedFiles, loginIndex) ?? throw new InvalidDataException($"Retained login has no following login outcome: {loginFile}");
        IMessage loginMessage = await ReadMessageAsync(loginFile, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        IMessage outcome = await ReadMessageAsync(outcomeFile, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        LoginException.ThrowIfFailed(outcome);
        int outcomeIndex = Array.IndexOf(orderedFiles, outcomeFile);

        string ownHomeFile = FindOwnHomeFile(orderedFiles, outcomeIndex)
            ?? throw new InvalidDataException($"Retained login has no following own-home data: {loginFile}");

        IMessage ownHomeMessage = await ReadMessageAsync(ownHomeFile, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return ownHomeMessage is not OwnHomeDataMessage ownHomeDataMessage
            ? throw new InvalidDataException($"Retained own-home frame did not decode as {nameof(OwnHomeDataMessage)}: {ownHomeFile}")
            : loginMessage is LoginMessage login
            ? new CapturedLoginExchange(login, outcome, ownHomeDataMessage)
            : throw new InvalidDataException($"Retained login frame did not decode as {nameof(LoginMessage)}: {loginFile}");
    }

    private static async Task<IMessage> ReadMessageAsync(string path, CancellationToken cancellationToken)
    {
        byte[] data = await File.ReadAllBytesAsync(path, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if (data.Length < 7)
            throw new InvalidDataException($"Retained frame is truncated: {path}");

        int payloadLength = (data[2] << 16) | (data[3] << 8) | data[4];

        if (payloadLength != data.Length - 7)
            throw new InvalidDataException($"Retained frame length is invalid: {path}");

        using MessageStream payload = MessageStream.Create(data.AsMemory(start: 7));

        ushort identifier = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(start: 0, length: 2));
        ushort version = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(start: 5, length: 2));
        IMessage message = MessageRegistry.Resolve(new MessageContainer(identifier, version, payload));

        return payload.Position != payload.Length
            ? throw new InvalidDataException($"Retained frame contains trailing payload data: {path}")
            : message;
    }

    private async Task<bool> ImportCandidateAsync(ClientSessionLedger ledger, ClientSession candidate, CancellationToken cancellationToken)
    {
        ClientSession? existing = await ledger.GetSessionAsync(candidate.AccountIdentifier, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (existing is not null)
            return false;

        ClientConfiguration configuration = new(
            _options.UpstreamHost,
            _options.UpstreamPort,
            _options.Protocol,
            candidate.AccountIdentifier.ToFormattedString(),
            ledger.FilePath,
            BootstrapFingerprintSha: null,
            AssetDirectory: _options.AssetDirectory
        );

        ProtocolClient client = _protocolClients.Create(configuration);

        await using (client.ConfigureAwait(continueOnCapturedContext: false))
        {
            ClientSession validated = await client.ValidateSessionAsync(candidate, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            return await ledger.TryAddAsync(validated, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    private async Task<int> ImportRetainedCapturesAsync(ClientSessionLedger ledger, CancellationToken cancellationToken)
    {
        if (_options.CaptureDirectory is not { } captureDirectory || !Directory.Exists(captureDirectory))
            return 0;

        ushort loginIdentifier = MessageRegistry.GetIdentifier<LoginMessage>();
        string loginName = MessageRegistry.Registrations[loginIdentifier].CaptureName;
        string loginMarker = string.Create(CultureInfo.InvariantCulture, $"-outgoing-serverbound-{loginIdentifier}-");
        string[] loginFiles;

        try
        {
            loginFiles = [.. Directory
                .EnumerateFiles(captureDirectory, searchPattern: "*.bin", SearchOption.AllDirectories)
                .Where(path => IsCapturedMessage(path, loginMarker, loginName))
                .OrderByDescending(File.GetLastWriteTimeUtc)
                .ThenByDescending(static path => path, StringComparer.Ordinal)];
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            CaptureSessionImportLog.Warning(_logger, captureDirectory, exception);

            return 0;
        }

        int importedSessionCount = 0;

        foreach (string loginFile in loginFiles)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                CapturedLoginExchange exchange = await ReadLoginExchangeAsync(loginFile, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);

                ClientSession candidate = ClientSession.FromLoginOutcome(exchange.Login, exchange.Outcome, exchange.OwnHomeData);

                bool imported = await ImportCandidateAsync(ledger, candidate, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);

                if (imported)
                    importedSessionCount++;
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                CaptureSessionImportLog.Warning(_logger, loginFile, exception);
            }
        }

        return importedSessionCount;
    }

    private async Task ValidateSelectedProxySessionAsync(ClientSessionLedger ledger, CancellationToken cancellationToken)
    {
        if (_options.SessionAccountIdentifier is not { } selectedAccount)
            return;

        bool validAccountIdentifier = LongIdentifier.TryParse(selectedAccount, out LongIdentifier accountIdentifier)
            && accountIdentifier != LongIdentifier.Empty;

        if (!validAccountIdentifier)
            throw new InvalidDataException(message: "The selected proxy session account identifier is invalid.");

        ClientSession? selectedSession = await ledger.GetSessionAsync(accountIdentifier, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (selectedSession?.AccountIdentifier != accountIdentifier)
            throw new InvalidDataException($"The selected proxy session {selectedAccount} does not exist in {ledger.FilePath}.");
    }
}
