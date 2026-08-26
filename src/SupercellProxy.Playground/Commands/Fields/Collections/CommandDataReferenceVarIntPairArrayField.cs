using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>CommandDataReferenceVarIntPairArrayField</c>.
/// </summary>
public sealed record CommandDataReferenceVarIntPairArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandDataReferenceVarIntPairArrayField"/> instance.
    /// </summary>
    public CommandDataReferenceVarIntPairArrayField(
        ReadOnlyMemory<CommandDataReferenceVarIntPair> values
    ) => Values = values.ToArray();

    /// <summary>
    /// Gets the <c>Values</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair> Values { get; }
    internal override CommandFieldType FieldType => CommandFieldType.DataReferenceVarIntPairArray;

    internal static CommandDataReferenceVarIntPairArrayField Decode(MessageStream stream)
    {
        var count = CommandVarIntPairArrayField.ReadCount(stream);
        var values = new CommandDataReferenceVarIntPair[count];

        for (var i = 0; i < values.Length; i++)
            values[i] = new CommandDataReferenceVarIntPair(
                stream.ReadVarInt(),
                stream.ReadVarInt()
            );

        return new CommandDataReferenceVarIntPairArrayField(values);
    }

    internal override void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Values.Length);

        foreach (var value in Values.Span)
        {
            stream.WriteVarInt(value.GlobalId);
            stream.WriteVarInt(value.Value);
        }
    }
}
