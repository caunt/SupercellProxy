using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace SupercellProxy.Keys.Svelte;

/// <summary>Decodes Svelte's reference-table wire format through concrete application contracts.</summary>
[JsonConverter(typeof(SvelteDataConverter))]
internal sealed class SvelteData
{
    private readonly JsonArray _values;

    internal SvelteData(JsonArray values)
    {
        _values = values;
    }

    /// <summary>Deserializes the expanded reference table through the requested concrete contract.</summary>
    public TValue? Decode<TValue>(JsonTypeInfo<TValue> contract) where TValue : class
    {
        return SvelteDataDecoder.Decode(_values, contract);
    }

    internal void Encode(System.Text.Json.Utf8JsonWriter writer, JsonSerializerOptions options)
    {
        _values.WriteTo(writer, options);
    }
}
