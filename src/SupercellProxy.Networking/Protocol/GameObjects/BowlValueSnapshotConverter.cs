using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects;

/// <summary>Reads and writes the two observed saved-bowl formats.</summary>
public sealed class BowlValueSnapshotConverter : JsonConverter<BowlValueSnapshot>
{
    private const string InvalidBowlMessage = "A saved bowl must be a boolean or integer.";

    /// <inheritdoc />
    public override BowlValueSnapshot Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True => new BowlValueSnapshot(integerValue: 1, booleanRepresentation: true),
            JsonTokenType.False => new BowlValueSnapshot(integerValue: 0, booleanRepresentation: true),
            JsonTokenType.Number => new BowlValueSnapshot(reader.GetInt32(), booleanRepresentation: false),
            JsonTokenType.None => throw new JsonException(InvalidBowlMessage),
            JsonTokenType.StartObject => throw new JsonException(InvalidBowlMessage),
            JsonTokenType.EndObject => throw new JsonException(InvalidBowlMessage),
            JsonTokenType.StartArray => throw new JsonException(InvalidBowlMessage),
            JsonTokenType.EndArray => throw new JsonException(InvalidBowlMessage),
            JsonTokenType.PropertyName => throw new JsonException(InvalidBowlMessage),
            JsonTokenType.Comment => throw new JsonException(InvalidBowlMessage),
            JsonTokenType.String => throw new JsonException(InvalidBowlMessage),
            JsonTokenType.Null => throw new JsonException(InvalidBowlMessage),
            _ => throw new JsonException(InvalidBowlMessage),
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, BowlValueSnapshot value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        if (value.BooleanRepresentation)
            writer.WriteBooleanValue(value.IntegerValue is not 0);
        else
            writer.WriteNumberValue(value.IntegerValue);
    }
}
