using SupercellProxy.Networking.Protocol.MapGame.Fuel;
using SupercellProxy.Networking.Protocol.MapGame.Notifications;
using SupercellProxy.Networking.Protocol.MapGame.Quests;
using SupercellProxy.Networking.Protocol.MapGame.Rewards;
using SupercellProxy.Networking.Protocol.MapGame.Wallets;

using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>Valley state saved with a home: the season it belongs to, the board, quests, shop, wallet and notifications.</summary>
public sealed record MapGameSnapshot
{
    /// <summary>Gets or sets the id of the live event of the Valley season the state belongs to; 0 when none.</summary>
    /// <value>The event id of the running Valley live event.</value>
    public int Event { get; init; }

    /// <summary>Gets or sets the retained Valley fuel-generator state.</summary>
    /// <value>Sun-point tasks, spins and the fuel prize wheel; <see langword="null"/> when the home has no fuel manager saved.</value>
    public MapGameFuelManagerSnapshot? FuelManager { get; init; }

    /// <summary>
    /// Gets or sets the retained <c language="csharp">MapGameManager</c> state.
    /// </summary>
    /// <value>The saved cars and themed task progress under the <c>MapGameManager</c> key; <see langword="null"/> when not saved.</value>
    [JsonPropertyName("MapGameManager")]
    public MapGameHomeManagerSnapshot? Manager { get; init; }

    /// <summary>Gets or sets the global id of the season's map table entry.</summary>
    /// <value>The global id of a row in the Valley maps table; 0 means no map was chosen yet.</value>
    [JsonPropertyName("MapGlobalId")]
    public int MapGlobalId { get; init; }

    /// <summary>Gets or sets the Unix hour at which the Valley daily quests reset next.</summary>
    /// <value>Hours since 1970-01-01 UTC, at the configured reset hour of a day.</value>
    public int NextExpireHourIndex { get; init; }

    /// <summary>Gets or sets the retained pending Valley notifications.</summary>
    /// <value>The notifications waiting to be shown; <see langword="null"/> when none were saved.</value>
    public MapGameNotificationManagerSnapshot? NotificationManager { get; init; }

    /// <summary>Gets or sets the retained Valley daily quests and aggregate progression.</summary>
    /// <value>The quest manager's state, saved under the native key <c>QuestrManager</c> (spelled that way by the game); an empty manager by default.</value>
    [JsonPropertyName("QuestrManager")]
    public MapGameQuestManagerSnapshot QuestManager { get; init; } = new();

    /// <summary>Gets or sets the retained Valley shop reward inventories.</summary>
    /// <value>One reward inventory per Valley game instance; <see langword="null"/> when no shop state was saved.</value>
    public MapGamePromotionShopSnapshot? ShopManager { get; init; }

    /// <summary>Gets or sets the retained Valley wallet and piggy-bank state.</summary>
    /// <value>One coin wallet per Valley game plus the optional piggy bank; <see langword="null"/> when no wallet state was saved.</value>
    public MapGameWalletManagerSnapshot? WalletManager { get; init; }
}
