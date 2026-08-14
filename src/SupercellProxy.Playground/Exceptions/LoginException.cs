using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Clientbound;

namespace SupercellProxy.Playground.Exceptions;

public class LoginException(LoginFailedMessage loginFailedMessage) : Exception(GetMessage(loginFailedMessage))
{
    public LoginFailedMessage LoginFailedMessage => loginFailedMessage;

    public static void ThrowIfFailed(IMessage message)
    {
        if (message is not LoginFailedMessage loginFailedMessage)
            return;

        throw new LoginException(loginFailedMessage);
    }

    private static string GetMessage(LoginFailedMessage loginFailedMessage)
    {
        var error = loginFailedMessage.ErrorCode is LoginFailedMessage.Type.InvalidCredentials
            ? $"{loginFailedMessage.ErrorCode} (account ID or pass token is invalid)"
            : loginFailedMessage.ErrorCode.ToString();

        return $"{error}{(string.IsNullOrWhiteSpace(loginFailedMessage.Reason) ? string.Empty : $" (reason: {loginFailedMessage.Reason})")}";
    }
}
