using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Commands;

/// <summary>Writes a command contract as its name; contracts come from command records and are never read from JSON.</summary>
internal sealed class CommandContractJsonConverter : JsonConverter<CommandContract>
{
    /// <inheritdoc />
    public override CommandContract Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new JsonException(message: "Command contracts are identified by their command records and cannot be read from JSON.");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, CommandContract value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        writer.WriteStringValue(value.Name);
    }
}
