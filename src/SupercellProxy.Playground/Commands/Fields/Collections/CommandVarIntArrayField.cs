using System.Globalization;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>CommandVarIntArrayField</c>.
/// </summary>
public sealed record CommandVarIntArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandVarIntArrayField"/> instance.
    /// </summary>
    public CommandVarIntArrayField(ReadOnlyMemory<int> values) => Values = values.ToArray();

    /// <summary>
    /// Gets the <c>Values</c> value.
    /// </summary>
    public ReadOnlyMemory<int> Values { get; }
    internal override CommandFieldType FieldType => CommandFieldType.VarIntArray;

    internal static CommandVarIntArrayField Decode(MessageStream stream) =>
        new(DecodeValues(stream.ReadVarInt(), stream));

    internal override void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Values.Length);

        foreach (var value in Values.Span)
            stream.WriteVarInt(value);
    }

    internal static int[] DecodeValues(int count, MessageStream stream)
    {
        if (count < 0 || count > stream.Length - stream.Position)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Invalid command array count: {count}."
                )
            );

        var values = new int[count];

        for (var i = 0; i < values.Length; i++)
            values[i] = stream.ReadVarInt();

        return values;
    }
}
