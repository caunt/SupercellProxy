using System.Buffers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>Serializes typed table-cell literals and preserves unknown structured changes inside the codec.</summary>
public sealed class TableCellValueConverter : JsonConverter<TableCellValue>
{
    /// <inheritdoc />
    public override TableCellValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
            return new TableCellValue(TableCellValueKind.Null, text: null);

        if (reader.TokenType is JsonTokenType.String)
            return new TableCellValue(TableCellValueKind.Text, reader.GetString());

        if (reader.TokenType is JsonTokenType.True or JsonTokenType.False)
            return new TableCellValue(TableCellValueKind.Boolean, reader.GetBoolean() ? "true" : "false");

        if (reader.TokenType is JsonTokenType.Number)
        {
            string text = Encoding.UTF8.GetString(reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan);

            return new TableCellValue(TableCellValueKind.Number, text);
        }

        EncodedDocumentValue? structured = JsonSerializer.Deserialize<EncodedDocumentValue>(ref reader, options);

        return new TableCellValue(TableCellValueKind.Structured, text: null, structured);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TableCellValue value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        switch (value.Kind)
        {
            case TableCellValueKind.Text:
                {
                    writer.WriteStringValue(value.Text);

                    break;
                }
            case TableCellValueKind.Number:
            case TableCellValueKind.Boolean:
                {
                    writer.WriteRawValue(value.Text ?? throw new JsonException(message: "The scalar literal has no text."));

                    break;
                }
            case TableCellValueKind.Structured:
                {
                    JsonSerializer.Serialize(writer, value.Structured, options);

                    break;
                }
            case TableCellValueKind.Missing:
            case TableCellValueKind.Null:
                {
                    writer.WriteNullValue();

                    break;
                }
            default:
                throw new JsonException(message: "Unknown table-cell literal kind.");
        }
    }
}
