using System.Text.Json;
using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol;

namespace SupercellProxy.Networking.Sessions;

internal sealed class ClientSessionAccountIdentifierConverter : JsonConverter<LongIdentifier>
{
    public override LongIdentifier Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.String)
            throw new JsonException(message: "A client session account ID must be a tag string.");

        string? value = reader.GetString();

        bool validAccountIdentifier = LongIdentifier.TryParse(value, out LongIdentifier accountIdentifier)
            && accountIdentifier != LongIdentifier.Empty;

        return validAccountIdentifier
            ? accountIdentifier
            : throw new JsonException(message: "A client session account ID must be a valid nonempty tag string.");
    }

    public override void Write(Utf8JsonWriter writer, LongIdentifier value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteStringValue(value.ToFormattedString());
    }
}
