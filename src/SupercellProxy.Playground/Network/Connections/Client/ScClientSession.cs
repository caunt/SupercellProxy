using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Protocol;

namespace SupercellProxy.Playground.Network.Connections.Client;

internal sealed record ScClientSession
{
    internal const AppStore DefaultAppStore = AppStore.GooglePlay;

    private const string FileName = "sc-client-session.json";
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        WriteIndented = true,
    };

    public string? AccountId { get; init; }
    public AppStore AppStore { get; init; } = DefaultAppStore;
    public required string PassToken { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public byte[]? CompressedData { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AccountIdHigh { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AccountIdLow { get; init; }

    internal LongId ParsedAccountId =>
        AccountId is not null
            ? LongId.Parse(AccountId)
            : new LongId(AccountIdHigh ?? 0, AccountIdLow ?? 0);

    internal static async Task<ScClientSession?> LoadAsync(
        string? sessionPath = null,
        CancellationToken cancellationToken = default
    )
    {
        var path = ResolvePath(sessionPath);

        if (!File.Exists(path))
            return null;

        var stream = File.OpenRead(path);
        await using (stream.ConfigureAwait(false))
        {
            var session =
                await JsonSerializer
                    .DeserializeAsync<ScClientSession>(
                        stream,
                        JsonSerializerOptions,
                        cancellationToken
                    )
                    .ConfigureAwait(false)
                ?? throw new InvalidDataException(
                    $"Failed to deserialize client session from {path}."
                );

            session.Validate(path);
            return session;
        }
    }

    internal static async Task SaveAsync(
        LongId accountId,
        string passToken,
        AppStore appStore,
        Memory<byte>? compressedData,
        string? sessionPath = null,
        CancellationToken cancellationToken = default
    )
    {
        var path = ResolvePath(sessionPath);
        var temporaryPath = string.Create(
            CultureInfo.InvariantCulture,
            $"{path}.{Environment.ProcessId}.tmp"
        );
        var session = new ScClientSession
        {
            AccountId = accountId.ToFormattedString(),
            AppStore = appStore,
            PassToken = passToken,
            CompressedData = compressedData?.ToArray(),
        };

        session.Validate(path);

        var fileStreamOptions = new FileStreamOptions
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
            var stream = new FileStream(temporaryPath, fileStreamOptions);
            await using (stream.ConfigureAwait(false))
                await JsonSerializer
                    .SerializeAsync(stream, session, JsonSerializerOptions, cancellationToken)
                    .ConfigureAwait(false);

            File.Move(temporaryPath, path, overwrite: true);
        }
        finally
        {
            File.Delete(temporaryPath);
        }
    }

    private static string ResolvePath(string? sessionPath)
    {
        return sessionPath is null
            ? Path.Combine(AppContext.BaseDirectory, FileName)
            : Path.GetFullPath(sessionPath);
    }

    private void Validate(string path)
    {
        if (
            AccountId is not null
            && (
                !LongId.TryParse(AccountId, out var parsedAccountId)
                || parsedAccountId == LongId.Empty
            )
        )
            throw new InvalidDataException(
                $"Client session in {path} has an invalid account tag: {AccountId}."
            );

        if (
            AccountId is null
            && (AccountIdHigh is null || AccountIdLow is null || ParsedAccountId == LongId.Empty)
        )
            throw new InvalidDataException($"Client session in {path} has an empty account ID.");

        if (AppStore == default || !Enum.IsDefined(AppStore))
            throw new InvalidDataException(
                $"Client session in {path} has an invalid app store: {AppStore}."
            );

        if (string.IsNullOrWhiteSpace(PassToken))
            throw new InvalidDataException($"Client session in {path} has an empty pass token.");
    }
}
