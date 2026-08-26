using System.Text.Json;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>MapGameQuestSnapshot</c> home data.
/// </summary>
public sealed record MapGameQuestSnapshot
{
    /// <summary>
    /// Gets or sets the <c>CurrentQuests</c> value.
    /// </summary>
    public JsonElement CurrentQuests { get; init; }
}
