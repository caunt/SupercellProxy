using System.Globalization;
using System.Runtime.CompilerServices;

using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Client;

/// <summary>
/// Thrown when the server refuses a login. When the refusal came as a server reply, the reply is attached, so callers can react to
/// its error code: download new assets on out-of-date content, refresh the token on an invalid token, or wait out maintenance.
/// </summary>
public sealed class LoginException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginException"/> class. Creates a login failure with no description and no server reply attached.
    /// </summary>
    public LoginException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginException"/> class. Creates a login failure described by text alone, for refusals detected locally rather than reported by the server.
    /// </summary>
    /// <param name="message">What went wrong.</param>
    public LoginException(string? message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginException"/> class. Creates a login failure caused by another error, such as a broken connection during the handshake.
    /// </summary>
    /// <param name="message">What went wrong.</param>
    /// <param name="innerException">The error that stopped the login.</param>
    public LoginException(string? message, Exception? innerException)
        : base(message, innerException) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginException"/> class. Creates a login failure from the server's refusal; the text names the error code and the server's reason.
    /// </summary>
    /// <param name="loginFailedMessage">The server's reply, kept in <see cref="LoginFailedMessage"/>.</param>
    public LoginException(LoginFailedMessage loginFailedMessage)
        : base(GetMessage(loginFailedMessage))
    {
        LoginFailedMessage = loginFailedMessage;
    }

    /// <summary>
    /// Gets the server's refusal, with its error code, reason, maintenance countdown or new asset manifest; <see langword="null"/> when the failure was
    /// not reported by the server.
    /// </summary>
    /// <value>The <see cref="Protocol.Authentication.LoginFailedMessage"/> passed to the constructor, or <see langword="null"/> for a failure created from a plain error text.</value>
    public LoginFailedMessage? LoginFailedMessage { get; }

    /// <summary>
    /// Turns a login refusal into an exception while a handshake reads its replies; any other message passes through.
    /// </summary>
    /// <param name="message">A message received during login.</param>
    /// <exception cref="LoginException">The message is a login refusal.</exception>
    public static void ThrowIfFailed(IMessage message)
    {
        if (message is not LoginFailedMessage loginFailedMessage)
            return;

        throw new LoginException(loginFailedMessage);
    }

    private static string GetMessage(LoginFailedMessage loginFailedMessage)
    {
        string errorDescription = loginFailedMessage.ErrorCode switch
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

        string error = string.Create(CultureInfo.InvariantCulture, $"{Unsafe.BitCast<LoginFailureType, int>(loginFailedMessage.ErrorCode)} ({errorDescription})");

        return $"{error}{(string.IsNullOrWhiteSpace(loginFailedMessage.Reason) ? string.Empty : $" (reason: {loginFailedMessage.Reason})")}";
    }
}
