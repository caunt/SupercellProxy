using System.Globalization;

using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;

/// <summary>
/// Represents <c language="csharp">CommandVarIntPairArrayField</c>.
/// </summary>
public sealed record CommandVarIntPairArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandVarIntPairArrayField"/> instance.
    /// </summary>
    public CommandVarIntPairArrayField(ReadOnlyMemory<CommandVarIntPair> values)
    {
        Values = values.ToArray();
    }

    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.VarIntPairArray;

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandVarIntPair> Values { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CommandVarIntPairArrayField Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int count = ReadCount(stream);
        CommandVarIntPair[] values = new CommandVarIntPair[count];

        for (int index = 0; index < values.Length; index++)
            values[index] = new CommandVarIntPair(stream.ReadVarInt(), stream.ReadVarInt());

        return new CommandVarIntPairArrayField(values);
    }

    /// <summary>
    /// Provides the Read Count value or operation.
    /// </summary>
    public static int ReadCount(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int count = stream.ReadVarInt();

        return count < 0 || count > (stream.Length - stream.Position) / 2
            ? throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Invalid command pair array count: {count}."))
            : count;
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Values.Length);

        foreach (CommandVarIntPair value in Values.Span)
        {
            stream.WriteVarInt(value.Value0);
            stream.WriteVarInt(value.Value1);
        }
    }
}
