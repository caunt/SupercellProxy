using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>ChronosEventsSnapshot</c> home data.
/// </summary>
public sealed record ChronosEventsSnapshot
{
    /// <summary>
    /// Gets or sets the <c>EventBoardState</c> value.
    /// </summary>
    [JsonPropertyName("event_board_state")]
    public ChronosEventBoardSnapshot EventBoardState { get; init; } = new();
}
