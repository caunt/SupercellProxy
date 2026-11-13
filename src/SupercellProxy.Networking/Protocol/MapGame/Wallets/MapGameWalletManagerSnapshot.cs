using System.Collections.Immutable;
using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame.Wallets;

/// <summary>Retains normal Valley wallets and the optional Valley piggy bank.</summary>
public sealed record MapGameWalletManagerSnapshot : ExtensibleDocument
{
    /// <summary>Gets or sets the Valley piggy bank, a separate wallet that piggy-bank rewards are added to.</summary>
    /// <value>A wallet whose <see cref="MapGameWalletSnapshot.IsPiggyBank"/> must be <see langword="true"/>, or <see langword="null"/> (left out of the JSON) when the home has no piggy bank.</value>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MapGameWalletSnapshot? PiggyBank { get; init; }

    /// <summary>Gets or sets the normal wallets, one per Valley game, that hold the coins earned in that game.</summary>
    /// <value>Saved as <c>MapGameWallets</c>; each entry carries a <see cref="MapGameWalletSnapshot.MapInstanceId"/> and is not a piggy bank. Empty by default.</value>
    [JsonPropertyName("MapGameWallets")]
    public ImmutableArray<MapGameWalletSnapshot> Wallets { get; init; } = [];
}
