using SupercellProxy.Networking.Protocol.MapGame.Fuel;
using SupercellProxy.Networking.Protocol.MapGame.Notifications;
using SupercellProxy.Networking.Protocol.MapGame.Quests;
using SupercellProxy.Networking.Protocol.MapGame.Wallets;

using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// Represents decoded <c language="csharp">MapGameSnapshot</c> home data.
/// </summary>
public sealed record MapGameSnapshot
{
    /// <summary>
    /// Gets the Event value.
    /// </summary>
    public int Event { get; init; }

    /// <summary>Gets the retained Valley fuel-generator state.</summary>
    public MapGameFuelManagerSnapshot? FuelManager { get; init; }

    /// <summary>
    /// Gets the retained <c language="csharp">MapGameManager</c> state.
    /// </summary>
    [JsonPropertyName("MapGameManager")]
    public MapGameHomeManagerSnapshot? Manager { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">MapGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("MapGlobalId")]
    public int MapGlobalId { get; init; }

    /// <summary>
    /// Gets the Next Expire Hour Index value.
    /// </summary>
    public int NextExpireHourIndex { get; init; }

    /// <summary>Gets the retained pending Valley notifications.</summary>
    public MapGameNotificationManagerSnapshot? NotificationManager { get; init; }

    /// <summary>Gets the retained Valley daily quests and aggregate progression.</summary>
    [JsonPropertyName("QuestrManager")]
    public MapGameQuestManagerSnapshot QuestManager { get; init; } = new();

    /// <summary>Gets the retained Valley wallet and piggy-bank state.</summary>
    public MapGameWalletManagerSnapshot? WalletManager { get; init; }
}
