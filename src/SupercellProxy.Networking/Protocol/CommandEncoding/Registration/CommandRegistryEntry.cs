using SupercellProxy.Networking.Protocol.CommandEncoding.FieldSchemas;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

/// <summary>
/// Defines the Command Registry Entry contract.
/// </summary>
/// <summary>
/// Defines the Type contract.
/// </summary>
/// <summary>
/// Defines the Is Server Command contract.
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
    bool IsServerCommand,
    bool BaseFirst,
    CommandFieldSchema[]? FieldSchemas,
    Func<MessageStream, CommandEnvironment, ICommandDataResolver?, Command> Factory
);
