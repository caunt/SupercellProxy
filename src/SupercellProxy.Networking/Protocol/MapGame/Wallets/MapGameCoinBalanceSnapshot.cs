using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame.Wallets;

/// <summary>Retains one Valley coin balance.</summary>
public sealed record MapGameCoinBalanceSnapshot : ExtensibleDocument
{
    /// <summary>Gets the retained amount.</summary>
    public int Amount { get; init; }

    /// <summary>Gets the Valley coin data id.</summary>
    [JsonPropertyName("CoinGlobalId")]
    public int CoinGlobalId { get; init; }
}
