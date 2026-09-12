namespace SupercellProxy.Networking.Protocol.MapGame.Quests;

/// <summary>Identifies a generated or retained Valley daily quest.</summary>
/// <param name="QuestGlobalIdentifier">The quest data identifier.</param>
public sealed record MapGameDailyQuestSnapshot([property: System.Text.Json.Serialization.JsonPropertyName("QuestGlobalId")] int QuestGlobalIdentifier);
