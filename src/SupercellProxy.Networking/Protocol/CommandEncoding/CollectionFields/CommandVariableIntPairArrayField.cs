using System.Globalization;

using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;

/// <summary>
/// Represents <c language="csharp">CommandVarIntPairArrayField</c>.
/// </summary>
public sealed record CommandVariableIntPairArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandVariableIntPairArrayField"/> instance.
    /// </summary>
    public CommandVariableIntPairArrayField(ReadOnlyMemory<CommandVariableIntPair> values)
    {
        Values = values.ToArray();
    }

    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.VariableIntPairArray;

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandVariableIntPair> Values { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CommandVariableIntPairArrayField Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int count = ReadCount(stream);
        CommandVariableIntPair[] values = new CommandVariableIntPair[count];

        for (int index = 0; index < values.Length; index++)
            values[index] = new CommandVariableIntPair(stream.ReadVariableInt(), stream.ReadVariableInt());

        return new CommandVariableIntPairArrayField(values);
    }

    /// <summary>
    /// Provides the Read Count value or operation.
    /// </summary>
    public static int ReadCount(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int count = stream.ReadVariableInt();

        return count < 0 || count > (stream.Length - stream.Position) / 2
            ? throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Invalid command pair array count: {count}."))
            : count;
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteVariableInt(Values.Length);

        foreach (CommandVariableIntPair value in Values.Span)
        {
            stream.WriteVariableInt(value.Value0);
            stream.WriteVariableInt(value.Value1);
        }
    }
}
