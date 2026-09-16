using SupercellProxy.Networking.Events;
using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Sessions;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Proxy;

internal sealed class ProxyHandshake(Func<bool, CancellationToken, Task<SessionTokenData>>? sessionTokenProvider)
{
    internal async Task OnMessageReceivedEventAsync(MessageReceivedEvent @event, CancellationToken cancellationToken)
    {
        if (@event.Message is LoginMessage login && @event.Direction is MessageDirection.Serverbound && sessionTokenProvider is not null)
        {
            SessionTokenData token = await sessionTokenProvider(arg1: false, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            SessionTokenData.ValidateAuthentication(token, TimeProvider.System);

            login.AccountIdentifier = LongIdentifier.Empty;
            login.PassToken = null;
            login.SessionToken = token;
        }
        else if (@event.Message is ServerHelloMessage hello)
        {
            await @event.Source.SetupEncryptionAsync(RemotePeerRole.Server, hello.SessionKey, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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

            throw new Client.LoginException(failure);
        }
    }
}
