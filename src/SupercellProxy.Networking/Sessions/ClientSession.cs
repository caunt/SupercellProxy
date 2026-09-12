using System.Text.Json.Serialization;

using SupercellProxy.Networking.Client;
using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.Homes;
using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Sessions;

/// <summary>Stores one authenticated game account and its session credentials.</summary>
public sealed record ClientSession
{
    /// <summary>Provides the default application store.</summary>
    public const AppStore DefaultAppStore = AppStore.GooglePlay;

    /// <summary>Gets the authenticated game account.</summary>
    [JsonPropertyName("AccountId")]
    public required LongIdentifier AccountIdentifier { get; init; }

    /// <summary>Gets the application store used by the account.</summary>
    public AppStore AppStore { get; init; } = DefaultAppStore;

    /// <summary>Gets the in-game farm name.</summary>
    public required string FarmName { get; init; }

    /// <summary>Gets the game-account pass token.</summary>
    public required string PassToken { get; init; }

    /// <summary>Gets the optional Supercell ID refresh token.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SessionRefreshToken { get; init; }

    /// <summary>Gets the optional decoded Supercell ID session token.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LoginSessionToken? SessionToken { get; init; }

    /// <summary>Creates a session from a successful login followed by matching own-home data.</summary>
    /// <exception cref="LoginException">The login result is a failure.</exception>
    /// <exception cref="InvalidDataException">The login or home response does not describe one complete account.</exception>
    /// <exception cref="UnauthorizedAccessException">The response identifies different credentials from the request.</exception>
    public static ClientSession FromLoginOutcome(LoginMessage loginMessage, IMessage loginResult, OwnHomeDataMessage ownHomeDataMessage)
    {
        ArgumentNullException.ThrowIfNull(loginMessage);
        ArgumentNullException.ThrowIfNull(loginResult);
        ArgumentNullException.ThrowIfNull(ownHomeDataMessage);

        LoginException.ThrowIfFailed(loginResult);

        LoginOkMessage loginOkMessage = loginResult as LoginOkMessage
            ?? throw new InvalidDataException($"Expected {nameof(LoginOkMessage)}, but received {loginResult.GetType().Name}.");

        bool mismatchedRequestAccount = loginMessage.AccountIdentifier != LongIdentifier.Empty
            && loginMessage.AccountIdentifier != loginOkMessage.AccountIdentifier;

        if (mismatchedRequestAccount)
            throw new UnauthorizedAccessException(message: "Authentication returned a different account from the requested session.");

        bool mismatchedPassToken = loginMessage.PassToken is not null
            && !string.Equals(loginMessage.PassToken, loginOkMessage.PassToken, StringComparison.Ordinal);

        if (mismatchedPassToken)
            throw new UnauthorizedAccessException(message: "Authentication returned a different pass token from the requested session.");

        if (ownHomeDataMessage.ClientAvatar.AccountIdentifier != loginOkMessage.AccountIdentifier)
            throw new InvalidDataException(message: "Own-home data belongs to a different account from the successful login.");

        string farmName = ownHomeDataMessage.ClientAvatar.FarmName
            ?? throw new InvalidDataException(message: "Own-home data has no farm name.");

        return string.IsNullOrWhiteSpace(farmName)
            ? throw new InvalidDataException(message: "Own-home data has an empty farm name.")
            : new ClientSession
            {
                AccountIdentifier = loginOkMessage.AccountIdentifier,
                AppStore = loginMessage.AppStore,
                FarmName = farmName,
                PassToken = loginOkMessage.PassToken,
                SessionToken = loginMessage.SessionToken is { IsEmpty: false } sessionToken ? sessionToken : null,
            };
    }
}
