using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;

using Nito.AsyncEx;

using SupercellProxy.Networking.Client;
using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.Homes;
using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Sessions;

/// <summary>Loads, validates, provides, and atomically updates multiple account sessions.</summary>
/// <remarks>Initializes a ledger at the supplied path or beside the application.</remarks>
public sealed class ClientSessionLedger(string? ledgerPath = null)
{

    /// <summary>Defines the current ledger document version.</summary>
    public const int CurrentVersion = 1;
    /// <summary>Defines the default ledger file name.</summary>
    public const string DefaultFileName = "sc-client-session-ledger.json";

    private static readonly JsonSerializerOptions DocumentSerializerOptions = CreateSerializerOptions();

    private static readonly ConcurrentDictionary<string, AsyncLock> FileGates = new(StringComparer.Ordinal);

    private readonly AsyncLock _fileGate = FileGates.GetOrAdd(ResolvePath(ledgerPath), static unusedParameter => new AsyncLock());

    /// <summary>Gets the resolved ledger path.</summary>
    public string FilePath { get; } = ResolvePath(ledgerPath);

    /// <summary>Archives the unversioned malformed ledger emitted by the previous implementation.</summary>
    /// <returns>The archive path, or null when no migration was required.</returns>
    public async Task<string?> ArchiveUnversionedAsync(CancellationToken cancellationToken = default)
    {
        using IDisposable ledgerLock = await _fileGate.LockAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if (!File.Exists(FilePath))
            return null;

        FileStream stream = File.OpenRead(FilePath);
        ClientSessionLedgerHeader header;

        await using (stream.ConfigureAwait(continueOnCapturedContext: false))
        {
            header = await JsonSerializer
                .DeserializeAsync<ClientSessionLedgerHeader>(stream, cancellationToken: cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false)
                ?? new ClientSessionLedgerHeader();
        }

        if (header.Version == CurrentVersion)
            return null;

        if (header.Version is not null)
            throw new InvalidDataException($"Client session ledger in {FilePath} has unsupported version {header.Version}.");

        string archivePath = string.Create(CultureInfo.InvariantCulture, $"{FilePath}.legacy-{DateTime.UtcNow:yyyyMMddTHHmmssZ}-{Guid.NewGuid():N}");
        File.Move(FilePath, archivePath);

        return archivePath;
    }

    /// <summary>Gets one saved account session, or null when it has not been retained.</summary>
    public async Task<ClientSession?> GetSessionAsync(LongIdentifier accountIdentifier, CancellationToken cancellationToken = default)
    {
        ClientSession[] sessions = await GetSessionsAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return sessions.FirstOrDefault(session => session.AccountIdentifier == accountIdentifier);
    }

    /// <summary>Gets every saved session in stable account-ID order.</summary>
    public async Task<ClientSession[]> GetSessionsAsync(CancellationToken cancellationToken = default)
    {
        using IDisposable ledgerLock = await _fileGate.LockAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return await LoadCoreAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    /// <summary>Adds a validated account session unless that account is already retained.</summary>
    /// <returns>True when the ledger changed; false for an existing account.</returns>
    public async Task<bool> TryAddAsync(ClientSession session, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(session);

        using IDisposable ledgerLock = await _fileGate.LockAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        ClientSessionValidation.Validate(session, FilePath);
        ClientSession[] sessions = await LoadCoreAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if (sessions.Any(existing => existing.AccountIdentifier == session.AccountIdentifier))
            return false;

        ClientSession[] updated = [.. sessions, session];
        Array.Sort(updated, static (left, right) => left.AccountIdentifier.AsUInt64.CompareTo(right.AccountIdentifier.AsUInt64));
        await SaveCoreAsync(updated, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return true;
    }

    /// <summary>Validates a completed login and own-home exchange and adds its session unless already retained.</summary>
    /// <exception cref="LoginException">The supplied login result is a failure.</exception>
    public Task<bool> TryAddAsync(
        LoginMessage loginMessage,
        IMessage loginResult,
        OwnHomeDataMessage ownHomeDataMessage,
        CancellationToken cancellationToken = default
    )
    {
        return TryAddAsync(ClientSession.FromLoginOutcome(loginMessage, loginResult, ownHomeDataMessage), cancellationToken);
    }

    /// <summary>Updates mutable metadata and token material for an existing account.</summary>
    public async Task UpdateSessionAsync(ClientSession session, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(session);

        using IDisposable ledgerLock = await _fileGate.LockAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        ClientSessionValidation.Validate(session, FilePath);
        ClientSession[] sessions = await LoadCoreAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        int index = Array.FindIndex(sessions, existing => existing.AccountIdentifier == session.AccountIdentifier);

        if (index < 0)
            throw new KeyNotFoundException($"Client session {session.AccountIdentifier.ToFormattedString()} is not present in {FilePath}.");

        ClientSession existing = sessions[index];

        if (existing.AppStore != session.AppStore || !string.Equals(existing.PassToken, session.PassToken, StringComparison.Ordinal))
            throw new UnauthorizedAccessException(message: "Updated session credentials do not match the retained account.");

        ClientSession updated = existing with
        {
            FarmName = session.FarmName,
            SessionToken = session.SessionToken ?? existing.SessionToken,
            SessionRefreshToken = session.SessionRefreshToken ?? existing.SessionRefreshToken,
        };

        bool unchanged = string.Equals(existing.FarmName, updated.FarmName, StringComparison.Ordinal)
            && string.Equals(existing.SessionToken?.Value, updated.SessionToken?.Value, StringComparison.Ordinal)
            && string.Equals(existing.SessionRefreshToken, updated.SessionRefreshToken, StringComparison.Ordinal);

        if (unchanged)
            return;

        sessions[index] = updated;
        await SaveCoreAsync(sessions, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    private static JsonSerializerOptions CreateSerializerOptions()
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = true,
        };

        options.Converters.Add(new ClientSessionAccountIdentifierConverter());
        options.Converters.Add(new ClientSessionTokenConverter());

        return options;
    }

    private static string ResolvePath(string? ledgerPath)
    {
        return ledgerPath is null
            ? Path.Combine(Environment.CurrentDirectory, DefaultFileName)
            : Path.GetFullPath(ledgerPath);
    }

    private async Task<ClientSession[]> LoadCoreAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(FilePath))
            return [];

        FileStream stream = File.OpenRead(FilePath);

        await using (stream.ConfigureAwait(continueOnCapturedContext: false))
        {
            ClientSessionLedgerDocument document = await JsonSerializer
                .DeserializeAsync<ClientSessionLedgerDocument>(stream, DocumentSerializerOptions, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false)
                ?? throw new InvalidDataException($"Failed to deserialize client session ledger from {FilePath}.");

            if (document.Version != CurrentVersion)
                throw new InvalidDataException($"Client session ledger in {FilePath} has unsupported version {document.Version}.");

            ClientSession?[] storedSessions = document.Sessions
                ?? throw new InvalidDataException($"Client session ledger in {FilePath} has no session collection.");

            ClientSession[] sessions = new ClientSession[storedSessions.Length];
            HashSet<LongIdentifier> accounts = [];

            for (int index = 0; index < storedSessions.Length; index++)
            {
                ClientSession session = storedSessions[index]
                    ?? throw new InvalidDataException($"Client session ledger in {FilePath} contains a null session.");

                ClientSessionValidation.Validate(session, FilePath);

                if (!accounts.Add(session.AccountIdentifier))
                    throw new InvalidDataException($"Client session ledger in {FilePath} contains duplicate account {session.AccountIdentifier.ToFormattedString()}.");

                sessions[index] = session;
            }

            Array.Sort(sessions, static (left, right) => left.AccountIdentifier.AsUInt64.CompareTo(right.AccountIdentifier.AsUInt64));

            return sessions;
        }
    }

    private async Task SaveCoreAsync(ClientSession[] sessions, CancellationToken cancellationToken)
    {
        string temporaryPath = string.Create(CultureInfo.InvariantCulture, $"{FilePath}.{Environment.ProcessId}.{Guid.NewGuid():N}.tmp");

        try
        {
            FileStream stream = new(
                temporaryPath,
                new FileStreamOptions
                {
                    Mode = FileMode.CreateNew,
                    Access = FileAccess.Write,
                    Share = FileShare.None,
                    Options = FileOptions.Asynchronous,
                }
            );

            await using (stream.ConfigureAwait(continueOnCapturedContext: false))
            {
                await JsonSerializer
                    .SerializeAsync(
                        stream,
                        new ClientSessionLedgerDocument
                        {
                            Version = CurrentVersion,
                            Sessions = sessions,
                        },
                        DocumentSerializerOptions,
                        cancellationToken
                    )
                    .ConfigureAwait(continueOnCapturedContext: false);
            }

            File.Move(temporaryPath, FilePath, overwrite: true);
        }
        finally
        {
            File.Delete(temporaryPath);
        }
    }
}
