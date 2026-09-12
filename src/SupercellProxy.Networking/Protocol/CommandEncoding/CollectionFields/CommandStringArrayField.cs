using System.Globalization;

using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;

/// <summary>
/// Represents <c language="csharp">CommandStringArrayField</c>.
/// </summary>
public sealed record CommandStringArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandStringArrayField"/> instance.
    /// </summary>
    public CommandStringArrayField(ReadOnlyMemory<string> values)
    {
        Values = values.ToArray();
    }

    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.StringArray;

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<string> Values { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CommandStringArrayField Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int count = stream.ReadVariableInt();

        if (count < 0 || count > stream.Length - stream.Position)
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Invalid command string array count: {count}."));

        string[] values = new string[count];

        for (int index = 0; index < values.Length; index++)
            values[index] = stream.ReadString();

        return new CommandStringArrayField(values);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteVariableInt(Values.Length);

        foreach (string value in Values.Span)
            stream.WriteString(value);
    }
}
