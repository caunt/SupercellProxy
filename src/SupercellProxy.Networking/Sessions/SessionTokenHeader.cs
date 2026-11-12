using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Sessions;

/// <summary>Describes the signed session token's algorithm, key, and media type.</summary>
public sealed record SessionTokenHeader
{
    /// <summary>Gets or sets the signature algorithm.</summary>
    /// <value>The JOSE <c>alg</c> header, required; decoding rejects any token whose value is not exactly <c>ES256</c>.</value>
    [JsonPropertyName("alg")]
    public required string Alg { get; init; }

    /// <summary>Gets or sets the signing key id.</summary>
    /// <value>The JOSE <c>kid</c> header naming the issuer key that signed the token, or <see langword="null"/> when absent; kept for inspection only, since signatures are not verified here.</value>
    [JsonPropertyName("kid")]
    public string? Kid { get; init; }

    /// <summary>Gets or sets the token media type.</summary>
    /// <value>The JOSE <c>typ</c> header (conventionally <c>JWT</c>), or <see langword="null"/> when absent; it is not checked.</value>
    [JsonPropertyName("typ")]
    public string? Typ { get; init; }
}
