using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame.Wallets;

/// <summary>Retains one Valley map wallet or the Valley piggy bank.</summary>
public sealed record MapGameWalletSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the coin balances held in this wallet, one entry per Valley coin kind.</summary>
    /// <value>Saved as <c>CoinsList</c>; empty by default.</value>
    [JsonPropertyName("CoinsList")]
    public MapGameCoinBalanceSnapshot[] Coins { get; init; } = [];

    /// <summary>Gets or sets the coins that count toward the season's collection goal, such as chickens or sanctuary animals.</summary>
    /// <value>Global ids of rows in the Valley coin table; empty by default.</value>
    public int[] GlobalCollectGoalData { get; init; } = [];

    /// <summary>Gets or sets whether this wallet is the piggy bank rather than a normal per-game wallet (saved as <c>PiggyBank</c>).</summary>
    /// <value><see langword="true"/> if this is <see cref="MapGameWalletManagerSnapshot.PiggyBank"/>; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("PiggyBank")]
    public bool IsPiggyBank { get; init; }

    /// <summary>Gets or sets the Valley game a normal wallet belongs to.</summary>
    /// <value>The id of the game whose coins this wallet holds; required on normal wallets and <see langword="null"/> (left out of the JSON) on the piggy bank.</value>
    [JsonPropertyName("MapInstanceId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MapGameInstanceIdSnapshot? MapInstanceId { get; init; }

    /// <summary>Gets or sets whether the piggy bank has been broken open.</summary>
    /// <value><see langword="true"/> if the save marks the piggy bank as broken; otherwise, <see langword="false"/>.</value>
    /// <remarks>The flag is carried through unchanged from the save.</remarks>
    public bool PiggyBankBroken { get; init; }

    /// <summary>Gets or sets how many goal coins the season's collection goal asks for.</summary>
    /// <value>The goal count of the Valley event's collection goal.</value>
    public int Tier2RewardsGoalNumber { get; init; }
}
