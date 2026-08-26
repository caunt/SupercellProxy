using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

public static partial class CommandRegistry
{
    private static readonly Dictionary<int, CommandRegistryEntry> TypedEntries = new()
    {
        [210] = new CommandRegistryEntry(
            Type: typeof(ServerCommand210),
            IsServerCommand: true,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (
                MessageStream stream,
                CommandEnvironment environment,
                ICommandDataResolver? _
            ) => ServerCommand210.Decode(stream, environment)
        ),
        [274] = new CommandRegistryEntry(
            Type: typeof(ServerCommand274),
            IsServerCommand: true,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (
                MessageStream stream,
                CommandEnvironment environment,
                ICommandDataResolver? dataResolver
            ) => ServerCommand274.Decode(stream, environment, dataResolver)
        ),
        [355] = new CommandRegistryEntry(
            Type: typeof(ServerCommand355),
            IsServerCommand: true,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (
                MessageStream stream,
                CommandEnvironment environment,
                ICommandDataResolver? _
            ) => ServerCommand355.Decode(stream, environment)
        ),
        [672] = new CommandRegistryEntry(
            Type: typeof(CollectAllLettersCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (
                MessageStream stream,
                CommandEnvironment environment,
                ICommandDataResolver? _
            ) => CollectAllLettersCommand.Decode(stream, environment)
        ),
        [35] = new CommandRegistryEntry(
            Type: typeof(CompleteTutorialCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (
                MessageStream stream,
                CommandEnvironment environment,
                ICommandDataResolver? _
            ) => CompleteTutorialCommand.Decode(stream, environment)
        ),
        [3] = new CommandRegistryEntry(
            Type: typeof(MoveGameObjectByOffsetCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (
                MessageStream stream,
                CommandEnvironment environment,
                ICommandDataResolver? _
            ) => MoveGameObjectByOffsetCommand.Decode(stream, environment)
        ),
        [124] = new CommandRegistryEntry(
            Type: typeof(MoveGameObjectCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (
                MessageStream stream,
                CommandEnvironment environment,
                ICommandDataResolver? _
            ) => MoveGameObjectCommand.Decode(stream, environment)
        ),
        [544] = new CommandRegistryEntry(
            Type: typeof(StartHarvestFieldCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (
                MessageStream stream,
                CommandEnvironment environment,
                ICommandDataResolver? _
            ) => StartHarvestFieldCommand.Decode(stream, environment)
        ),
        [506] = new CommandRegistryEntry(
            Type: typeof(HarvestFieldCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (
                MessageStream stream,
                CommandEnvironment environment,
                ICommandDataResolver? _
            ) => HarvestFieldCommand.Decode(stream, environment)
        ),
        [657] = new CommandRegistryEntry(
            Type: typeof(HarvestFieldGainCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (
                MessageStream stream,
                CommandEnvironment environment,
                ICommandDataResolver? _
            ) => HarvestFieldGainCommand.Decode(stream, environment)
        ),
        [247] = new CommandRegistryEntry(
            Type: typeof(Command247),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (
                MessageStream stream,
                CommandEnvironment environment,
                ICommandDataResolver? _
            ) => Command247.Decode(stream, environment)
        ),
        [321] = new CommandRegistryEntry(
            Type: typeof(Command321),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (
                MessageStream stream,
                CommandEnvironment environment,
                ICommandDataResolver? dataResolver
            ) => Command321.Decode(stream, environment, dataResolver)
        ),
        [599] = new CommandRegistryEntry(
            Type: typeof(Command599),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (
                MessageStream stream,
                CommandEnvironment environment,
                ICommandDataResolver? _
            ) => Command599.Decode(stream, environment)
        ),
        [694] = new CommandRegistryEntry(
            Type: typeof(PostmanStateCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (
                MessageStream stream,
                CommandEnvironment environment,
                ICommandDataResolver? _
            ) => PostmanStateCommand.Decode(stream, environment)
        ),
    };
}
