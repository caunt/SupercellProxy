using System.Globalization;

using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Client;

/// <summary>
/// Represents <c language="csharp">LoginException</c>.
/// </summary>
/// <remarks>
/// Initializes a new <see cref="LoginException"/> instance.
/// </remarks>
public sealed class LoginException : Exception
{
    /// <summary>
    /// Provides the Login Exception value or operation.
    /// </summary>
    public LoginException() { }

    /// <summary>
    /// Provides the Login Exception value or operation.
    /// </summary>
    public LoginException(string? message)
        : base(message) { }

    /// <summary>
    /// Provides the Login Exception value or operation.
    /// </summary>
    public LoginException(string? message, Exception? innerException)
        : base(message, innerException) { }

    /// <summary>
    /// Provides the Login Exception value or operation.
    /// </summary>
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

        string error = string.Create(
            CultureInfo.InvariantCulture,
            $"{System.Runtime.CompilerServices.Unsafe.BitCast<LoginFailureType, int>(loginFailedMessage.ErrorCode)} ({errorDescription})"
        );

        return $"{error}{(string.IsNullOrWhiteSpace(loginFailedMessage.Reason) ? string.Empty : $" (reason: {loginFailedMessage.Reason})")}";
    }
}
