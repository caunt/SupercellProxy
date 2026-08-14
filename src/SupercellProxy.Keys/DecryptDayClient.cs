namespace SupercellProxy.Keys;

internal sealed partial class DecryptDayClient(HttpClient client)
{
    private const string ApiUserAgent = "PlayCover/3.0 CFNetwork/1494.0.7 Darwin/23.4.0";
    private readonly Dictionary<string, DecryptDayAppDetail> details = new(StringComparer.OrdinalIgnoreCase);

    public async Task<IpaApp> GetAppAsync(string appStoreId, CancellationToken cancellationToken)
    {
        var detail = await GetDetailAsync(NormalizeAppStoreId(appStoreId), cancellationToken);
        return new IpaApp(detail.BundleId, detail.Versions);
    }

    public async Task<IpaDownload?> TryAuthorizeAsync(
        string appStoreId,
        string version,
        CancellationToken cancellationToken)
    {
        var id = NormalizeAppStoreId(appStoreId);
        var detail = await GetDetailAsync(id, cancellationToken);
        var fileId = await GetFileIdAsync(id, detail.Id, version, cancellationToken);

        if (fileId is null)
            return null;

        return new IpaDownload(
            version,
            new Uri($"https://decrypt.day/app/id{id}/dl/{Uri.EscapeDataString(fileId)}"));
    }

    private static string NormalizeAppStoreId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        var normalized = value.StartsWith("id", StringComparison.OrdinalIgnoreCase)
            ? value[2..]
            : value;

        if (normalized.Length == 0 || !normalized.All(char.IsAsciiDigit))
            throw new ArgumentException($"Invalid App Store ID: {value}", nameof(value));

        return normalized;
    }
}
