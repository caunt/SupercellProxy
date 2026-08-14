using System.Text.Json.Nodes;

namespace SupercellProxy.Keys;

internal static class SvelteDataDecoder
{
    public static JsonNode? Decode(JsonArray values)
    {
        if (values.Count == 0)
            return null;

        var cache = new JsonNode?[values.Count];
        var cached = new bool[values.Count];
        var active = new bool[values.Count];

        return Resolve(0);

        JsonNode? Resolve(int index)
        {
            if (index < 0 || index >= values.Count)
                return null;

            if (cached[index])
                return cache[index]?.DeepClone();

            if (active[index])
                return null;

            active[index] = true;
            var result = Expand(values[index]);
            active[index] = false;

            cache[index] = result;
            cached[index] = true;

            return result?.DeepClone();
        }

        JsonNode? Expand(JsonNode? value)
        {
            if (value is JsonValue scalar && scalar.TryGetValue<int>(out var reference))
                return Resolve(reference);

            if (value is JsonArray array)
                return new JsonArray(array.Select(Expand).ToArray());

            if (value is JsonObject source)
            {
                var result = new JsonObject();

                foreach (var property in source)
                    result[property.Key] = Expand(property.Value);

                return result;
            }

            return value?.DeepClone();
        }
    }
}
