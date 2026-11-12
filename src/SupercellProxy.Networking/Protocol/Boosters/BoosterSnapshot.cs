using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>
/// Represents decoded <c language="csharp">BoosterSnapshot</c> home data.
/// </summary>
public sealed record BoosterSnapshot
{
    /// <summary>
    /// Gets or sets the global id of the booster type in the boosters table.
    /// </summary>
    /// <value>The booster data global id saved as <c>LogicBoosterDataGlobalID</c>; it decides the booster's effect type and target.</value>
    [JsonPropertyName("LogicBoosterDataGlobalID")]
    public int BoosterDataGlobalId { get; init; }

    /// <summary>Gets or sets the retained game mode the booster is active in.</summary>
    /// <value>The native game mode the booster works in, such as 1 for the own farm, 4 for the fishing area or 6 for the own town; 1 by default.</value>
    public int GameMode { get; init; } = 1;

    /// <summary>Gets or sets the production duration retained when this booster was attached.</summary>
    /// <value>The production's remaining time in seconds when the booster was attached to it; 0 for boosters not attached to a production.</value>
    public int ProductionTime { get; init; }

    /// <summary>Gets or sets the production duration after applying the attached booster.</summary>
    /// <value>The shortened remaining time in seconds; the difference from <see cref="ProductionTime"/> is the time the booster already covers.</value>
    public int ReducedProductionTime { get; init; }

    /// <summary>
    /// Gets or sets the countdown until the booster wears off.
    /// </summary>
    /// <value>The remaining duration timer of the booster.</value>
    public TimerSnapshot Timer { get; init; }
}
