using SupercellProxy.Playground.Supercell;
using System.Text.Json;

namespace SupercellProxy.Playground.Network.Sides;

internal sealed record ScClientSession
{
    private const string FileName = "sc-client-session.json";
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { WriteIndented = true };

    public required int AccountIdHigh { get; init; }
    public required int AccountIdLow { get; init; }
    public required string PassToken { get; init; }

    internal LogicLong AccountId => new(AccountIdHigh, AccountIdLow);

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

    internal static async Task SaveAsync(LogicLong accountId, string passToken, CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(AppContext.BaseDirectory, FileName);
        var temporaryPath = $"{path}.{Environment.ProcessId}.tmp";
        var session = new ScClientSession
        {
            AccountIdHigh = accountId.HighInt32,
            AccountIdLow = accountId.LowInt32,
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
        if (AccountId == LogicLong.Empty)
            throw new InvalidDataException($"Client session in {path} has an empty account ID.");

        if (string.IsNullOrWhiteSpace(PassToken))
            throw new InvalidDataException($"Client session in {path} has an empty pass token.");
    }
}
