using System.Globalization;
using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Network.Protocol;

namespace SupercellProxy.Playground.Network.Connections.Client.Exceptions;

/// <summary>
/// Represents <c language="csharp">LoginException</c>.
/// </summary>
/// <remarks>
/// Initializes a new <see cref="LoginException"/> instance.
/// </remarks>
internal sealed class LoginException : Exception
{
    public LoginException() { }

    public LoginException(string? message)
        : base(message) { }

    public LoginException(string? message, Exception? innerException)
        : base(message, innerException) { }

    public LoginException(LoginFailedMessage loginFailedMessage)
        : base(GetMessage(loginFailedMessage))
    {
        LoginFailedMessage = loginFailedMessage;
    }

    /// <summary>
    /// Gets the <c language="csharp">LoginFailedMessage</c> value.
    /// </summary>
    public LoginFailedMessage? LoginFailedMessage { get; }

    /// <summary>
    /// Executes the <c language="csharp">ThrowIfFailed</c> operation.
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
            LoginFailureType.OutdatedContent
            or LoginFailureType.OutdatedVersion
            or LoginFailureType.Unknown1
            or LoginFailureType.Maintenance
            or LoginFailureType.TemporarilyBanned
            or LoginFailureType.Redirection
            or LoginFailureType.Locked
            or LoginFailureType.AccountNotBound => loginFailedMessage.ErrorCode.ToString(),
            _ => loginFailedMessage.ErrorCode.ToString(),
        };
        var error = string.Create(
            CultureInfo.InvariantCulture,
            $"{System.Runtime.CompilerServices.Unsafe.BitCast<LoginFailureType, int>(loginFailedMessage.ErrorCode)} ({errorDescription})"
        );

        return $"{error}{(string.IsNullOrWhiteSpace(loginFailedMessage.Reason) ? string.Empty : $" (reason: {loginFailedMessage.Reason})")}";
    }
}
