using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Sessions;

internal sealed class ClientSessionTokenConverter : JsonConverter<LoginSessionToken>
{
    public override LoginSessionToken Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.String)
            throw new JsonException(message: "A client session token must be a JWT string.");

        string value = reader.GetString()
            ?? throw new JsonException(message: "A client session token must not be null.");

        try
        {
            return LoginSessionToken.Decode(value);
        }
        catch (InvalidDataException exception)
        {
            throw new JsonException(message: "A client session token must be a valid JWT string.", exception);
        }
    }

    public override void Write(Utf8JsonWriter writer, LoginSessionToken value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteStringValue(value.Value);
    }
}
