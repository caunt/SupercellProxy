using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Sessions;

/// <summary>Describes the session refresh endpoint response.</summary>
public sealed record SessionRefreshResponse
{
    /// <summary>Gets whether the refresh succeeded.</summary>
    [JsonPropertyName("ok")]
    public required bool Success { get; init; }

    /// <summary>Gets the refreshed JWT text.</summary>
    [JsonPropertyName("token")]
    public required string Token { get; init; }
}
