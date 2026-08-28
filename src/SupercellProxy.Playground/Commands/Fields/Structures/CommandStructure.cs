namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">CommandStructure</c>.
/// </summary>
internal sealed record CommandStructure
{
    /// <summary>
    /// Initializes a new <see cref="CommandStructure"/> instance.
    /// </summary>
    public CommandStructure(ReadOnlyMemory<CommandField> fields)
    {
        Fields = fields.ToArray();
    }

    /// <summary>
    /// Gets the <c language="csharp">Fields</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandField> Fields { get; }
}
