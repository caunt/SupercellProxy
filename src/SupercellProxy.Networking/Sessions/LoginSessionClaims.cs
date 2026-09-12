using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Sessions;

/// <summary>Contains the identity, application, and lifetime claims of a Supercell ID session.</summary>
public sealed record LoginSessionClaims
{
    /// <summary>Gets the application.</summary>
    [JsonPropertyName("https://id.supercell.com/app")]
    public string? Application { get; init; }

    /// <summary>Gets the linked game account.</summary>
    [JsonPropertyName("https://id.supercell.com/appAccountId")]
    public string? ApplicationAccountIdentifier { get; init; }

    /// <summary>Gets the application's environment.</summary>
    [JsonPropertyName("https://id.supercell.com/appEnv")]
    public string? ApplicationEnvironment { get; init; }

    /// <summary>Gets the intended recipient.</summary>
    [JsonPropertyName("aud")]
    public string? Audience { get; init; }

    /// <summary>Gets the session environment.</summary>
    [JsonPropertyName("env")]
    public string? Environment { get; init; }

    /// <summary>Gets the expiration time in Unix seconds.</summary>
    [JsonPropertyName("exp")]
    public required long ExpiresAt { get; init; }

    /// <summary>Gets the game identifier.</summary>
    [JsonPropertyName("game")]
    public string? Game { get; init; }

    /// <summary>Gets the original refresh token's issue time in Unix seconds.</summary>
    [JsonPropertyName("https://id.supercell.com/initialRefreshTokenIssuedAt")]
    public long? InitialRefreshTokenIssuedAt { get; init; }

    /// <summary>Gets the issue time in Unix seconds.</summary>
    [JsonPropertyName("iat")]
    public required long IssuedAt { get; init; }

    /// <summary>Gets the issuing authority.</summary>
    [JsonPropertyName("iss")]
    public string? Issuer { get; init; }

    /// <summary>Gets the player identifier.</summary>
    [JsonPropertyName("pid")]
    public string? PlayerIdentifier { get; init; }

    /// <summary>Gets the session subject.</summary>
    [JsonPropertyName("sub")]
    public string? Subject { get; init; }

    /// <summary>Gets the Supercell ID account identifier.</summary>
    [JsonPropertyName("scid")]
    public string? SupercellIdentifier { get; init; }

    /// <summary>Gets the session token type.</summary>
    [JsonPropertyName("https://id.supercell.com/type")]
    public string? Type { get; init; }
}
