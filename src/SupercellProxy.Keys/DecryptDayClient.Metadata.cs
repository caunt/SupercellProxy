using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;

namespace SupercellProxy.Keys;

internal sealed partial class DecryptDayClient
{
    private async Task<DecryptDayAppDetail> GetDetailAsync(
        string appStoreId,
        CancellationToken cancellationToken)
    {
        if (details.TryGetValue(appStoreId, out var cached))
            return cached;

        using var response = await client.SendWithRetryAsync(
            () => CreateMetadataRequest(appStoreId),
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var document = await JsonNode.ParseAsync(
                           await response.Content.ReadAsStreamAsync(cancellationToken),
                           cancellationToken: cancellationToken)
                       ?? throw new InvalidDataException("decrypt.day returned empty app metadata.");

        foreach (var node in document["nodes"]?.AsArray() ?? [])
        {
            if (node?["data"] is not JsonArray values ||
                SvelteDataDecoder.Decode(values) is not JsonObject root ||
                root["app"] is not JsonObject app)
            {
                continue;
            }

            var bundleId = GetString(app, "bundle_id");

            if (bundleId is null)
                continue;

            var versions = (root["versions"] as JsonArray ?? [])
                .OfType<JsonObject>()
                .Select(version => GetString(version, "name"))
                .Where(version => !string.IsNullOrWhiteSpace(version))
                .Select(version => version!)
                .ToArray();
            var id = GetString(app, "id")
                     ?? throw new InvalidDataException("decrypt.day metadata omitted its internal app ID.");
            var detail = new DecryptDayAppDetail(id, bundleId, versions);

            details[appStoreId] = detail;
            return detail;
        }

        throw new InvalidDataException("decrypt.day did not return recognizable app metadata.");
    }

    private async Task<string?> GetFileIdAsync(
        string appStoreId,
        string decryptDayId,
        string version,
        CancellationToken cancellationToken)
    {
        using var response = await client.SendWithRetryAsync(
            () => CreateFileRequest(appStoreId, decryptDayId, version),
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(
                $"decrypt.day file lookup failed with {(int)response.StatusCode}: {body}",
                null,
                response.StatusCode);
        }

        var envelope = await JsonNode.ParseAsync(
            await response.Content.ReadAsStreamAsync(cancellationToken),
            cancellationToken: cancellationToken) as JsonObject;
        var serialized = envelope?["data"]?.GetValue<string>();

        if (serialized is null ||
            JsonNode.Parse(serialized) is not JsonArray values ||
            SvelteDataDecoder.Decode(values) is not JsonObject root ||
            root["data"]?["files"] is not JsonArray files)
        {
            throw new InvalidDataException("decrypt.day returned an unrecognized file list.");
        }

        return files
            .OfType<JsonObject>()
            .Where(file => !GetBoolean(file, "premium") && !GetBoolean(file, "login_required"))
            .Select(file => GetString(file, "id"))
            .FirstOrDefault(id => !string.IsNullOrWhiteSpace(id));
    }

    private static HttpRequestMessage CreateMetadataRequest(string appStoreId)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"https://decrypt.day/app/id{Uri.EscapeDataString(appStoreId)}/__data.json");

        request.Headers.UserAgent.ParseAdd(ApiUserAgent);
        return request;
    }

    private static HttpRequestMessage CreateFileRequest(
        string appStoreId,
        string decryptDayId,
        string version)
    {
        var boundary = $"----WebKitFormBoundary{Guid.NewGuid():N}";
        var body = $"--{boundary}\r\nContent-Disposition: form-data; name=\"data\"\r\n\r\n" +
                   $"{BuildFilePayload(decryptDayId, version)}\r\n--{boundary}--\r\n";
        var content = new ByteArrayContent(Encoding.UTF8.GetBytes(body));

        content.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
        content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("boundary", boundary));

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"https://decrypt.day/app/id{Uri.EscapeDataString(appStoreId)}?/files")
        {
            Content = content
        };

        request.Headers.UserAgent.ParseAdd(ApiUserAgent);
        request.Headers.Referrer = new Uri($"https://decrypt.day/app/id{appStoreId}");
        request.Headers.TryAddWithoutValidation("Origin", "https://decrypt.day");

        return request;
    }

    private static string BuildFilePayload(string appId, string version)
    {
        var bytes = new List<byte> { 0xA3 };

        foreach (var value in new[] { "appId", appId, "version", version, "isPremier" })
        {
            var encoded = Encoding.UTF8.GetBytes(value);

            if (encoded.Length <= 15)
            {
                bytes.Add((byte)(0x60 + encoded.Length));
            }
            else
            {
                bytes.Add(0x78);
                bytes.Add(checked((byte)encoded.Length));
            }

            bytes.AddRange(encoded);
        }

        bytes.Add(0xF7);
        return string.Join(',', bytes);
    }

    private static string? GetString(JsonObject value, string propertyName)
    {
        return value[propertyName] is JsonValue scalar && scalar.TryGetValue<string>(out var result)
            ? result
            : null;
    }

    private static bool GetBoolean(JsonObject value, string propertyName)
    {
        return value[propertyName] is JsonValue scalar &&
               scalar.TryGetValue<bool>(out var result) &&
               result;
    }
}
