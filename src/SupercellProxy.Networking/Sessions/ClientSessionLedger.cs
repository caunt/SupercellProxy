using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;

using Nito.AsyncEx;

using SupercellProxy.Networking.Client;
using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Sessions;

/// <summary>Loads, validates, provides, and atomically updates multiple account sessions.</summary>
/// <remarks>Initializes a ledger at the supplied path or beside the application.</remarks>
public sealed class ClientSessionLedger(string? ledgerPath = null)
{
    /// <summary>Defines the default ledger file name.</summary>
    public const string DefaultFileName = "sc-client-session-ledger.json";

    private static readonly ConcurrentDictionary<string, AsyncLock> FileGates = new(StringComparer.Ordinal);

    private static readonly JsonSerializerOptions DocumentSerializerOptions = new()
    {
        WriteIndented = true,
    };

    private readonly AsyncLock _fileGate = FileGates.GetOrAdd(ResolvePath(ledgerPath), static unusedParameter => new AsyncLock());

    /// <summary>Gets the resolved ledger path.</summary>
    public string FilePath { get; } = ResolvePath(ledgerPath);

    /// <summary>Gets one saved account session, or null when it has not been retained.</summary>
    public async Task<ClientSession?> GetSessionAsync(LongIdentifier accountIdentifier, CancellationToken cancellationToken = default)
    {
        ClientSession[] sessions = await GetSessionsAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return sessions.FirstOrDefault(session => session.ParsedAccountIdentifier == accountIdentifier);
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

        ClientSession normalized = ClientSessionStore.Normalize(session, FilePath);
        ClientSessionStore.Validate(normalized, FilePath);
        ClientSession[] sessions = await LoadCoreAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if (sessions.Any(existing => existing.ParsedAccountIdentifier == normalized.ParsedAccountIdentifier))
            return false;

        ClientSession[] updated = [.. sessions, normalized];
        Array.Sort(updated, static (left, right) => left.ParsedAccountIdentifier.AsUInt64.CompareTo(right.ParsedAccountIdentifier.AsUInt64));
        await SaveCoreAsync(updated, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return true;
    }

    /// <summary>Validates a completed login outcome and adds its session unless the account is already retained.</summary>
    /// <exception cref="LoginException">The supplied login result is a failure.</exception>
    public Task<bool> TryAddAsync(LoginMessage loginMessage, IMessage loginResult, CancellationToken cancellationToken = default)
    {
        return TryAddAsync(ClientSession.FromLoginOutcome(loginMessage, loginResult), cancellationToken);
    }

    /// <summary>Updates refreshable token material for an existing account.</summary>
    public async Task UpdateRefreshDataAsync(ClientSession session, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(session);

        using IDisposable ledgerLock = await _fileGate.LockAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        ClientSession normalized = ClientSessionStore.Normalize(session, FilePath);
        ClientSessionStore.Validate(normalized, FilePath);
        ClientSession[] sessions = await LoadCoreAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        int index = Array.FindIndex(sessions, existing => existing.ParsedAccountIdentifier == normalized.ParsedAccountIdentifier);

        if (index < 0)
            throw new KeyNotFoundException($"Client session {normalized.AccountIdentifier} is not present in {FilePath}.");

        ClientSession existing = sessions[index];

        if (existing.AppStore != normalized.AppStore || !string.Equals(existing.PassToken, normalized.PassToken, StringComparison.Ordinal))
            throw new UnauthorizedAccessException(message: "Refreshed session credentials do not match the retained account.");

        bool unchanged = string.Equals(existing.SessionToken, normalized.SessionToken, StringComparison.Ordinal)
            && string.Equals(existing.SessionRefreshToken, normalized.SessionRefreshToken, StringComparison.Ordinal)
            && ((existing.CompressedData is null && normalized.CompressedData is null)
                || (existing.CompressedData is not null
                    && normalized.CompressedData is not null
                    && existing.CompressedData.AsSpan().SequenceEqual(normalized.CompressedData)));

        if (unchanged)
            return;

        sessions[index] = existing with
        {
            CompressedData = normalized.CompressedData,
            SessionRefreshToken = normalized.SessionRefreshToken,
            SessionToken = normalized.SessionToken,
        };
        await SaveCoreAsync(sessions, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    private static string ResolvePath(string? ledgerPath)
    {
        return ledgerPath is null
            ? Path.Combine(AppContext.BaseDirectory, DefaultFileName)
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

            ClientSession?[] storedSessions = document.Sessions
                ?? throw new InvalidDataException($"Client session ledger in {FilePath} has no session collection.");

            ClientSession[] sessions = new ClientSession[storedSessions.Length];
            HashSet<LongIdentifier> accounts = [];

            for (int index = 0; index < storedSessions.Length; index++)
            {
                ClientSession storedSession = storedSessions[index]
                    ?? throw new InvalidDataException($"Client session ledger in {FilePath} contains a null session.");

                ClientSession normalized = ClientSessionStore.Normalize(storedSession, FilePath);
                ClientSessionStore.Validate(normalized, FilePath);

                if (!accounts.Add(normalized.ParsedAccountIdentifier))
                    throw new InvalidDataException($"Client session ledger in {FilePath} contains duplicate account {normalized.AccountIdentifier}.");

                sessions[index] = normalized;
            }

            Array.Sort(sessions, static (left, right) => left.ParsedAccountIdentifier.AsUInt64.CompareTo(right.ParsedAccountIdentifier.AsUInt64));

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
                    .SerializeAsync(stream, new ClientSessionLedgerDocument { Sessions = sessions }, DocumentSerializerOptions, cancellationToken)
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
