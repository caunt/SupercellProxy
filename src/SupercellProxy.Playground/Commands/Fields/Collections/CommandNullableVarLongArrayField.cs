using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">CommandNullableVarLongArrayField</c>.
/// </summary>
internal sealed record CommandNullableVarLongArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandNullableVarLongArrayField"/> instance.
    /// </summary>
    public CommandNullableVarLongArrayField(ReadOnlyMemory<long>? values) =>
        Values = values?.ToArray();

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<long>? Values { get; }
    internal override CommandFieldType FieldType => CommandFieldType.NullableVarLongArray;

    internal static CommandNullableVarLongArrayField Decode(MessageStream stream)
    {
        var count = stream.ReadVarInt();
        return new CommandNullableVarLongArrayField(
            count is -1 ? null : CommandVarLongArrayField.DecodeValues(count, stream)
        );
    }

    internal override void Encode(MessageStream stream)
    {
        if (Values is null)
        {
            stream.WriteVarInt(-1);
            return;
        }

        stream.WriteVarInt(Values.Value.Length);

        foreach (var value in Values.Value.Span)
            stream.WriteVarLong(value);
    }
}
