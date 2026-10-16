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
    /// <summary>Id of the live event of the Valley season the state belongs to; 0 when none.</summary>
    public int Event { get; init; }

    /// <summary>Gets the retained Valley fuel-generator state.</summary>
    public MapGameFuelManagerSnapshot? FuelManager { get; init; }

    /// <summary>
    /// Gets the retained <c language="csharp">MapGameManager</c> state.
    /// </summary>
    [JsonPropertyName("MapGameManager")]
    public MapGameHomeManagerSnapshot? Manager { get; init; }

    /// <summary>Global id of the season's map table entry.</summary>
    [JsonPropertyName("MapGlobalId")]
    public int MapGlobalId { get; init; }

    /// <summary>Unix hour at which the Valley daily quests reset next.</summary>
    public int NextExpireHourIndex { get; init; }

    /// <summary>Gets the retained pending Valley notifications.</summary>
    public MapGameNotificationManagerSnapshot? NotificationManager { get; init; }

    /// <summary>Gets the retained Valley daily quests and aggregate progression.</summary>
    [JsonPropertyName("QuestrManager")]
    public MapGameQuestManagerSnapshot QuestManager { get; init; } = new();

    /// <summary>Gets the retained Valley shop reward inventories.</summary>
    public MapGamePromotionShopSnapshot? ShopManager { get; init; }

    /// <summary>Gets the retained Valley wallet and piggy-bank state.</summary>
    public MapGameWalletManagerSnapshot? WalletManager { get; init; }
}
