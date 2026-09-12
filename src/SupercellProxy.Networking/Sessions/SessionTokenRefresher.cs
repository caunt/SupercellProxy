using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace SupercellProxy.Networking.Sessions;

/// <summary>
/// Defines the Session Tokens contract.
/// </summary>
public sealed class SessionTokenRefresher(HttpClient webClient, TimeProvider timeProvider)
{
    private static readonly Uri[] Endpoints =
    [
        new(uriString: "https://security-eu.id.supercell.com/api/security/v2/sessionToken"),
        new(uriString: "https://security-us.id.supercell.com/api/security/v2/sessionToken"),
        new(uriString: "https://security-apac.id.supercell.com/api/security/v2/sessionToken"),
    ];

    private readonly HttpClient _webClient = webClient;
    private readonly TimeProvider _timeProvider = timeProvider;

    /// <summary>
    /// Provides the Refresh If Needed Async value or operation.
    /// </summary>
    public async Task<ClientSession> RefreshIfNeededAsync(ClientSession session, bool force, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(session);

        LoginSessionToken? current = session.SessionToken;

        if (current is null && !force)
            return session;

        if (!force && current is not null && !current.ShouldRefresh(_timeProvider))
            return session;

        if (string.IsNullOrWhiteSpace(session.SessionRefreshToken))
            throw new InvalidDataException(message: "The client session needs refresh but has no Supercell ID refresh token.");

        LoginSessionToken refreshed = await RefreshAsync(session.SessionRefreshToken, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return session with
        {
            SessionToken = refreshed,
        };
    }

    private static async Task<LoginSessionToken> ReadTokenAsync(HttpContent content, CancellationToken cancellationToken)
    {
        SessionRefreshResponse response = await content.ReadFromJsonAsync<SessionRefreshResponse>(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false)
            ?? throw new InvalidDataException(message: "Session-token endpoint returned a null response.");

        return !response.Success || string.IsNullOrWhiteSpace(response.Token)
            ? throw new InvalidDataException(message: "Session-token endpoint returned an invalid success response.")
            : LoginSessionToken.Decode(response.Token);
    }

    private async Task<LoginSessionToken> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        Exception? lastTransientFailure = null;

        foreach (Uri endpoint in Endpoints)
        {
            try
            {
                LoginSessionToken? token = await TryRefreshAsync(endpoint, refreshToken, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);

                if (token is not null)
                    return token;
            }
            catch (Exception exception) when (exception is HttpRequestException or IOException)
            {
                lastTransientFailure = exception;
            }
        }

        throw new HttpRequestException(message: "Every session-token endpoint failed before returning a response.", lastTransientFailure);
    }

    private async Task<LoginSessionToken?> TryRefreshAsync(Uri endpoint, string refreshToken, CancellationToken cancellationToken)
    {
        using HttpRequestMessage request = new(HttpMethod.Post, endpoint)
        {
            Content = JsonContent.Create(new SessionRefreshRequest()),
        };

        request.Headers.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", refreshToken);
        string deviceIdentifier = Guid.NewGuid().ToString(format: "D").ToUpperInvariant();
        string body = await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        request.Headers.Add(name: "User-Agent", SupercellRequestSigner.UserAgent);
        request.Headers.Add(name: "X-Supercell-Device-Id", deviceIdentifier);
        request.Headers.Add(
            name: "X-Supercell-Request-Forgery-Protection",
            SupercellRequestSigner.Sign(endpoint.AbsolutePath, body, SupercellRequestSigner.UserAgent, deviceIdentifier, request.Headers.Authorization.ToString())
        );

        using HttpResponseMessage response = await _webClient
            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        int statusCode = Unsafe.BitCast<HttpStatusCode, int>(response.StatusCode);

        return statusCode >= 500
            ? null
            : !response.IsSuccessStatusCode
            ? throw new InvalidOperationException(string.Create(CultureInfo.InvariantCulture, $"Session-token refresh was rejected with HTTP {statusCode}."))
            : await ReadTokenAsync(response.Content, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
}
