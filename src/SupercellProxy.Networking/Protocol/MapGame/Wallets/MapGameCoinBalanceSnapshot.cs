using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame.Wallets;

/// <summary>Retains one Valley coin balance.</summary>
public sealed record MapGameCoinBalanceSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the retained amount.</summary>
    /// <value>A non-negative coin count.</value>
    public int Amount { get; init; }

    /// <summary>Gets or sets the Valley coin data id.</summary>
    /// <value>The global id of a row in the Valley coins table; each coin appears at most once per wallet.</value>
    [JsonPropertyName("CoinGlobalId")]
    public int CoinGlobalId { get; init; }
}
