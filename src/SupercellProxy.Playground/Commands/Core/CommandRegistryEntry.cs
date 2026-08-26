using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

internal sealed record CommandRegistryEntry(
    Type Type,
    bool IsServerCommand,
    bool BaseFirst,
    CommandFieldSchema[]? FieldSchemas,
    Func<MessageStream, CommandEnvironment, ICommandDataResolver?, Command> Factory
);
