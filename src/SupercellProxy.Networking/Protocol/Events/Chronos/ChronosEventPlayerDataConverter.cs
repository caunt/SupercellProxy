using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>Preserves the native alternating event-id and document-string array.</summary>
public sealed class ChronosEventPlayerDataConverter : JsonConverter<ChronosEventPlayerData[]>
{
    /// <inheritdoc />
    public override ChronosEventPlayerData[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException(message: "Expected the saved event-data array.");

        List<ChronosEventPlayerData> entries = [];

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray) return [.. entries];

            if (reader.TokenType != JsonTokenType.Number || !reader.TryGetInt32(out int eventId))
                throw new JsonException(message: "Expected a saved event identifier.");

            if (!reader.Read() || reader.TokenType != JsonTokenType.String)
                throw new JsonException(message: "Expected the saved event document string.");

            entries.Add(new(eventId, reader.GetString() ?? throw new JsonException(message: "The saved event document is null.")));
        }

        throw new JsonException(message: "The saved event-data array is incomplete.");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ChronosEventPlayerData[] value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);
        writer.WriteStartArray();

        foreach (ChronosEventPlayerData entry in value)
        {
            writer.WriteNumberValue(entry.EventId);
            writer.WriteStringValue(entry.Document);
        }

        writer.WriteEndArray();
    }
}
