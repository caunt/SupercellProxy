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
        var errorDescription = loginFailedMessage.ErrorCode switch
        {
            LoginFailedMessage.Type.InvalidCredentials => "account ID or pass token is invalid",
            LoginFailedMessage.Type.InvalidToken => "pass token is invalid for this account",
            _ => loginFailedMessage.ErrorCode.ToString()
        };
        var error = $"{(int)loginFailedMessage.ErrorCode} ({errorDescription})";

        return $"{error}{(string.IsNullOrWhiteSpace(loginFailedMessage.Reason) ? string.Empty : $" (reason: {loginFailedMessage.Reason})")}";
    }
}
