namespace SupercellProxy.PublicKeyExtractor.Extensions;

/// <summary>
/// <para>Provides content-loading helpers for paths and URLs.</para>
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// <para>Reads bytes from a local path or supported URL.</para>
    /// </summary>
    public static async ValueTask<byte[]> ReadContentAsync(
        this string input,
        CancellationToken cancellationToken = default
    )
    {
        if (Uri.TryCreate(input, UriKind.Absolute, out var parsedUri))
        {
            var scheme = parsedUri.Scheme;

            if (
                string.Equals(scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
                || string.Equals(scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)
            )
            {
                using var httpClient = new HttpClient();

                if (string.Equals(parsedUri.Host, "temp.sh", StringComparison.Ordinal))
                {
                    var response = await httpClient
                        .PostAsync(parsedUri, content: null, cancellationToken)
                        .ConfigureAwait(false);
                    return await response
                        .Content.ReadAsByteArrayAsync(cancellationToken)
                        .ConfigureAwait(false);
                }

                return await httpClient
                    .GetByteArrayAsync(parsedUri, cancellationToken)
                    .ConfigureAwait(false);
            }

            if (string.Equals(scheme, Uri.UriSchemeFile, StringComparison.OrdinalIgnoreCase))
            {
                return await File.ReadAllBytesAsync(parsedUri.LocalPath, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        return await File.ReadAllBytesAsync(input, cancellationToken).ConfigureAwait(false);
    }
}
