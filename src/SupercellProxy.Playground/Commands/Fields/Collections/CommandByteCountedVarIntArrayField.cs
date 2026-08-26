using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>CommandByteCountedVarIntArrayField</c>.
/// </summary>
public sealed record CommandByteCountedVarIntArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandByteCountedVarIntArrayField"/> instance.
    /// </summary>
    public CommandByteCountedVarIntArrayField(ReadOnlyMemory<int> values)
    {
        if (values.Length > sbyte.MaxValue)
            throw new InvalidDataException(
                $"A byte-counted command array cannot contain more than {sbyte.MaxValue} values."
            );

        Values = values.ToArray();
    }

    /// <summary>
    /// Gets the <c>Values</c> value.
    /// </summary>
    public ReadOnlyMemory<int> Values { get; }
    internal override CommandFieldType FieldType => CommandFieldType.ByteCountedVarIntArray;

    internal static CommandByteCountedVarIntArrayField Decode(MessageStream stream)
    {
        var count = unchecked(sbyte.CreateTruncating(stream.ReadByte()));
        return new CommandByteCountedVarIntArrayField(
            CommandVarIntArrayField.DecodeValues(count, stream)
        );
    }

    internal override void Encode(MessageStream stream)
    {
        stream.WriteByte(byte.CreateTruncating(Values.Length));

        foreach (var value in Values.Span)
            stream.WriteVarInt(value);
    }
}
