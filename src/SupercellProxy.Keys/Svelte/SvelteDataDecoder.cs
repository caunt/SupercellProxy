using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;

namespace SupercellProxy.Keys.Svelte;

internal static class SvelteDataDecoder
{
    public static TValue? Decode<TValue>(JsonArray values, JsonTypeInfo<TValue> contract) where TValue : class
    {
        if (values.Count is 0)
            return null;

        JsonNode?[] cache = new JsonNode?[values.Count];
        bool[] cached = new bool[values.Count];
        bool[] active = new bool[values.Count];

        return Resolve(index: 0)?.Deserialize(contract);

        JsonNode? Resolve(int index)
        {
            if (index < 0 || index >= values.Count)
                return null;

            if (cached[index])
                return cache[index]?.DeepClone();

            if (active[index])
                return null;

            active[index] = true;
            JsonNode? result = Expand(values[index]);
            active[index] = false;

            cache[index] = result;
            cached[index] = true;

            return result?.DeepClone();
        }

        JsonNode? Expand(JsonNode? value)
        {
            if (value is JsonValue scalar && scalar.TryGetValue<int>(out int reference))
                return Resolve(reference);

            if (value is JsonArray array)
                return new JsonArray([.. array.Select(Expand)]);

            if (value is JsonObject source)
            {
                JsonObject result = [];

                foreach (KeyValuePair<string, JsonNode?> property in source)
                    result[property.Key] = Expand(property.Value);

                return result;
            }

            return value?.DeepClone();
        }
    }

}
