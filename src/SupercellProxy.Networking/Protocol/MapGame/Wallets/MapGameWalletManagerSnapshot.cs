using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame.Wallets;

/// <summary>Retains normal Valley wallets and the optional Valley piggy bank.</summary>
public sealed record MapGameWalletManagerSnapshot : ExtensibleDocument
{
    /// <summary>Gets the optional Valley piggy bank.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MapGameWalletSnapshot? PiggyBank { get; init; }

    /// <summary>Gets the normal per-map wallets.</summary>
    [JsonPropertyName("MapGameWallets")]
    public MapGameWalletSnapshot[] Wallets { get; init; } = [];
}
