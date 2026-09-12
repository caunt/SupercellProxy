using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Timing;

/// <summary>Serializes the concrete counter and countdown timer variants.</summary>
public sealed class TimerValueConverter : JsonConverter<TimerValue>
{
    /// <inheritdoc />
    public override TimerValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Null => new TimerValue(TimerValueKind.Null, counter: 0, countdown: default),
            JsonTokenType.Number => TimerValue.FromCounter(reader.GetInt32()),
            JsonTokenType.StartObject => TimerValue.FromCountdown(JsonSerializer.Deserialize<TimerSnapshot>(ref reader, options)),
            JsonTokenType.None => throw new JsonException(message: "Unsupported JSON literal for this contract."),
            JsonTokenType.EndObject => throw new JsonException(message: "Unsupported JSON literal for this contract."),
            JsonTokenType.StartArray => throw new JsonException(message: "Unsupported JSON literal for this contract."),
            JsonTokenType.EndArray => throw new JsonException(message: "Unsupported JSON literal for this contract."),
            JsonTokenType.PropertyName => throw new JsonException(message: "Unsupported JSON literal for this contract."),
            JsonTokenType.Comment => throw new JsonException(message: "Unsupported JSON literal for this contract."),
            JsonTokenType.String => throw new JsonException(message: "Unsupported JSON literal for this contract."),
            JsonTokenType.True => throw new JsonException(message: "Unsupported JSON literal for this contract."),
            JsonTokenType.False => throw new JsonException(message: "Unsupported JSON literal for this contract."),
            _ => throw new JsonException(message: "Expected a timer counter or countdown object."),
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TimerValue value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        switch (value.Kind)
        {
            case TimerValueKind.Counter:
                {
                    writer.WriteNumberValue(value.Counter);

                    break;
                }
            case TimerValueKind.Countdown:
                {
                    JsonSerializer.Serialize(writer, value.Countdown, options);

                    break;
                }
            case TimerValueKind.Missing:
            case TimerValueKind.Null:
                {
                    writer.WriteNullValue();

                    break;
                }
            default:
                throw new JsonException(message: "Unknown timer representation.");
        }
    }
}
