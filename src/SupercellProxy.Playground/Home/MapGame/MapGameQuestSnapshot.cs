using System.Text.Json;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">MapGameQuestSnapshot</c> home data.
/// </summary>
internal sealed record MapGameQuestSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">CurrentQuests</c> value.
    /// </summary>
    public JsonElement CurrentQuests { get; init; }

    /// <summary>
    /// Gets the retained <c language="csharp">LastChickenDayIndex</c> gate.
    /// </summary>
    public int LastChickenDayIndex { get; init; }
}
