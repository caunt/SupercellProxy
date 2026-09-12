using SupercellProxy.Networking.Assets;
using SupercellProxy.Networking.Assets.Tables;
using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.Homes;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Sessions;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Client;

internal sealed class ClientAuthenticator(ProtocolClient client, SessionTokenRefresher sessionTokens, GameAssetCache assets)
{
    internal async Task<DataTableResolver> LoadCatalogAsync(CancellationToken cancellationToken)
    {
        try
        {
            LoginOkMessage unexpectedLogin = await LoginCoreAsync(string.Empty, includeSession: false, session: null, ClientSession.DefaultAppStore, requestOwnHome: false, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            throw new InvalidDataException($"Catalog bootstrap returned {unexpectedLogin.GetType().Name} without asset metadata.");
        }
        catch (LoginException exception)
            when (exception.LoginFailedMessage is { ErrorCode: LoginFailureType.OutdatedContent } content)
        {
            GameAsset[] resources = await assets.GetAssetsAsync(content.GameAssetFingerprint, content.AssetsUrlsFiltered, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            return new DataTableResolver(resources);
        }
    }

    internal async Task<ClientLoginResult> LoginAsync(CancellationToken cancellationToken = default)
    {
        LongIdentifier accountIdentifier = ParseSelectedAccountIdentifier();
        ClientSessionLedger ledger = new(client.Configuration.SessionLedgerPath);

        ClientSession session = await ledger.GetSessionAsync(accountIdentifier, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false)
            ?? throw new InvalidDataException($"Client session {client.Configuration.SessionAccountIdentifier} is not present in {ledger.FilePath}.");

        try
        {
            AuthenticatedClientLogin authenticated = await LoginWithSessionAsync(session, loadAssets: true, requestOwnHome: true, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            await ledger.UpdateSessionAsync(authenticated.Session, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            return authenticated.Result;
        }
        catch
        {
            await client.DisconnectAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            throw;
        }
    }

    internal async Task<ClientSession> ValidateSessionAsync(ClientSession session, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(session);
        const string description = "the supplied client session";
        ClientSessionValidation.Validate(session, description);

        AuthenticatedClientLogin authenticated = await LoginWithSessionAsync(session, loadAssets: false, requestOwnHome: false, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return authenticated.Session;
    }

    private static bool CanForceRefresh(LoginException exception, ClientSession session)
    {
        return exception.LoginFailedMessage is { ErrorCode: LoginFailureType.InvalidToken }
            && session.SessionToken is not null;
    }

    private static LoginMessage CreateLoginMessage(string fingerprintSha1, bool includeSession, ClientSession? session, AppStore appStore)
    {
        return new LoginMessage
        {
            AccountIdentifier = includeSession ? session?.AccountIdentifier ?? LongIdentifier.Empty : LongIdentifier.Empty,
            PassToken = includeSession ? session?.PassToken : null,
            ResourceSha = fingerprintSha1,
            LoginVersion = LoginMessage.CurrentLoginVersion,
            UniqueDeviceIdentifier = "",
            OpenUniqueDeviceIdentifier = "",
            MacAddress = "",
            DeviceModel = "",
            AdvertisingIdentifier = "",
            IsAndroid = true,
            OsVersion = "",
            UnknownString0 = "",
            AndroidIdentifier = "",
            PreferredLanguage = "",
            UnknownString1 = "",
            AdvertisingTrackingEnabled = true,
            IdentifierForVendor = "",
            AppStore = appStore,
            SessionToken = includeSession ? session?.SessionToken : null,
            StorefrontCountryCode = "",
            StorefrontIdentifier = "",
        };
    }

    private static TMessage RequireMessage<TMessage>(IMessage message)
        where TMessage : class, IMessage
    {
        LoginException.ThrowIfFailed(message);

        return message as TMessage
            ?? throw new InvalidOperationException($"Expected {typeof(TMessage).Name}, but received {message}.");
    }

    private static void ValidateAccountIdentity(ClientSession? session, LoginOkMessage login, bool includeSession)
    {
        if (!includeSession)
        {
            throw new InvalidOperationException(
                message: "The fingerprint bootstrap login unexpectedly succeeded; expected one OutdatedContent LoginFailed before the authenticated retry."
            );
        }

        if (session is not null && login.AccountIdentifier != session.AccountIdentifier)
            throw new UnauthorizedAccessException(message: "Authentication returned a different account from the requested ledger session.");

        if (session is not null && !string.Equals(login.PassToken, session.PassToken, StringComparison.Ordinal))
            throw new UnauthorizedAccessException(message: "Authentication returned a different pass token from the requested ledger session.");
    }

    private ClientHelloMessage CreateClientHelloMessage(string fingerprintSha1, AppStore appStore)
    {
        return new ClientHelloMessage
        {
            ProtocolVersion = client.Configuration.Protocol.ProtocolVersion,
            KeyVersion = client.Configuration.Protocol.KeyVersion,
            MajorVersion = client.Configuration.Protocol.MajorVersion,
            MinorVersion = client.Configuration.Protocol.MinorVersion,
            PatchVersion = client.Configuration.Protocol.PatchVersion,
            FingerprintSha1 = fingerprintSha1,
            DeviceType = 2,
            AppStore = appStore,
            Unknown1 = -1,
        };
    }

    private async Task<LoginOkMessage> LoginCoreAsync(
        string fingerprintSha1,
        bool includeSession,
        ClientSession? session,
        AppStore appStore,
        bool requestOwnHome,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            MessageStream stream = await client.GetStreamAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            await stream
                .WriteMessageAsync(CreateClientHelloMessage(fingerprintSha1, appStore), cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            ServerHelloMessage serverHello = RequireMessage<ServerHelloMessage>(await stream.ReadMessageAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));

            await stream
                .SetupEncryptionAsync(RemotePeerRole.Server, serverHello.SessionKey, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            await stream
                .WriteMessageAsync(CreateLoginMessage(fingerprintSha1, includeSession, session, appStore), cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            LoginOkMessage loginOkMessage = RequireMessage<LoginOkMessage>(await stream.ReadMessageAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));

            ValidateAccountIdentity(session, loginOkMessage, includeSession);

            if (requestOwnHome)
            {
                await stream
                    .WriteMessageAsync(new RequestOwnHomeMessage(), cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }

            return loginOkMessage;
        }
        catch
        {
            await client.DisconnectAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            throw;
        }
    }

    private async Task<AuthenticatedClientLogin> LoginWithSessionAsync(ClientSession session, bool loadAssets, bool requestOwnHome, CancellationToken cancellationToken)
    {
        try
        {
            // The bootstrap value is deliberately stale. It only obtains the current
            // fingerprint from the expected OutdatedContent response before authentication.
            LoginOkMessage unexpectedLogin = await LoginCoreAsync(
                    client.Configuration.BootstrapFingerprintSha ?? string.Empty,
                    includeSession: false,
                    session: null,
                    session.AppStore,
                    requestOwnHome: false,
                    cancellationToken
                )
                .ConfigureAwait(continueOnCapturedContext: false);

            throw new InvalidDataException($"Session validation returned unexpected {unexpectedLogin.GetType().Name} without current asset metadata.");
        }
        catch (LoginException loginException)
            when (loginException.LoginFailedMessage is { ErrorCode: LoginFailureType.OutdatedContent })
        {
            return await RecoverOutdatedContentAsync(loginException, session, loadAssets, requestOwnHome, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    private LongIdentifier ParseSelectedAccountIdentifier()
    {
        bool validAccountIdentifier = LongIdentifier.TryParse(client.Configuration.SessionAccountIdentifier, out LongIdentifier accountIdentifier)
            && accountIdentifier != LongIdentifier.Empty;

        return validAccountIdentifier
            ? accountIdentifier
            : throw new InvalidDataException(message: "A valid nonempty session account identifier must be configured.");
    }

    private async Task<AuthenticatedClientLogin> RecoverOutdatedContentAsync(LoginException loginException, ClientSession session, bool loadAssets, bool requestOwnHome, CancellationToken cancellationToken)
    {
        LoginFailedMessage loginFailedMessage =
            loginException.LoginFailedMessage
            ?? throw new InvalidOperationException(message: "The outdated-content login failure has no decoded message.", loginException);

        GameAssetFingerprint fingerprint = loginFailedMessage.GameAssetFingerprint;

        if (string.IsNullOrWhiteSpace(fingerprint.Sha))
            throw new InvalidOperationException($"Failed to parse fingerprint from login failed message:\n{loginFailedMessage.GameAssetFingerprintData}", loginException);

        GameAsset[] resources = loadAssets
            ? await assets.GetAssetsAsync(fingerprint, loginFailedMessage.AssetsUrlsFiltered, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false)
            : [];

        ClientSession authenticatedSession = await sessionTokens
            .RefreshIfNeededAsync(session, force: false, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        LoginOkMessage loginOk;

        try
        {
            loginOk = await LoginCoreAsync(fingerprint.Sha, includeSession: true, authenticatedSession, authenticatedSession.AppStore, requestOwnHome, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (LoginException exception)
            when (CanForceRefresh(exception, authenticatedSession))
        {
            authenticatedSession = await sessionTokens
                .RefreshIfNeededAsync(authenticatedSession, force: true, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            loginOk = await LoginCoreAsync(fingerprint.Sha, includeSession: true, authenticatedSession, authenticatedSession.AppStore, requestOwnHome, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        return new AuthenticatedClientLogin(new ClientLoginResult(loginOk, fingerprint, resources), authenticatedSession);
    }

    private sealed record AuthenticatedClientLogin(ClientLoginResult Result, ClientSession Session);
}
