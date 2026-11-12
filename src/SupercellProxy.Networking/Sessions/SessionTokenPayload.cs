using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Sessions;

/// <summary>Contains the identity, application, and lifetime claims of a Supercell ID session.</summary>
public sealed record SessionTokenPayload
{
    /// <summary>Gets or sets the application.</summary>
    /// <value>The Supercell ID <c>app</c> claim naming the game application the session was issued for, or <see langword="null"/> when absent; not interpreted here.</value>
    [JsonPropertyName("https://id.supercell.com/app")]
    public string? App { get; init; }

    /// <summary>Gets or sets the linked game account.</summary>
    /// <value>The Supercell ID <c>appAccountId</c> claim, the game account bound to the session, as the issuer's string form; <see langword="null"/> when absent.</value>
    [JsonPropertyName("https://id.supercell.com/appAccountId")]
    public string? AppAccountId { get; init; }

    /// <summary>Gets or sets the application's environment.</summary>
    /// <value>The Supercell ID <c>appEnv</c> claim naming the game environment the account lives in, or <see langword="null"/> when absent; not interpreted here.</value>
    [JsonPropertyName("https://id.supercell.com/appEnv")]
    public string? AppEnvironment { get; init; }

    /// <summary>Gets or sets the intended recipient.</summary>
    /// <value>The standard JWT <c>aud</c> claim naming who should accept the token, or <see langword="null"/> when absent; not checked here.</value>
    [JsonPropertyName("aud")]
    public string? Aud { get; init; }

    /// <summary>Gets or sets the session environment.</summary>
    /// <value>The <c>env</c> claim naming the Supercell ID environment that issued the session, or <see langword="null"/> when absent; not interpreted here.</value>
    [JsonPropertyName("env")]
    public string? Environment { get; init; }

    /// <summary>Gets or sets the expiration time in Unix seconds.</summary>
    /// <value>The required <c>exp</c> claim; decoding rejects a token whose expiry is not after <see cref="Iat"/>, and authentication rejects one whose expiry has passed.</value>
    [JsonPropertyName("exp")]
    public required long Exp { get; init; }

    /// <summary>Gets or sets the game id.</summary>
    /// <value>The <c>game</c> claim identifying the Supercell game the session belongs to, or <see langword="null"/> when absent; not interpreted here.</value>
    [JsonPropertyName("game")]
    public string? Game { get; init; }

    /// <summary>Gets or sets the issue time in Unix seconds.</summary>
    /// <value>The required <c>iat</c> claim, the start of the lifetime used to decide when the token should be refreshed.</value>
    [JsonPropertyName("iat")]
    public required long Iat { get; init; }

    /// <summary>Gets or sets the original refresh token's issue time in Unix seconds.</summary>
    /// <value>The Supercell ID <c>initialRefreshTokenIssuedAt</c> claim, unchanged across refreshes of one sign-in, or <see langword="null"/> when the token does not carry it.</value>
    [JsonPropertyName("https://id.supercell.com/initialRefreshTokenIssuedAt")]
    public long? InitialRefreshTokenIssuedAt { get; init; }

    /// <summary>Gets or sets the issuing authority.</summary>
    /// <value>The standard JWT <c>iss</c> claim naming the service that issued the token, or <see langword="null"/> when absent; not checked here.</value>
    [JsonPropertyName("iss")]
    public string? Iss { get; init; }

    /// <summary>Gets or sets the player id.</summary>
    /// <value>The <c>pid</c> claim with the issuer's player identifier as text, or <see langword="null"/> when absent; not interpreted here.</value>
    [JsonPropertyName("pid")]
    public string? Pid { get; init; }

    /// <summary>Gets or sets the Supercell ID account id.</summary>
    /// <value>The <c>scid</c> claim identifying the Supercell ID account that signed in, as text, or <see langword="null"/> when absent.</value>
    [JsonPropertyName("scid")]
    public string? Scid { get; init; }

    /// <summary>Gets or sets the session subject.</summary>
    /// <value>The standard JWT <c>sub</c> claim naming the principal the token is about, or <see langword="null"/> when absent; not interpreted here.</value>
    [JsonPropertyName("sub")]
    public string? Sub { get; init; }

    /// <summary>Gets or sets the session token type.</summary>
    /// <value>The Supercell ID <c>type</c> claim; only a token whose type is exactly <c>session</c> may be used to log in, and <see langword="null"/> when absent.</value>
    [JsonPropertyName("https://id.supercell.com/type")]
    public string? Type { get; init; }
}
