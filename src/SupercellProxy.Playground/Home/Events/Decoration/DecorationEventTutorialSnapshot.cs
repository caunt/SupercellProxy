using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

internal sealed record DecorationEventTutorialSnapshot
{
    [JsonPropertyName("lastIntroEventId")]
    public int LastIntroEventId { get; init; } = -1;

    [JsonPropertyName("lastIntroStep")]
    public int LastIntroStep { get; init; } = -1;
}
