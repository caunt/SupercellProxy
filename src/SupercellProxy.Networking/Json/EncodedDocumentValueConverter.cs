using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Json;

/// <summary>Copies unknown values losslessly at the JSON codec boundary.</summary>
public sealed class EncodedDocumentValueConverter : JsonConverter<EncodedDocumentValue>
{
    /// <inheritdoc />
    public override EncodedDocumentValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);

        return new EncodedDocumentValue(Encoding.UTF8.GetBytes(document.RootElement.GetRawText()));
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, EncodedDocumentValue value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);
        writer.WriteRawValue(value.Encode());
    }
}
