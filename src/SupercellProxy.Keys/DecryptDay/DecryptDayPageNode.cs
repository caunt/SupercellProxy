using System.Text.Json.Serialization;

using SupercellProxy.Keys.Svelte;

namespace SupercellProxy.Keys.DecryptDay;

/// <summary>One part of a decrypt.day page's data, as its web framework sends it.</summary>
internal sealed record DecryptDayPageNode
{
    /// <summary>Encoded data of this page part; it is decoded only when it describes an app.</summary>
    [JsonPropertyName("data")]
    public SvelteData? Data { get; init; }
}
