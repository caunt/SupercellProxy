using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame.Quests;

/// <summary>Represents decoded MapGameCurrentQuestsSnapshot state.</summary>
public sealed record MapGameCurrentQuestsSnapshot : ExtensibleDocument
{
    /// <summary>Gets the Quests value.</summary>
    [JsonPropertyName("Quests")]
    public MapGameDailyQuestSnapshot[] Quests { get; init; } = [];

}
