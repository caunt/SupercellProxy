using SupercellProxy.Networking.Assets;
using SupercellProxy.Networking.Assets.Tables;
using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.Homes;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Sessions;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Client;

internal sealed class ClientAuthenticator(ProtocolClient client, GameAssetCache assets)
{

    internal static LoginMessage CreateLoginMessage(string fingerprintSha1, SessionTokenData? sessionToken, AppStore appStore)
    {
        return new LoginMessage
        {
            AccountIdentifier = LongIdentifier.Empty,
            PassToken = null,
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
            SessionToken = sessionToken,
            StorefrontCountryCode = "",
            StorefrontIdentifier = "",
        };
    }

    internal async Task<DataTableResolver> LoadCatalogAsync(CancellationToken cancellationToken)
    {
        try
        {
            LoginOkMessage unexpectedLogin = await LoginCoreAsync(string.Empty, sessionToken: null, AppStore.GooglePlay, requestOwnHome: false, cancellationToken)
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
        Func<bool, CancellationToken, Task<SessionTokenData>> provider = client.Configuration.SessionTokenProvider
            ?? throw new InvalidOperationException(message: "A session-token provider is required.");

        LoginFailedMessage content;

        try
        {
            LoginOkMessage unexpected = await LoginCoreAsync(
                client.Configuration.BootstrapFingerprintSha ?? string.Empty,
                sessionToken: null,
                AppStore.GooglePlay,
                requestOwnHome: false,
                cancellationToken
            ).ConfigureAwait(continueOnCapturedContext: false);

            throw new InvalidDataException($"Fingerprint discovery returned {unexpected.GetType().Name}.");
        }
        catch (LoginException exception) when (exception.LoginFailedMessage is { ErrorCode: LoginFailureType.OutdatedContent })
        {
            content = exception.LoginFailedMessage;
        }

        GameAssetFingerprint fingerprint = content.GameAssetFingerprint;
        GameAsset[] resources = await assets.GetAssetsAsync(fingerprint, content.AssetsUrlsFiltered, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        SessionTokenData token = await provider(arg1: false, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        LoginOkMessage login;

        try
        {
            login = await LoginCoreAsync(fingerprint.Sha, token, AppStore.GooglePlay, requestOwnHome: true, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (LoginException exception) when (exception.LoginFailedMessage is { ErrorCode: LoginFailureType.InvalidToken })
        {
            token = await provider(arg1: true, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            login = await LoginCoreAsync(fingerprint.Sha, token, AppStore.GooglePlay, requestOwnHome: true, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }

        return new ClientLoginResult(login, fingerprint, resources);
    }

    private static TMessage RequireMessage<TMessage>(IMessage message)
        where TMessage : class, IMessage
    {
        LoginException.ThrowIfFailed(message);

        return message as TMessage
            ?? throw new InvalidOperationException($"Expected {typeof(TMessage).Name}, but received {message}.");
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
        SessionTokenData? sessionToken,
        AppStore appStore,
        bool requestOwnHome,
        CancellationToken cancellationToken = default
    )
    {
        if (requestOwnHome) SessionTokenData.ValidateAuthentication(sessionToken, TimeProvider.System);

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
                .WriteMessageAsync(CreateLoginMessage(fingerprintSha1, sessionToken, appStore), cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            LoginOkMessage loginOkMessage = RequireMessage<LoginOkMessage>(await stream.ReadMessageAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));

            if (sessionToken is null || sessionToken.IsEmpty || loginOkMessage.AccountIdentifier == LongIdentifier.Empty)
                throw new InvalidDataException(message: "Authentication did not establish an account using a session token.");

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

}
