using System.Globalization;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using SupercellProxy.Playground.Commands;
using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Home;
using SupercellProxy.Playground.Home.Simulation;
using SupercellProxy.Playground.Json;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Configuration;
using SupercellProxy.Playground.Network.Connections.Client.Exceptions;
using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Messages.Serverbound;
using SupercellProxy.Playground.Network.Protocol;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Connections.Client;

public partial class ScClient
{
    private async Task<ScClientLoginResult> LoginAsync(
        CancellationToken cancellationToken = default
    )
    {
        var session = await ScClientSession
            .LoadAsync(configuration.SessionPath, cancellationToken)
            .ConfigureAwait(false);
        var appStore = session?.AppStore ?? ScClientSession.DefaultAppStore;

        try
        {
            // 1.67.170 => be514e02b198d18287af1405089a0e72b849ac69
            // 1.67.175 => fdb648cea5e3494c3cafc32eca103331d85c5bfd
            // 1.69.89  => 0c95746ec8ced89978f4b9fded2fdbc95b3daf18
            // This bootstrap fingerprint is deliberately stale. Its only purpose is to provoke
            // the single expected OutdatedContent LoginFailed; never hardcode the current
            // fingerprint here because it must be detected dynamically from that response.
            return new ScClientLoginResult(
                await LoginCoreAsync(
                        fingerprintSha1: configuration.BootstrapFingerprintSha ?? string.Empty,
                        includeSession: false,
                        session,
                        appStore,
                        cancellationToken
                    )
                    .ConfigureAwait(false)
            );
        }
        catch (LoginException loginException)
            when (loginException.LoginFailedMessage.ErrorCode is LoginFailureType.OutdatedContent)
        {
            var fingerprint = loginException.LoginFailedMessage.GameAssetFingerprint;
            var resources = await GetAssetsAsync(
                    fingerprint,
                    loginException.LoginFailedMessage.AssetsUrlsFiltered,
                    cancellationToken
                )
                .ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(fingerprint.Sha))
                throw new InvalidOperationException(
                    $"Failed to parse fingerprint from login failed message:\n{loginException.LoginFailedMessage.GameAssetFingerprintData}",
                    loginException
                );

            return new ScClientLoginResult(
                await LoginCoreAsync(
                        fingerprint.Sha,
                        includeSession: true,
                        session,
                        appStore,
                        cancellationToken
                    )
                    .ConfigureAwait(false),
                fingerprint,
                resources
            );
        }
    }

    private async Task<LoginOkMessage> LoginCoreAsync(
        string fingerprintSha1,
        bool includeSession,
        ScClientSession? session,
        AppStore appStore,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var stream = await GetStreamAsync(cancellationToken).ConfigureAwait(false);

            await stream
                .WriteMessageAsync(
                    CreateClientHelloMessage(fingerprintSha1, appStore),
                    cancellationToken
                )
                .ConfigureAwait(false);

            var serverHello = RequireMessage<ServerHelloMessage>(
                await stream.ReadMessageAsync(cancellationToken).ConfigureAwait(false)
            );

            await stream
                .SetupEncryptionAsync(Side.Server, serverHello.SessionKey, cancellationToken)
                .ConfigureAwait(false);

            await stream
                .WriteMessageAsync(
                    CreateLoginMessage(fingerprintSha1, includeSession, session, appStore),
                    cancellationToken
                )
                .ConfigureAwait(false);

            var loginOkMessage = RequireMessage<LoginOkMessage>(
                await stream.ReadMessageAsync(cancellationToken).ConfigureAwait(false)
            );

            if (!includeSession)
            {
                throw new InvalidOperationException(
                    "The fingerprint bootstrap login unexpectedly succeeded; expected one OutdatedContent LoginFailed before the authenticated retry."
                );
            }

            await ScClientSession
                .SaveAsync(
                    loginOkMessage.AccountId,
                    loginOkMessage.PassToken,
                    appStore,
                    session?.CompressedData,
                    configuration.SessionPath,
                    cancellationToken
                )
                .ConfigureAwait(false);
            await stream
                .WriteMessageAsync(new ClientLoadingFunnelMessage(), cancellationToken)
                .ConfigureAwait(false);
            return loginOkMessage;
        }
        catch
        {
            await DisconnectAsync(cancellationToken).ConfigureAwait(false);
            throw;
        }
    }

    private static TMessage RequireMessage<TMessage>(IMessage message)
        where TMessage : class, IMessage
    {
        LoginException.ThrowIfFailed(message);
        return message as TMessage
            ?? throw new InvalidOperationException(
                $"Expected {typeof(TMessage).Name}, but received {message}."
            );
    }

    private ClientHelloMessage CreateClientHelloMessage(string fingerprintSha1, AppStore appStore)
    {
        return new ClientHelloMessage
        {
            ProtocolVersion = configuration.Protocol.ProtocolVersion,
            KeyVersion = configuration.Protocol.KeyVersion,
            MajorVersion = configuration.Protocol.MajorVersion,
            MinorVersion = configuration.Protocol.MinorVersion,
            PatchVersion = configuration.Protocol.PatchVersion,
            FingerprintSha1 = fingerprintSha1,
            DeviceType = 2,
            AppStore = appStore,
            Unknown1 = -1,
        };
    }

    private static LoginMessage CreateLoginMessage(
        string fingerprintSha1,
        bool includeSession,
        ScClientSession? session,
        AppStore appStore
    )
    {
        return new LoginMessage
        {
            AccountId = includeSession ? session?.ParsedAccountId ?? LongId.Empty : LongId.Empty,
            PassToken = includeSession ? session?.PassToken : null,
            ResourceSha = fingerprintSha1,
            LoginVersion = LoginMessage.CurrentLoginVersion,
            UdId = "",
            OpenUdId = "",
            MacAddress = "",
            DeviceModel = "",
            AdvertisingId = "",
            IsAndroid = true,
            OsVersion = "",
            UnknownString0 = "",
            AndroidId = "",
            PreferredLanguage = "",
            UnknownString1 = "",
            AdvertisingTrackingEnabled = true,
            IdentifierForVendor = "",
            AppStore = appStore,
            CompressedData = includeSession ? session?.CompressedData : null,
            StorefrontCountryCode = "",
            StorefrontIdentifier = "",
        };
    }
}
