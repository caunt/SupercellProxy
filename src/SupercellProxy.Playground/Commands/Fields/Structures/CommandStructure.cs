namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>CommandStructure</c>.
/// </summary>
public sealed record CommandStructure
{
    /// <summary>
    /// Initializes a new <see cref="CommandStructure"/> instance.
    /// </summary>
    public CommandStructure(ReadOnlyMemory<CommandField> fields)
    {
        Fields = fields.ToArray();
    }

    /// <summary>
    /// Gets the <c>Fields</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandField> Fields { get; }
}
