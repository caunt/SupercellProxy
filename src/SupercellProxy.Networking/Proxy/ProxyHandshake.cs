using SupercellProxy.Networking.Events;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Sessions;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Proxy;

internal sealed class ProxyHandshake(string? sessionPath)
{
    private LoginMessage? _loginMessage;

    internal async Task OnMessageReceivedEventAsync(MessageReceivedEvent @event, CancellationToken cancellationToken)
    {
        switch (@event.Message)
        {
            case LoginMessage loginMessage when @event.Direction is MessageDirection.Serverbound:
                {
                    if (sessionPath is not null)
                    {
                        ClientSession session =
                            await ClientSessionStore.LoadAsync(sessionPath, cancellationToken)
                                .ConfigureAwait(continueOnCapturedContext: false)
                            ?? throw new InvalidDataException(message: "The selected proxy session does not exist.");

                        loginMessage.AccountIdentifier = session.ParsedAccountIdentifier;
                        loginMessage.PassToken = session.PassToken;
                        loginMessage.AppStore = session.AppStore;
                        loginMessage.SessionToken = session.CompressedData is { } compressed
                            ? LoginSessionToken.Decode(compressed)
                            : null;
                    }

                    _loginMessage = loginMessage;

                    break;
                }
            case ServerHelloMessage serverHelloMessage:
                {
                    await @event
                        .Source.SetupEncryptionAsync(RemotePeerRole.Server, serverHelloMessage.SessionKey, cancellationToken)
                        .ConfigureAwait(continueOnCapturedContext: false);

                    break;
                }
            default:
                break;
        }
    }

    internal async Task OnMessageSentEventAsync(MessageSentEvent @event, CancellationToken cancellationToken = default)
    {
        switch (@event.Message)
        {
            case LoginOkMessage loginOkMessage
                when @event.Direction is MessageDirection.Clientbound && _loginMessage is { } loginMessage:
                {
                    await ClientSessionStore.SaveAsync(
                            loginOkMessage.AccountIdentifier,
                            loginOkMessage.PassToken,
                            loginMessage.AppStore,
                            loginMessage.SessionToken?.Encode().AsMemory(),
                            sessionPath,
                            cancellationToken: cancellationToken
                        )
                        .ConfigureAwait(continueOnCapturedContext: false);

                    break;
                }
            case ServerHelloMessage serverHelloMessage:
                {
                    await @event
                        .Destination.SetupEncryptionAsync(RemotePeerRole.Client, serverHelloMessage.SessionKey, cancellationToken)
                        .ConfigureAwait(continueOnCapturedContext: false);

                    break;
                }
            default:
                break;
        }

    }

}
