using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">ChronosEventsSnapshot</c> home data.
/// </summary>
internal sealed record ChronosEventsSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">EventBoardState</c> value.
    /// </summary>
    [JsonPropertyName("event_board_state")]
    public ChronosEventBoardSnapshot EventBoardState { get; init; } = new();
}
