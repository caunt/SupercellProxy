using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>
/// Represents decoded <c language="csharp">BoosterSnapshot</c> home data.
/// </summary>
public sealed record BoosterSnapshot
{
    /// <summary>
    /// Global id of the booster type in the boosters table.
    /// </summary>
    [JsonPropertyName("LogicBoosterDataGlobalID")]
    public int BoosterDataGlobalId { get; init; }

    /// <summary>Gets the retained game mode the booster is active in.</summary>
    public int GameMode { get; init; } = 1;

    /// <summary>Gets the production duration retained when this booster was attached.</summary>
    public int ProductionTime { get; init; }

    /// <summary>Gets the production duration after applying the attached booster.</summary>
    public int ReducedProductionTime { get; init; }

    /// <summary>
    /// Countdown until the booster wears off.
    /// </summary>
    public TimerSnapshot Timer { get; init; }
}
