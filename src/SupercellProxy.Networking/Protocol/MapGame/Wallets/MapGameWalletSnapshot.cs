using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame.Wallets;

/// <summary>Retains one Valley map wallet or the Valley piggy bank.</summary>
public sealed record MapGameWalletSnapshot : ExtensibleDocument
{
    /// <summary>Gets the retained coin balances.</summary>
    [JsonPropertyName("CoinsList")]
    public MapGameCoinBalanceSnapshot[] Coins { get; init; } = [];

    /// <summary>Gets the configured global collection-goal coin ids.</summary>
    public int[] GlobalCollectGoalData { get; init; } = [];

    /// <summary>Gets whether this wallet is the piggy bank.</summary>
    [JsonPropertyName("PiggyBank")]
    public bool IsPiggyBank { get; init; }

    /// <summary>Gets the map instance owned by a normal wallet.</summary>
    [JsonPropertyName("MapInstanceId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MapGameInstanceIdSnapshot? MapInstanceId { get; init; }

    /// <summary>Gets whether the retained piggy bank has been broken.</summary>
    public bool PiggyBankBroken { get; init; }

    /// <summary>Gets the tier-two reward goal.</summary>
    public int Tier2RewardsGoalNumber { get; init; }
}
