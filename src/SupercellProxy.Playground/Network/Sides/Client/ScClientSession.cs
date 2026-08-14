using SupercellProxy.Playground.Supercell;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Network.Sides;

internal sealed record ScClientSession
{
    internal const AppStore DefaultAppStore = AppStore.GooglePlay;

    private const string FileName = "sc-client-session.json";
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { WriteIndented = true };

    public string? AccountId { get; init; }
    public AppStore AppStore { get; init; } = DefaultAppStore;
    public required string PassToken { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AccountIdHigh { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AccountIdLow { get; init; }

    internal LogicLong ParsedAccountId => AccountId is not null
        ? LogicLong.Parse(AccountId)
        : new LogicLong(AccountIdHigh ?? 0, AccountIdLow ?? 0);

    internal static async Task<ScClientSession?> LoadAsync(CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(AppContext.BaseDirectory, FileName);

        if (!File.Exists(path))
            return null;

        await using var stream = File.OpenRead(path);
        var session = await JsonSerializer.DeserializeAsync<ScClientSession>(stream, JsonSerializerOptions, cancellationToken)
            ?? throw new InvalidDataException($"Failed to deserialize client session from {path}.");

        session.Validate(path);
        return session;
    }

    internal static async Task SaveAsync(LogicLong accountId, string passToken, AppStore appStore, CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(AppContext.BaseDirectory, FileName);
        var temporaryPath = $"{path}.{Environment.ProcessId}.tmp";
        var session = new ScClientSession
        {
            AccountId = accountId.ToFormattedString(),
            AppStore = appStore,
            PassToken = passToken
        };

        session.Validate(path);

        var fileStreamOptions = new FileStreamOptions
        {
            Mode = FileMode.Create,
            Access = FileAccess.Write,
            Share = FileShare.None,
            Options = FileOptions.Asynchronous
        };

        if (!OperatingSystem.IsWindows())
            fileStreamOptions.UnixCreateMode = UnixFileMode.UserRead | UnixFileMode.UserWrite;

        try
        {
            await using (var stream = new FileStream(temporaryPath, fileStreamOptions))
                await JsonSerializer.SerializeAsync(stream, session, JsonSerializerOptions, cancellationToken);

            File.Move(temporaryPath, path, overwrite: true);
        }
        finally
        {
            File.Delete(temporaryPath);
        }
    }

    private void Validate(string path)
    {
        if (AccountId is not null && (!LogicLong.TryParse(AccountId, out var parsedAccountId) || parsedAccountId == LogicLong.Empty))
            throw new InvalidDataException($"Client session in {path} has an invalid account tag: {AccountId}.");

        if (AccountId is null && (AccountIdHigh is null || AccountIdLow is null || ParsedAccountId == LogicLong.Empty))
            throw new InvalidDataException($"Client session in {path} has an empty account ID.");

        if (AppStore == default || !Enum.IsDefined(AppStore))
            throw new InvalidDataException($"Client session in {path} has an invalid app store: {AppStore}.");

        if (string.IsNullOrWhiteSpace(PassToken))
            throw new InvalidDataException($"Client session in {path} has an empty pass token.");
    }
}
