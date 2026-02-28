using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Clientbound;

namespace SupercellProxy.Playground.Exceptions;

public class LoginException(LoginFailedMessage loginFailedMessage) : Exception($"{loginFailedMessage.ErrorCode}{(string.IsNullOrWhiteSpace(loginFailedMessage.Reason) ? string.Empty : $" (reason: {loginFailedMessage.Reason})")}")
{
    public LoginFailedMessage LoginFailedMessage => loginFailedMessage;

    public static void ThrowIfFailed(IMessage message)
    {
        if (message is not LoginFailedMessage loginFailedMessage)
            return;

        throw new LoginException(loginFailedMessage);
    }
}
