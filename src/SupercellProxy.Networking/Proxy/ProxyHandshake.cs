using SupercellProxy.Networking.Events;
using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Sessions;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Proxy;

internal sealed class ProxyHandshake(ClientSessionLedger sessionLedger, string? sessionAccountIdentifier)
{
    private LoginMessage? _loginMessage;

    internal async Task OnMessageReceivedEventAsync(MessageReceivedEvent @event, CancellationToken cancellationToken)
    {
        switch (@event.Message)
        {
            case LoginMessage loginMessage when @event.Direction is MessageDirection.Serverbound:
                {
                    if (sessionAccountIdentifier is not null)
                    {
                        bool validAccountIdentifier = LongIdentifier.TryParse(sessionAccountIdentifier, out LongIdentifier parsed)
                            && parsed != LongIdentifier.Empty;

                        LongIdentifier accountIdentifier = validAccountIdentifier
                            ? parsed
                            : throw new InvalidDataException(message: "The selected proxy session account identifier is invalid.");

                        ClientSession session =
                            await sessionLedger.GetSessionAsync(accountIdentifier, cancellationToken)
                                .ConfigureAwait(continueOnCapturedContext: false)
                            ?? throw new InvalidDataException($"The selected proxy session does not exist in {sessionLedger.FilePath}.");

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
            case LoginOkMessage or LoginFailedMessage
                when @event.Direction is MessageDirection.Clientbound && _loginMessage is { } loginMessage:
                {
                    _loginMessage = null;

                    bool sessionAdded = await sessionLedger
                        .TryAddAsync(loginMessage, @event.Message, cancellationToken)
                        .ConfigureAwait(continueOnCapturedContext: false);

                    if (!sessionAdded)
                        break;

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
