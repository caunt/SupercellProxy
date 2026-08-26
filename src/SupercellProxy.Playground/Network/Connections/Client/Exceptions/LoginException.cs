using System.Globalization;
using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Protocol;

namespace SupercellProxy.Playground.Network.Connections.Client.Exceptions;

/// <summary>
/// Represents <c>LoginException</c>.
/// </summary>
/// <remarks>
/// Initializes a new <see cref="LoginException"/> instance.
/// </remarks>
public class LoginException(LoginFailedMessage loginFailedMessage)
    : Exception(GetMessage(loginFailedMessage))
{
    /// <summary>
    /// Gets the <c>LoginFailedMessage</c> value.
    /// </summary>
    public LoginFailedMessage LoginFailedMessage { get; } = loginFailedMessage;

    /// <summary>
    /// Executes the <c>ThrowIfFailed</c> operation.
    /// </summary>
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
            LoginFailureType.InvalidCredentials => "account ID or pass token is invalid",
            LoginFailureType.InvalidToken => "pass token is invalid for this account",
            _ => loginFailedMessage.ErrorCode.ToString(),
        };
        var error = string.Create(
            CultureInfo.InvariantCulture,
            $"{System.Runtime.CompilerServices.Unsafe.BitCast<LoginFailureType, int>(loginFailedMessage.ErrorCode)} ({errorDescription})"
        );

        return $"{error}{(string.IsNullOrWhiteSpace(loginFailedMessage.Reason) ? string.Empty : $" (reason: {loginFailedMessage.Reason})")}";
    }
}
