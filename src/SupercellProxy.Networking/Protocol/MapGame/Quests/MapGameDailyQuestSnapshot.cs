using System.Text.Json.Serialization;
namespace SupercellProxy.Networking.Protocol.MapGame.Quests;

/// <summary>Identifies a generated or retained Valley daily quest.</summary>
/// <param name="QuestGlobalId">The quest data id.</param>
public sealed record MapGameDailyQuestSnapshot([property: JsonPropertyName("QuestGlobalId")] int QuestGlobalId)
{
    /// <summary>Gets or sets whether the quest target has been reached.</summary>
    /// <value><see langword="true"/> if <see cref="Progress"/> reached the quest's <c>Param1</c> target; otherwise, <see langword="false"/>.</value>
    /// <remarks>The flag defaults to <see langword="false"/>.</remarks>
    public bool Complete { get; init; }

    /// <summary>Gets or sets the retained progress.</summary>
    /// <value>A non-negative count toward the quest's <c>Param1</c> target.</value>
    public int Progress { get; init; }

    /// <summary>Gets or sets whether completion has been presented to the player.</summary>
    /// <value><see langword="true"/> if the player was shown the completed quest; otherwise, <see langword="false"/>.</value>
    public bool Seen { get; init; }
}
