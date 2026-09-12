namespace SupercellProxy.PublicKeyExtractor.Extensions;

/// <summary>
/// <para>Provides content-loading helpers for paths and URLs.</para>
/// </summary>
internal static class StringExtensions
{
    /// <summary>
    /// <para>Reads bytes from a local path or supported URL.</para>
    /// </summary>
    public static async ValueTask<byte[]> ReadContentAsync(this string input, CancellationToken cancellationToken = default)
    {
        if (Uri.TryCreate(input, UriKind.Absolute, out Uri? parsedAddress))
        {
            string scheme = parsedAddress.Scheme;

            if (string.Equals(scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase))
            {
                using HttpClient webClient = new();

                if (string.Equals(parsedAddress.Host, b: "temp.sh", StringComparison.Ordinal))
                {
                    HttpResponseMessage response = await webClient
                        .PostAsync(parsedAddress, content: null, cancellationToken)
                        .ConfigureAwait(continueOnCapturedContext: false);

                    return await response
                        .Content.ReadAsByteArrayAsync(cancellationToken)
                        .ConfigureAwait(continueOnCapturedContext: false);
                }

                return await webClient
                    .GetByteArrayAsync(parsedAddress, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }

            if (string.Equals(scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
            {
                using HttpClient webClient = new();

                if (string.Equals(parsedAddress.Host, b: "temp.sh", StringComparison.Ordinal))
                {
                    HttpResponseMessage response = await webClient
                        .PostAsync(parsedAddress, content: null, cancellationToken)
                        .ConfigureAwait(continueOnCapturedContext: false);

                    return await response
                        .Content.ReadAsByteArrayAsync(cancellationToken)
                        .ConfigureAwait(continueOnCapturedContext: false);
                }

                return await webClient
                    .GetByteArrayAsync(parsedAddress, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }

            if (string.Equals(scheme, Uri.UriSchemeFile, StringComparison.OrdinalIgnoreCase))
            {
                return await File.ReadAllBytesAsync(parsedAddress.LocalPath, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        return await File.ReadAllBytesAsync(input, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
}
