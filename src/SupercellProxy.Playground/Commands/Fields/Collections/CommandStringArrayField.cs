using System.Globalization;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">CommandStringArrayField</c>.
/// </summary>
internal sealed record CommandStringArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandStringArrayField"/> instance.
    /// </summary>
    public CommandStringArrayField(ReadOnlyMemory<string> values) => Values = values.ToArray();

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<string> Values { get; }
    internal override CommandFieldType FieldType => CommandFieldType.StringArray;

    internal static CommandStringArrayField Decode(MessageStream stream)
    {
        var count = stream.ReadVarInt();

        if (count < 0 || count > stream.Length - stream.Position)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Invalid command string array count: {count}."
                )
            );

        var values = new string[count];

        for (var i = 0; i < values.Length; i++)
            values[i] = stream.ReadString();

        return new CommandStringArrayField(values);
    }

    internal override void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Values.Length);

        foreach (var value in Values.Span)
            stream.WriteString(value);
    }
}
