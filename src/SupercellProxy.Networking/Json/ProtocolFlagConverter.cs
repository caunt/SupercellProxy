using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Json;

/// <summary>Serializes boolean and integer flag literals without losing their original representation.</summary>
public sealed class ProtocolFlagConverter : JsonConverter<ProtocolFlag>
{
    /// <inheritdoc />
    public override ProtocolFlag Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Null => new ProtocolFlag(ProtocolFlagKind.Null, integer: 0),
            JsonTokenType.True => new ProtocolFlag(ProtocolFlagKind.Boolean, integer: 1),
            JsonTokenType.False => new ProtocolFlag(ProtocolFlagKind.Boolean, integer: 0),
            JsonTokenType.Number => new ProtocolFlag(ProtocolFlagKind.Integer, reader.GetInt32()),
            JsonTokenType.None => throw new JsonException(message: "Unsupported JSON literal for this contract."),
            JsonTokenType.StartObject => throw new JsonException(message: "Unsupported JSON literal for this contract."),
            JsonTokenType.EndObject => throw new JsonException(message: "Unsupported JSON literal for this contract."),
            JsonTokenType.StartArray => throw new JsonException(message: "Unsupported JSON literal for this contract."),
            JsonTokenType.EndArray => throw new JsonException(message: "Unsupported JSON literal for this contract."),
            JsonTokenType.PropertyName => throw new JsonException(message: "Unsupported JSON literal for this contract."),
            JsonTokenType.Comment => throw new JsonException(message: "Unsupported JSON literal for this contract."),
            JsonTokenType.String => throw new JsonException(message: "Unsupported JSON literal for this contract."),
            _ => throw new JsonException(message: "Expected a boolean or integer protocol flag."),
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ProtocolFlag value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        switch (value.Kind)
        {
            case ProtocolFlagKind.Boolean:
                {
                    writer.WriteBooleanValue(value.Value);

                    break;
                }
            case ProtocolFlagKind.Integer:
                {
                    writer.WriteNumberValue(value.Integer);

                    break;
                }
            case ProtocolFlagKind.Missing:
            case ProtocolFlagKind.Null:
                {
                    writer.WriteNullValue();

                    break;
                }
            default:
                throw new JsonException(message: "Unknown protocol flag representation.");
        }
    }
}
