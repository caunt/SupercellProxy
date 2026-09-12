using System.Globalization;
using System.Text.Json;

using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.Authentication;

namespace SupercellProxy.Networking.Sessions;

/// <summary>Loads, validates, and atomically saves client session files using the established on-disk format.</summary>
public static class ClientSessionStore
{

    private const string FileName = "sc-client-session.json";

    private static readonly JsonSerializerOptions DocumentSerializerOptions = new()
    {
        WriteIndented = true,
    };

    /// <summary>
    /// Provides the Load Async value or operation.
    /// </summary>
    public static async Task<ClientSession?> LoadAsync(string? sessionPath = null, CancellationToken cancellationToken = default)
    {
        string path = ResolvePath(sessionPath);

        if (!File.Exists(path))
            return null;

        FileStream stream = File.OpenRead(path);

        await using (stream.ConfigureAwait(continueOnCapturedContext: false))
        {
            ClientSession session =
                await JsonSerializer
                    .DeserializeAsync<ClientSession>(stream, DocumentSerializerOptions, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false)
                ?? throw new InvalidDataException($"Failed to deserialize client session from {path}.");

            ClientSession normalized = Normalize(session, path);
            Validate(normalized, path);

            return normalized;
        }
    }

    /// <summary>
    /// Provides the Save Async value or operation.
    /// </summary>
    public static async Task SaveAsync(
        LongIdentifier accountIdentifier,
        string passToken,
        AppStore appStore,
        Memory<byte>? compressedData,
        string? sessionPath = null,
        string? sessionRefreshToken = null,
        CancellationToken cancellationToken = default
    )
    {
        string path = ResolvePath(sessionPath);

        string temporaryPath = string.Create(CultureInfo.InvariantCulture, $"{path}.{Environment.ProcessId}.tmp");

        sessionRefreshToken ??= await RetainRefreshTokenAsync(path, accountIdentifier, passToken, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        ClientSession session = Normalize(
            new ClientSession
            {
                AccountIdentifier = accountIdentifier.ToFormattedString(),
                AppStore = appStore,
                PassToken = passToken,
                CompressedData = compressedData?.ToArray(),
                SessionRefreshToken = sessionRefreshToken,
            },
            path
        );

        Validate(session, path);

        FileStreamOptions fileStreamOptions = new()
        {
            Mode = FileMode.Create,
            Access = FileAccess.Write,
            Share = FileShare.None,
            Options = FileOptions.Asynchronous,
        };

        if (!OperatingSystem.IsWindows())
            fileStreamOptions.UnixCreateMode = UnixFileMode.UserRead | UnixFileMode.UserWrite;

        try
        {
            FileStream stream = new(temporaryPath, fileStreamOptions);

            await using (stream.ConfigureAwait(continueOnCapturedContext: false))
            {
                await JsonSerializer
                    .SerializeAsync(stream, session, DocumentSerializerOptions, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }

            File.Move(temporaryPath, path, overwrite: true);
        }
        finally
        {
            File.Delete(temporaryPath);
        }
    }

    private static ClientSession Normalize(ClientSession session, string path)
    {
        if (session.CompressedData is null)
        {
            if (session.SessionToken is not { } sessionToken)
                return session;

            try
            {
                LoginSessionToken encoded = LoginSessionToken.Decode(sessionToken);

                return session with { CompressedData = encoded.Encode() };
            }
            catch (InvalidDataException exception)
            {
                throw new InvalidDataException($"Client session in {path} has invalid decoded session data.", exception);
            }
        }

        LoginSessionToken decoded;

        try
        {
            decoded = LoginSessionToken.Decode(session.CompressedData);
        }
        catch (InvalidDataException exception)
        {
            throw new InvalidDataException($"Client session in {path} has invalid compressed session data.", exception);
        }

        return session.SessionToken is not null && !string.Equals(session.SessionToken, decoded.Value, StringComparison.Ordinal)
            ? throw new InvalidDataException($"Client session in {path} has conflicting decoded and compressed session data.")
            : (session with
            {
                SessionToken = decoded.Value,
            });
    }

    private static string ResolvePath(string? sessionPath)
    {
        return sessionPath is null
            ? Path.Combine(AppContext.BaseDirectory, FileName)
            : Path.GetFullPath(sessionPath);
    }

    private static async Task<string?> RetainRefreshTokenAsync(string path, LongIdentifier accountIdentifier, string passToken, CancellationToken cancellationToken)
    {
        ClientSession? existing = await LoadAsync(path, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        bool matchesSession = existing is not null
            && existing.ParsedAccountIdentifier == accountIdentifier
            && string.Equals(existing.PassToken, passToken, StringComparison.Ordinal);

        return matchesSession ? existing?.SessionRefreshToken : null;
    }

    private static void Validate(ClientSession session, string path)
    {
        bool invalidTag = session.AccountIdentifier is not null
            && (
                !LongIdentifier.TryParse(session.AccountIdentifier, out LongIdentifier parsedAccountIdentifier)
                || parsedAccountIdentifier == LongIdentifier.Empty
            );

        if (invalidTag)
            throw new InvalidDataException($"Client session in {path} has an invalid account tag: {session.AccountIdentifier}.");

        bool emptyIdentifier = session.AccountIdentifier is null
            && (session.AccountIdentifierHigh is null || session.AccountIdentifierLow is null || session.ParsedAccountIdentifier == LongIdentifier.Empty);

        if (emptyIdentifier)
            throw new InvalidDataException($"Client session in {path} has an empty account ID.");

        if (session.AppStore == default || !Enum.IsDefined(session.AppStore))
            throw new InvalidDataException($"Client session in {path} has an invalid app store: {session.AppStore}.");

        if (string.IsNullOrWhiteSpace(session.PassToken))
            throw new InvalidDataException($"Client session in {path} has an empty pass token.");

        if (session.SessionRefreshToken is not null && string.IsNullOrWhiteSpace(session.SessionRefreshToken))
            throw new InvalidDataException($"Client session in {path} has an empty Supercell ID refresh token.");
    }

}
