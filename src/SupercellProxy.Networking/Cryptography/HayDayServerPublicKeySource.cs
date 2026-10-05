using System.Globalization;
using System.Net.Http.Headers;

using Nito.AsyncEx;

namespace SupercellProxy.Networking.Cryptography;

internal sealed class HayDayServerPublicKeySource(HttpClient webClient) : IServerPublicKeySource
{
    private static readonly Uri KeysAddress = new(uriString: "https://raw.githubusercontent.com/caunt/SupercellProxy/refs/heads/main/KEYS.md");
    private readonly AsyncLock _refresh = new();
    private DateTimeOffset _expiresAt;
    private HayDayServerKey? _latest;

    public async ValueTask<byte[]> GetServerPublicKeyAsync(CancellationToken cancellationToken = default)
    {
        HayDayServerKey latest = await GetLatestAsync(refresh: false, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return latest.PublicKey;
    }

    internal async ValueTask<HayDayServerKey> GetLatestAsync(bool refresh, CancellationToken cancellationToken)
    {
        using IDisposable gate = await _refresh.LockAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if (!refresh && _latest is not null && DateTimeOffset.UtcNow < _expiresAt)
            return _latest;

        HayDayServerKey? local = null;

        for (DirectoryInfo? directory = new(Environment.CurrentDirectory); directory is not null; directory = directory.Parent)
        {
            string path = Path.Combine(directory.FullName, path2: "KEYS.md");

            if (!File.Exists(path))
                continue;

            local = ReadLatest(await File.ReadAllTextAsync(path, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));

            break;
        }

        HayDayServerKey? published;

        try
        {
            using HttpRequestMessage request = new(HttpMethod.Get, KeysAddress);

            if (refresh)
                request.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };

            using HttpResponseMessage response = await webClient.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"KEYS.md request failed: {response.StatusCode}.", inner: null, response.StatusCode);

            published = ReadLatest(await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
        }
        catch (HttpRequestException) when (local is not null)
        {
            published = null;
        }

        bool useLocal = local is not null && (published is null || local.GameVersion > published.GameVersion
            || (local.GameVersion == published.GameVersion && local.KeyVersion is not null));

        _latest = (useLocal ? local : published) ?? throw new InvalidDataException(message: "The Hay Day server public key was not found.");
        _expiresAt = DateTimeOffset.UtcNow.AddMinutes(minutes: 5);

        return _latest;
    }

    private static HayDayServerKey? ReadLatest(string content)
    {
        bool selected = false;
        int versionColumn = -1;
        int keyVersionColumn = -1;
        int keyColumn = -1;
        HayDayServerKey? latest = null;

        foreach (string line in content.Split(separator: '\n'))
        {
            if (line.StartsWith(value: "##", StringComparison.Ordinal))
            {
                selected = line.Contains(value: "Hay Day", StringComparison.Ordinal);
                versionColumn = keyVersionColumn = keyColumn = -1;
            }

            if (!selected || !line.TrimStart().StartsWith(value: '|'))
                continue;

            string[] cells = line.Trim().Trim(trimChar: '|').Split(separator: '|', StringSplitOptions.TrimEntries);

            if (versionColumn < 0)
            {
                versionColumn = Array.FindIndex(cells, static cell => cell is "Game version" or "Version");
                keyVersionColumn = Array.FindIndex(cells, static cell => cell == "Key version");
                keyColumn = Array.FindIndex(cells, static cell => cell == "Key");

                continue;
            }

            if (keyColumn < 0 || cells.Length <= Math.Max(versionColumn, Math.Max(keyColumn, keyVersionColumn)))
                continue;

            string key = cells[keyColumn].Trim(trimChar: '`');

            bool valid = Version.TryParse(cells[versionColumn], out Version? version) && version.Build >= 0
                && key.Length == 64 && key.All(Uri.IsHexDigit);

            if (!valid || version is null)
                continue;

            int parsed = 0;

            bool hasKeyVersion = keyVersionColumn >= 0
                && int.TryParse(cells[keyVersionColumn], NumberStyles.None, CultureInfo.InvariantCulture, out parsed) && parsed > 0;

            int? keyVersion = hasKeyVersion ? parsed : null;

            if (latest is null || version > latest.GameVersion)
                latest = new HayDayServerKey(version, keyVersion, Convert.FromHexString(key));
        }

        return latest;
    }
}
