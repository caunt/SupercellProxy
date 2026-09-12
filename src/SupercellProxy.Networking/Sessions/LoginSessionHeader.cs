using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Sessions;

/// <summary>Describes the signed session token's algorithm, key, and media type.</summary>
public sealed record LoginSessionHeader
{
    /// <summary>Gets the signature algorithm.</summary>
    [JsonPropertyName("alg")]
    public required string Algorithm { get; init; }

    /// <summary>Gets the signing key identifier.</summary>
    [JsonPropertyName("kid")]
    public string? KeyIdentifier { get; init; }

    /// <summary>Gets the token media type.</summary>
    [JsonPropertyName("typ")]
    public string? Type { get; init; }
}
