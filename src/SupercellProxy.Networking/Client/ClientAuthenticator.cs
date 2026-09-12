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

    /// <summary>
    /// Provides the Load Catalog Async value or operation.
    /// </summary>
    internal async Task<DataTableResolver> LoadCatalogAsync(CancellationToken cancellationToken)
    {
        try
        {
            LoginOkMessage unexpectedLogin = await LoginCoreAsync(string.Empty, includeSession: false, session: null, ClientSession.DefaultAppStore, cancellationToken)
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
        ClientSession? session = await ClientSessionStore.LoadAsync(client.Configuration.SessionPath, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        AppStore appStore = session?.AppStore ?? ClientSession.DefaultAppStore;

        try
        {
            // 1.67.170 => be514e02b198d18287af1405089a0e72b849ac69
            // 1.67.175 => fdb648cea5e3494c3cafc32eca103331d85c5bfd
            // 1.69.89  => 0c95746ec8ced89978f4b9fded2fdbc95b3daf18
            // This bootstrap fingerprint is deliberately stale. Its only purpose is to provoke
            // the single expected OutdatedContent LoginFailed; never hardcode the current
            // fingerprint here because it must be detected dynamically from that response.
            return new ClientLoginResult(
                await LoginCoreAsync(
                        fingerprintSha1: client.Configuration.BootstrapFingerprintSha ?? string.Empty,
                        includeSession: false,
                        session,
                        appStore,
                        cancellationToken
                    )
                    .ConfigureAwait(continueOnCapturedContext: false)
            );
        }
        catch (LoginException loginException)
            when (loginException.LoginFailedMessage is { ErrorCode: LoginFailureType.OutdatedContent })
        {
            return await RecoverOutdatedContentAsync(loginException, session, appStore, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    private static LoginMessage CreateLoginMessage(string fingerprintSha1, bool includeSession, ClientSession? session, AppStore appStore)
    {
        return new LoginMessage
        {
            AccountIdentifier = includeSession ? session?.ParsedAccountIdentifier ?? LongIdentifier.Empty : LongIdentifier.Empty,
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
            SessionToken =
                includeSession && session?.CompressedData is { } compressed
                    ? LoginSessionToken.Decode(compressed)
                    : null,
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

        if (session is not null && login.AccountIdentifier != session.ParsedAccountIdentifier)
            throw new UnauthorizedAccessException(message: "Authentication returned a different account from the requested session; the session file was not changed.");

        if (session is not null && !string.Equals(login.PassToken, session.PassToken, StringComparison.Ordinal))
            throw new UnauthorizedAccessException(message: "Authentication returned a different pass token from the requested session; the session file was not changed.");
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

            await ClientSessionStore.SaveAsync(
                    loginOkMessage.AccountIdentifier,
                    session?.PassToken ?? loginOkMessage.PassToken,
                    appStore,
                    session?.CompressedData is { } compressed
                        ? compressed.AsMemory()
                        : (Memory<byte>?)null,
                    client.Configuration.SessionPath,
                    session?.SessionRefreshToken,
                    cancellationToken
                )
                .ConfigureAwait(continueOnCapturedContext: false);
            await stream
                .WriteMessageAsync(new RequestOwnHomeMessage(), cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            return loginOkMessage;
        }
        catch
        {
            await client.DisconnectAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            throw;
        }
    }

    private async Task<ClientLoginResult> RecoverOutdatedContentAsync(LoginException loginException, ClientSession? session, AppStore appStore, CancellationToken cancellationToken)
    {
        LoginFailedMessage loginFailedMessage =
            loginException.LoginFailedMessage
            ?? throw new InvalidOperationException(message: "The outdated-content login failure has no decoded message.", loginException);

        GameAssetFingerprint fingerprint = loginFailedMessage.GameAssetFingerprint;

        if (string.IsNullOrWhiteSpace(fingerprint.Sha))
            throw new InvalidOperationException($"Failed to parse fingerprint from login failed message:\n{loginFailedMessage.GameAssetFingerprintData}", loginException);

        GameAsset[] resources = await assets.GetAssetsAsync(fingerprint, loginFailedMessage.AssetsUrlsFiltered, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        ClientSession? authenticatedSession = session is null
            ? null
            : await sessionTokens
                .RefreshIfNeededAsync(session, force: false, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

        LoginOkMessage loginOk;

        try
        {
            loginOk = await LoginCoreAsync(fingerprint.Sha, includeSession: true, authenticatedSession, appStore, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (LoginException exception)
            when (exception.LoginFailedMessage is { ErrorCode: LoginFailureType.InvalidToken } && authenticatedSession?.CompressedData is not null)
        {
            authenticatedSession = await sessionTokens
                .RefreshIfNeededAsync(authenticatedSession, force: true, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            loginOk = await LoginCoreAsync(fingerprint.Sha, includeSession: true, authenticatedSession, appStore, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        return new ClientLoginResult(loginOk, fingerprint, resources);
    }

}
