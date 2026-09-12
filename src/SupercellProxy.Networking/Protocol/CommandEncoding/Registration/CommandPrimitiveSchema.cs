namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

/// <summary>
/// Defines the Command Primitive Schema contract.
/// </summary>
public sealed class CommandPrimitiveSchema(int[] commandTypes, CommandFieldType[] fieldTypes, bool isServerCommand = false, bool baseFirst = true)
{
    /// <summary>
    /// Gets the Command Types value.
    /// </summary>
    public int[] CommandTypes { get; } = commandTypes;

    /// <summary>
    /// Gets the Field Types value.
    /// </summary>
    public CommandFieldType[] FieldTypes { get; } = fieldTypes;

    /// <summary>
    /// Gets the Is Server Command value.
    /// </summary>
    public bool IsServerCommand { get; } = isServerCommand;

    /// <summary>
    /// Gets the Base First value.
    /// </summary>
    public bool BaseFirst { get; } = baseFirst;
}
