using Microsoft.Extensions.Logging;

using SupercellProxy.Networking.Client;
using SupercellProxy.Networking.Events;
using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.Homes;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Sessions;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Proxy;

internal sealed partial class ProxyHandshake(ClientSessionLedger sessionLedger, string? sessionAccountIdentifier, ILogger? logger, string remoteEndPoint)
{
    private LoginMessage? _loginMessage;
    private LoginOkMessage? _loginOkMessage;

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

                        if (logger is not null)
                            LogSessionReplacement(logger, remoteEndPoint, session.FarmName, session.AccountIdentifier);

                        loginMessage.AccountIdentifier = session.AccountIdentifier;
                        loginMessage.PassToken = session.PassToken;
                        loginMessage.AppStore = session.AppStore;
                        loginMessage.SessionToken = session.SessionToken;
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
            case LoginFailedMessage loginFailedMessage
                when @event.Direction is MessageDirection.Clientbound && _loginMessage is not null:
                {
                    _loginMessage = null;
                    _loginOkMessage = null;

                    throw new LoginException(loginFailedMessage);
                }
            case LoginOkMessage loginOkMessage
                when @event.Direction is MessageDirection.Clientbound && _loginMessage is not null:
                {
                    _loginOkMessage = loginOkMessage;

                    break;
                }
            case OwnHomeDataMessage ownHomeDataMessage
                when @event.Direction is MessageDirection.Clientbound:
                {
                    if (_loginMessage is not { } loginMessage || _loginOkMessage is not { } loginOkMessage)
                        break;

                    _loginMessage = null;
                    _loginOkMessage = null;
                    ClientSession session = ClientSession.FromLoginOutcome(loginMessage, loginOkMessage, ownHomeDataMessage);

                    bool sessionAdded = await sessionLedger.TryAddAsync(session, cancellationToken)
                        .ConfigureAwait(continueOnCapturedContext: false);

                    if (!sessionAdded)
                    {
                        await sessionLedger.UpdateSessionAsync(session, cancellationToken)
                            .ConfigureAwait(continueOnCapturedContext: false);
                    }

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

    [LoggerMessage(Level = LogLevel.Information, Message = "Replacing the session for incoming client {RemoteEndPoint} with farm {FarmName} ({AccountIdentifier}).")]
    private static partial void LogSessionReplacement(ILogger logger, string remoteEndPoint, string farmName, LongIdentifier accountIdentifier);

}
