using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.Customization;

/// <summary>
/// Represents decoded <c language="csharp">CustomizationManagerSnapshot</c> home data.
/// </summary>
public sealed record CustomizationManagerSnapshot
{
    /// <summary>Gets the event id that last forced the stock issue.</summary>
    [JsonPropertyName("ForcedStockChronosEventID")]
    public int ForcedStockChronosEventId { get; init; }

    /// <summary>Gets the three offered-tag acknowledgement flags.</summary>
    public bool[]? InitialCustomizationTagsOfferedSeen { get; init; }

    /// <summary>Gets the history of offered customization parts.</summary>
    public int[]? PartsSeen { get; init; }

    /// <summary>Gets the parts sold from the current issue.</summary>
    public int[]? PartsSoldFromCurrentStock { get; init; }

    /// <summary>Gets the retained used customization parts.</summary>
    public int[]? PartsUsed { get; init; }

    /// <summary>Gets the current free renovator parts.</summary>
    public int[]? RenovatorFreeStock { get; init; }

    /// <summary>Gets the retained reroll state.</summary>
    public int[]? RenovatorRerolls { get; init; }

    /// <summary>Gets the current offered renovator parts, in category order.</summary>
    public int[]? RenovatorStock { get; init; }

    /// <summary>
    /// Gets or sets the retained customization random state.
    /// </summary>
    public int? Seed { get; init; }

    /// <summary>Gets the explicitly selected customization parts.</summary>
    public int[]? SelectedOptions { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">StockSeconds</c> value.
    /// </summary>
    public int StockSeconds { get; init; }

    /// <summary>Gets whether the tutorial stock timer counts down.</summary>
    public bool TutorialStockActive { get; init; }

    /// <summary>Gets the retained tutorial stock countdown.</summary>
    public TimerSnapshot? TutorialStockSeconds { get; init; }
}
