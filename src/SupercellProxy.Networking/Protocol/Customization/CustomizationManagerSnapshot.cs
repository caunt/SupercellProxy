using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.Customization;

/// <summary>
/// Represents decoded <c language="csharp">CustomizationManagerSnapshot</c> home data.
/// </summary>
public sealed record CustomizationManagerSnapshot
{
    /// <summary>Gets or sets the event id that last forced the stock issue.</summary>
    /// <value>The live event id of the renovator event the current stock was built under, saved as <c>ForcedStockChronosEventID</c>; 0 when it was built without one.</value>
    [JsonPropertyName("ForcedStockChronosEventID")]
    public int ForcedStockChronosEventId { get; init; }

    /// <summary>Gets or sets the three offered-tag acknowledgement flags.</summary>
    /// <value>Three flags, or <see langword="null"/> when the save omits them; which tag each flag stands for is not identified.</value>
    public bool[]? InitialCustomizationTagsOfferedSeen { get; init; }

    /// <summary>Gets or sets the history of offered customization parts.</summary>
    /// <value>Part data global ids the renovator ever offered, each listed once; <see langword="null"/> when the save omits the list.</value>
    public int[]? PartsSeen { get; init; }

    /// <summary>Gets or sets the parts sold from the current issue.</summary>
    /// <value>Part data global ids bought from the current stock; <see langword="null"/> when the save omits the list.</value>
    public int[]? PartsSoldFromCurrentStock { get; init; }

    /// <summary>Gets or sets the retained used customization parts.</summary>
    /// <value>Part data global ids the player has owned before; <see langword="null"/> when the save omits the list.</value>
    public int[]? PartsUsed { get; init; }

    /// <summary>Gets or sets the current free renovator parts.</summary>
    /// <value>Part data global ids the current stock offers at no coin, diamond or voucher cost; <see langword="null"/> when the save omits the list.</value>
    public int[]? RenovatorFreeStock { get; init; }

    /// <summary>Gets or sets the retained reroll state.</summary>
    /// <value>One category number per reroll made on the current stock, so a category's entry count is its rerolls so far.</value>
    public int[]? RenovatorRerolls { get; init; }

    /// <summary>Gets or sets the current offered renovator parts, in category order.</summary>
    /// <value>One part data global id per category, from truck body to shop base; <see langword="null"/> when the save omits the list.</value>
    public int[]? RenovatorStock { get; init; }

    /// <summary>
    /// Gets or sets the retained customization random state.
    /// </summary>
    /// <value>The saved state of the renovator's own random generator; <see langword="null"/> when the save omits it.</value>
    public int? Seed { get; init; }

    /// <summary>Gets or sets the explicitly selected customization parts.</summary>
    /// <value>Part data global ids the player has applied; <see langword="null"/> when the save omits the list.</value>
    public int[]? SelectedOptions { get; init; }

    /// <summary>
    /// Gets or sets the saved time value of the renovator's regular stock, in seconds, next to the separate tutorial stock countdown.
    /// </summary>
    /// <value>The Unix time in seconds the current stock was scheduled to arrive.</value>
    public int StockSeconds { get; init; }

    /// <summary>Gets or sets a value indicating whether the tutorial stock timer counts down.</summary>
    /// <value><see langword="true"/> if the tutorial stock is running down; otherwise, <see langword="false"/>.</value>
    public bool TutorialStockActive { get; init; }

    /// <summary>Gets or sets the retained tutorial stock countdown.</summary>
    /// <value>The countdown of the introductory stock, or <see langword="null"/> when the save has none.</value>
    public TimerSnapshot? TutorialStockSeconds { get; init; }
}
