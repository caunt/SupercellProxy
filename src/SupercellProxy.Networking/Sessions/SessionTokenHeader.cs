using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Sessions;

/// <summary>Describes the signed session token's algorithm, key, and media type.</summary>
public sealed record SessionTokenHeader
{
    /// <summary>Gets the signature algorithm.</summary>
    [JsonPropertyName("alg")]
    public required string Alg { get; init; }

    /// <summary>Gets the signing key identifier.</summary>
    [JsonPropertyName("kid")]
    public string? Kid { get; init; }

    /// <summary>Gets the token media type.</summary>
    [JsonPropertyName("typ")]
    public string? Typ { get; init; }
}
