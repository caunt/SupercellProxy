using System.Globalization;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">CommandVarIntPairArrayField</c>.
/// </summary>
internal sealed record CommandVarIntPairArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandVarIntPairArrayField"/> instance.
    /// </summary>
    public CommandVarIntPairArrayField(ReadOnlyMemory<CommandVarIntPair> values) =>
        Values = values.ToArray();

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandVarIntPair> Values { get; }
    internal override CommandFieldType FieldType => CommandFieldType.VarIntPairArray;

    internal static CommandVarIntPairArrayField Decode(MessageStream stream)
    {
        var count = ReadCount(stream);
        var values = new CommandVarIntPair[count];

        for (var i = 0; i < values.Length; i++)
            values[i] = new CommandVarIntPair(stream.ReadVarInt(), stream.ReadVarInt());

        return new CommandVarIntPairArrayField(values);
    }

    internal override void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Values.Length);

        foreach (var value in Values.Span)
        {
            stream.WriteVarInt(value.Value0);
            stream.WriteVarInt(value.Value1);
        }
    }

    internal static int ReadCount(MessageStream stream)
    {
        var count = stream.ReadVarInt();

        if (count < 0 || count > (stream.Length - stream.Position) / 2)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Invalid command pair array count: {count}."
                )
            );

        return count;
    }
}
