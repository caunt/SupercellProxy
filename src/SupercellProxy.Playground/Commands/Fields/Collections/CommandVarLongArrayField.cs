using System.Globalization;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">CommandVarLongArrayField</c>.
/// </summary>
internal sealed record CommandVarLongArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandVarLongArrayField"/> instance.
    /// </summary>
    public CommandVarLongArrayField(ReadOnlyMemory<long> values) => Values = values.ToArray();

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<long> Values { get; }
    internal override CommandFieldType FieldType => CommandFieldType.VarLongArray;

    internal static CommandVarLongArrayField Decode(MessageStream stream) =>
        new(DecodeValues(stream.ReadVarInt(), stream));

    internal override void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Values.Length);

        foreach (var value in Values.Span)
            stream.WriteVarLong(value);
    }

    internal static long[] DecodeValues(int count, MessageStream stream)
    {
        if (count < 0 || count > stream.Length - stream.Position)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Invalid command array count: {count}."
                )
            );

        var values = new long[count];

        for (var i = 0; i < values.Length; i++)
            values[i] = stream.ReadVarLong();

        return values;
    }
}
