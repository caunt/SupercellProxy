using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace SupercellProxy.Keys.Svelte;

/// <summary>Reads and writes the reference table at the Svelte codec boundary.</summary>
internal sealed class SvelteDataConverter : JsonConverter<SvelteData>
{
    /// <inheritdoc />
    public override SvelteData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.StartArray)
        {
            reader.Skip();

            return null;
        }

        return JsonNode.Parse(ref reader) is JsonArray values ? new SvelteData(values) : null;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, SvelteData value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(value);
        value.Encode(writer, options);
    }
}
