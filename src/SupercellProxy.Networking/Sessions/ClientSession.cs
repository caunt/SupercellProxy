using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol;
using SupercellProxy.Networking.Protocol.Authentication;

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

}
