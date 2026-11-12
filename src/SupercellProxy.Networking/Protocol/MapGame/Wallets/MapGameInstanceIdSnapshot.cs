using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame.Wallets;

/// <summary>Identifies one retained Valley map instance.</summary>
public sealed record MapGameInstanceIdSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the high 32-bit word of the Valley game id the wallet belongs to.</summary>
    /// <value>The <see cref="LongId.HighInt32"/> half of the game's <see cref="LongId"/>, saved under the JSON key <c>h</c>.</value>
    [JsonPropertyName("h")]
    public int High { get; init; }

    /// <summary>Gets or sets the low 32-bit word of the Valley game id the wallet belongs to.</summary>
    /// <value>The <see cref="LongId.LowInt32"/> half of the game's <see cref="LongId"/>, saved under the JSON key <c>l</c>.</value>
    [JsonPropertyName("l")]
    public int Low { get; init; }
}
