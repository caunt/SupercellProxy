using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.MapGame.Quests;

/// <summary>Today's Valley daily quests.</summary>
public sealed record MapGameCurrentQuestsSnapshot : ExtensibleDocument
{
    /// <summary>Each of today's daily quests with its progress.</summary>
    [JsonPropertyName("Quests")]
    public MapGameDailyQuestSnapshot[] Quests { get; init; } = [];
}
