
namespace SupercellProxy.Networking.Protocol.MapGame.Quests;

/// <summary>
/// Represents decoded <c language="csharp">MapGameQuestManagerSnapshot</c> home data.
/// </summary>
public sealed record MapGameQuestManagerSnapshot
{
    /// <summary>Gets whether the daily progression chicken has been collected.</summary>
    public bool ChickenCollected { get; init; }

    /// <summary>Gets the completed-quest position of the daily chicken, or -1 when none is placed.</summary>
    public int ChickenPosition { get; init; } = -1;

    /// <summary>Gets the progression-prize data ids already claimed, in claim order.</summary>
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
    /// Gets the day on which a daily chicken was last placed, or -1 before the first placement.
    /// </summary>
    public int LastChickenDayIndex { get; init; } = -1;
}
