using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

/// <summary>
/// Defines the Command Primitive Schema contract.
/// </summary>
public sealed class CommandPrimitiveSchema(int[] commandTypes, CommandFieldType[] fieldTypes, MessageDirection direction, bool baseFirst = true)
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
    /// Gets the Direction value.
    /// </summary>
    public MessageDirection Direction { get; } = direction;

    /// <summary>
    /// Gets the Base First value.
    /// </summary>
    public bool BaseFirst { get; } = baseFirst;
}
