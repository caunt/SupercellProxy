namespace SupercellProxy.Networking.Protocol.MapGame.Quests;

/// <summary>Identifies a generated or retained Valley daily quest.</summary>
/// <param name="QuestGlobalIdentifier">The quest data identifier.</param>
public sealed record MapGameDailyQuestSnapshot([property: System.Text.Json.Serialization.JsonPropertyName("QuestGlobalId")] int QuestGlobalIdentifier)
{
    /// <summary>Gets whether the quest target has been reached.</summary>
    public bool Complete { get; init; }
    /// <summary>Gets the retained progress, capped at the quest target on completion.</summary>
    public int Progress { get; init; }
    /// <summary>Gets whether completion has been presented to the player.</summary>
    public bool Seen { get; init; }
}
