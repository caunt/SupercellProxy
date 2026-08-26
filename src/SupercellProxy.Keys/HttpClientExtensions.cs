using System.Diagnostics;
using System.Net;

namespace SupercellProxy.Keys;

internal static class HttpClientExtensions
{
    private const int MaximumAttempts = 3;

    public static async Task<HttpResponseMessage> SendWithRetryAsync(
        this HttpClient client,
        Func<HttpRequestMessage> requestFactory,
        CancellationToken cancellationToken
    )
    {
        for (var attempt = 1; attempt <= MaximumAttempts; attempt++)
        {
            using var request = requestFactory();
            var response = await client
                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);

            if (!IsTransient(response.StatusCode) || attempt == MaximumAttempts)
                return response;

            response.Dispose();

            await Task.Delay(
                    TimeSpan.FromMilliseconds(500 * attempt),
                    TimeProvider.System,
                    cancellationToken
                )
                .ConfigureAwait(false);
        }

        throw new UnreachableException();
    }

    private static bool IsTransient(HttpStatusCode statusCode)
    {
        return statusCode is HttpStatusCode.RequestTimeout or HttpStatusCode.TooManyRequests
            || Convert.ToInt32(statusCode, System.Globalization.CultureInfo.InvariantCulture)
                >= 500;
    }
}
