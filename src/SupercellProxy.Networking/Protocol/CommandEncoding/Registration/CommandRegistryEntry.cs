using SupercellProxy.Networking.Protocol.CommandEncoding.FieldSchemas;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

/// <summary>
/// Defines the Command Registry Entry contract.
/// </summary>
/// <summary>
/// Defines the Type contract.
/// </summary>
/// <summary>
/// Defines the Direction contract.
/// </summary>
/// <summary>
/// Defines the Base First contract.
/// </summary>
/// <summary>
/// Defines the Field Schemas contract.
/// </summary>
/// <summary>
/// Defines the Factory contract.
/// </summary>
public sealed record CommandRegistryEntry(
    Type Type,
    MessageDirection Direction,
    bool BaseFirst,
    CommandFieldSchema[]? FieldSchemas,
    Func<MessageStream, CommandEnvironment, ICommandDataResolver?, Command> Factory
);
