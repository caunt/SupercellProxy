using System.Text.Json.Serialization;

using SupercellProxy.Keys.Svelte;

namespace SupercellProxy.Keys.DecryptDay;

/// <summary>Represents the typed DecryptDayPageNode response contract.</summary>
internal sealed record DecryptDayPageNode
{
    /// <summary>Gets the Data value.</summary>
    [JsonPropertyName("data")]
    public SvelteData? Data { get; init; }

}
