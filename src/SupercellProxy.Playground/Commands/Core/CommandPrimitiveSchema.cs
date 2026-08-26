namespace SupercellProxy.Playground.Commands;

internal sealed class CommandPrimitiveSchema(
    int[] commandTypes,
    CommandFieldType[] fieldTypes,
    bool isServerCommand = false,
    bool baseFirst = true
)
{
    public int[] CommandTypes { get; } = commandTypes;
    public CommandFieldType[] FieldTypes { get; } = fieldTypes;
    public bool IsServerCommand { get; } = isServerCommand;
    public bool BaseFirst { get; } = baseFirst;
}
