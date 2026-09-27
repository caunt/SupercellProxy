
namespace SupercellProxy.Networking.Protocol.MapGame.Quests;

/// <summary>
/// Represents decoded <c language="csharp">MapGameQuestManagerSnapshot</c> home data.
/// </summary>
public sealed record MapGameQuestManagerSnapshot
{
    /// <summary>Gets the progression-prize data identifiers already claimed, in claim order.</summary>
    public int[] ClaimedProgressionPrizes { get; init; } = [];

    /// <summary>Gets the number of completed quests in the active Valley progression.</summary>
    public int CompletedQuests { get; init; }

    /// <summary>Gets the completed-quest count last acknowledged by the player.</summary>
    public int CompletedQuestsSeen { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CurrentQuests</c> value.
    /// </summary>
    public MapGameCurrentQuestsSnapshot? CurrentQuests { get; init; }

    /// <summary>Gets whether the current Valley daily quests have been seen.</summary>
    public bool HasSeenCurrentQuests { get; init; }

    /// <summary>
    /// Gets the retained <c language="csharp">LastChickenDayIndex</c> gate.
    /// </summary>
    public int LastChickenDayIndex { get; init; }
}
