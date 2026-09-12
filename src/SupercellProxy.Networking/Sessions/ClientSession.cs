using System.Text.Json.Serialization;

using SupercellProxy.Networking.Client;
using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Sessions;

/// <summary>Stores the account identity and authentication tokens used by protocol clients and proxies.</summary>
public sealed record ClientSession
{
    /// <summary>
    /// Provides the Default App Store value or operation.
    /// </summary>
    public const AppStore DefaultAppStore = AppStore.GooglePlay;

    /// <summary>
    /// Gets the Account Id value.
    /// </summary>
    [JsonPropertyName("AccountId")]
    public string? AccountIdentifier { get; init; }

    /// <summary>
    /// Gets the Account Id High value.
    /// </summary>
    [JsonPropertyName("AccountIdHigh")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AccountIdentifierHigh { get; init; }

    /// <summary>
    /// Gets the Account Id Low value.
    /// </summary>
    [JsonPropertyName("AccountIdLow")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AccountIdentifierLow { get; init; }

    /// <summary>
    /// Gets the App Store value.
    /// </summary>
    public AppStore AppStore { get; init; } = DefaultAppStore;

    /// <summary>
    /// Gets the Compressed Data value.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public byte[]? CompressedData { get; init; }

    /// <summary>
    /// Gets the Parsed Account Id value.
    /// </summary>
    [JsonPropertyName("ParsedAccountId")]
    public LongIdentifier ParsedAccountIdentifier =>
        AccountIdentifier is not null
            ? LongIdentifier.Parse(AccountIdentifier)
            : new LongIdentifier(AccountIdentifierHigh ?? 0, AccountIdentifierLow ?? 0);

    /// <summary>
    /// Gets the Pass Token value.
    /// </summary>
    public required string PassToken { get; init; }

    /// <summary>
    /// Gets the Session Refresh Token value.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SessionRefreshToken { get; init; }

    /// <summary>
    /// Gets the Session Token value.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SessionToken { get; init; }

    /// <summary>Creates a session from a completed login exchange.</summary>
    /// <exception cref="LoginException">The login result is a failure.</exception>
    /// <exception cref="InvalidDataException">The login result is not a supported authentication outcome.</exception>
    /// <exception cref="UnauthorizedAccessException">The response identifies different credentials from the request.</exception>
    public static ClientSession FromLoginOutcome(LoginMessage loginMessage, IMessage loginResult)
    {
        ArgumentNullException.ThrowIfNull(loginMessage);
        ArgumentNullException.ThrowIfNull(loginResult);

        LoginException.ThrowIfFailed(loginResult);

        LoginOkMessage loginOkMessage = loginResult as LoginOkMessage
            ?? throw new InvalidDataException($"Expected {nameof(LoginOkMessage)}, but received {loginResult.GetType().Name}.");

        return loginMessage.AccountIdentifier != LongIdentifier.Empty && loginMessage.AccountIdentifier != loginOkMessage.AccountIdentifier
            ? throw new UnauthorizedAccessException(message: "Authentication returned a different account from the requested session.")
            : loginMessage.PassToken is not null && !string.Equals(loginMessage.PassToken, loginOkMessage.PassToken, StringComparison.Ordinal)
            ? throw new UnauthorizedAccessException(message: "Authentication returned a different pass token from the requested session.")
            : new ClientSession
            {
                AccountIdentifier = loginOkMessage.AccountIdentifier.ToFormattedString(),
                AppStore = loginMessage.AppStore,
                PassToken = loginOkMessage.PassToken,
                CompressedData = loginMessage.SessionToken?.Encode(),
            };
    }

}
