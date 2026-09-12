using System.Diagnostics;
using System.Net;

namespace SupercellProxy.Keys;

internal static class WebClientExtensions
{
    private const int MaximumAttempts = 3;

    public static async Task<HttpResponseMessage> SendWithRetryAsync(this HttpClient client, Func<HttpRequestMessage> requestFactory, CancellationToken cancellationToken)
    {
        for (int attempt = 1; attempt <= MaximumAttempts; attempt++)
        {
            using HttpRequestMessage request = requestFactory();

            HttpResponseMessage response = await client
                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (!IsTransient(response.StatusCode) || attempt == MaximumAttempts)
                return response;

            response.Dispose();

            await Task.Delay(TimeSpan.FromMilliseconds(500 * attempt), TimeProvider.System, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        throw new UnreachableException();
    }

    private static bool IsTransient(HttpStatusCode statusCode)
    {
        return statusCode is HttpStatusCode.RequestTimeout or HttpStatusCode.TooManyRequests
            || (int)statusCode >= 500;
    }
}
