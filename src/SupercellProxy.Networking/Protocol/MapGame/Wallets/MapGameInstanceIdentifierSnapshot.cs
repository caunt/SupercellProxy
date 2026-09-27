using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame.Wallets;

/// <summary>Identifies one retained Valley map instance.</summary>
public sealed record MapGameInstanceIdentifierSnapshot : ExtensibleDocument
{
    /// <summary>Gets the high 32-bit word.</summary>
    [JsonPropertyName("h")]
    public int High { get; init; }

    /// <summary>Gets the low 32-bit word.</summary>
    [JsonPropertyName("l")]
    public int Low { get; init; }
}
