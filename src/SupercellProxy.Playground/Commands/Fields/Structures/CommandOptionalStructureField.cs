using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">CommandOptionalStructureField</c>.
/// </summary>
internal sealed record CommandOptionalStructureField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandOptionalStructureField"/> instance.
    /// </summary>
    public CommandOptionalStructureField(ReadOnlyMemory<CommandField>? fields)
    {
        Fields = fields?.ToArray();
    }

    /// <summary>
    /// Gets the <c language="csharp">Fields</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandField>? Fields { get; }
    internal override CommandFieldType FieldType => CommandFieldType.OptionalStructure;

    internal override void Encode(MessageStream stream)
    {
        stream.WriteBoolean(Fields is not null);

        if (Fields is null)
            return;

        foreach (var field in Fields.Value.Span)
            field.Encode(stream);
    }
}
