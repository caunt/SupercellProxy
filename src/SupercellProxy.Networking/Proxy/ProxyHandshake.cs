using SupercellProxy.Networking.Events;
using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Sessions;
using SupercellProxy.Networking.Sessions.Anonymous;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Proxy;

internal sealed class ProxyHandshake(Func<bool, CancellationToken, Task<SessionTokenData>>? sessionTokenProvider, AnonymousAccountLease? anonymousAccount = null)
{
    internal async Task OnMessageReceivedEventAsync(MessageReceivedEvent @event, CancellationToken cancellationToken)
    {
        if (@event.Message is ClientHelloMessage)
            @event.Destination.GameVersion = @event.Source.GameVersion;

        bool fromClient = @event.Direction is MessageDirection.Serverbound;

        if (@event.Message is LoginMessage anonymousLogin && fromClient && anonymousAccount is not null)
        {
            AnonymousAccountCredentials credentials = anonymousAccount.Credentials
                ?? throw new InvalidOperationException(message: "A proxy anonymous account must have saved credentials.");

            anonymousLogin.AccountId = new LongId(int.CreateTruncating(credentials.AccountId >> 32), int.CreateTruncating(credentials.AccountId));
            anonymousLogin.PassToken = credentials.PassToken;
            anonymousLogin.SessionToken = null;
        }
        else if (@event.Message is LoginMessage login && @event.Direction is MessageDirection.Serverbound && sessionTokenProvider is not null)
        {
            SessionTokenData token = await sessionTokenProvider(arg1: false, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            SessionTokenData.ValidateAuthentication(token, TimeProvider.System);

            login.AccountId = LongId.Empty;
            login.PassToken = null;
            login.SessionToken = token;
        }
        else if (@event.Message is ServerHelloMessage hello)
        {
            await @event.Source.SetupEncryptionAsync(RemotePeerRole.Server, hello.SessionKey, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
        else if (@event.Message is LoginOkMessage accepted && @event.Direction is MessageDirection.Clientbound && anonymousAccount is not null)
        {
            if (accepted.AccountId.AsInt64 != anonymousAccount.Credentials?.AccountId)
                throw new InvalidDataException(message: "The proxy authenticated a different anonymous account.");

            using CancellationTokenSource saving = new(TimeSpan.FromSeconds(seconds: 10));

            await anonymousAccount.SaveAsync(new(accepted.AccountId.AsInt64, accepted.PassToken), saving.Token).ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    internal async Task OnMessageSentEventAsync(MessageSentEvent @event, CancellationToken cancellationToken = default)
    {
        if (@event.Message is ServerHelloMessage hello)
        {
            await @event.Destination.SetupEncryptionAsync(RemotePeerRole.Client, hello.SessionKey, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
        else if (@event.Message is LoginFailedMessage failure && @event.Direction is MessageDirection.Clientbound)
        {
            if (failure.ErrorCode is LoginFailureType.InvalidToken && sessionTokenProvider is not null)
            {
                SessionTokenData refreshed = await sessionTokenProvider(arg1: true, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

                SessionTokenData.ValidateAuthentication(refreshed, TimeProvider.System);
            }
        }
    }
}
