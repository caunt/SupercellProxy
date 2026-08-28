using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">CommandStructureArrayField</c>.
/// </summary>
internal sealed record CommandStructureArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandStructureArrayField"/> instance.
    /// </summary>
    public CommandStructureArrayField(ReadOnlyMemory<CommandStructure>? values)
    {
        Values = values?.ToArray();
    }

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandStructure>? Values { get; }
    internal override CommandFieldType FieldType => CommandFieldType.StructureArray;

    internal override void Encode(MessageStream stream)
    {
        if (Values is null)
        {
            stream.WriteVarInt(-1);
            return;
        }

        stream.WriteVarInt(Values.Value.Length);

        foreach (var value in Values.Value.Span)
        {
            foreach (var field in value.Fields.Span)
                field.Encode(stream);
        }
    }
}
