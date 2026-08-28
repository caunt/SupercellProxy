namespace SupercellProxy.Playground.Commands;

internal static partial class CommandRegistry
{
    private static readonly Dictionary<int, CommandRegistryEntry> TypedEntries = new()
    {
        [210] = new CommandRegistryEntry(
            Type: typeof(ServerCommand210),
            IsServerCommand: true,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, _) => ServerCommand210.Decode(stream, environment)
        ),
        [274] = new CommandRegistryEntry(
            Type: typeof(ServerCommand274),
            IsServerCommand: true,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, dataResolver) =>
                ServerCommand274.Decode(stream, environment, dataResolver)
        ),
        [355] = new CommandRegistryEntry(
            Type: typeof(ServerCommand355),
            IsServerCommand: true,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, _) => ServerCommand355.Decode(stream, environment)
        ),
        [672] = new CommandRegistryEntry(
            Type: typeof(CollectAllLettersCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, _) =>
                CollectAllLettersCommand.Decode(stream, environment)
        ),
        [35] = new CommandRegistryEntry(
            Type: typeof(CompleteTutorialCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, _) =>
                CompleteTutorialCommand.Decode(stream, environment)
        ),
        [3] = new CommandRegistryEntry(
            Type: typeof(MoveGameObjectByOffsetCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, _) =>
                MoveGameObjectByOffsetCommand.Decode(stream, environment)
        ),
        [124] = new CommandRegistryEntry(
            Type: typeof(MoveGameObjectCommand),
            IsServerCommand: false,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, _) =>
                MoveGameObjectCommand.Decode(stream, environment)
        ),
        [544] = new CommandRegistryEntry(
            Type: typeof(StartHarvestFieldCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, _) =>
                StartHarvestFieldCommand.Decode(stream, environment)
        ),
        [506] = new CommandRegistryEntry(
            Type: typeof(HarvestFieldCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, _) =>
                HarvestFieldCommand.Decode(stream, environment)
        ),
        [657] = new CommandRegistryEntry(
            Type: typeof(HarvestFieldGainCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, _) =>
                HarvestFieldGainCommand.Decode(stream, environment)
        ),
        [247] = new CommandRegistryEntry(
            Type: typeof(Command247),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, _) => Command247.Decode(stream, environment)
        ),
        [321] = new CommandRegistryEntry(
            Type: typeof(Command321),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, dataResolver) =>
                Command321.Decode(stream, environment, dataResolver)
        ),
        [599] = new CommandRegistryEntry(
            Type: typeof(Command599),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, _) => Command599.Decode(stream, environment)
        ),
        [694] = new CommandRegistryEntry(
            Type: typeof(PostmanStateCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, _) =>
                PostmanStateCommand.Decode(stream, environment)
        ),
        [654] = new CommandRegistryEntry(
            Type: typeof(DecorationEventTutorialCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, _) =>
                DecorationEventTutorialCommand.Decode(stream, environment)
        ),
        [34] = new CommandRegistryEntry(
            Type: typeof(NewEventBoardEventSeenCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, _) =>
                NewEventBoardEventSeenCommand.Decode(stream, environment)
        ),
        [649] = new CommandRegistryEntry(
            Type: typeof(RoadsideReceiptCommand),
            IsServerCommand: false,
            BaseFirst: true,
            FieldSchemas: null,
            Factory: static (stream, environment, _) =>
                RoadsideReceiptCommand.Decode(stream, environment)
        ),
        [375] = new CommandRegistryEntry(
            Type: typeof(RoadsideSaleServerCommand),
            IsServerCommand: true,
            BaseFirst: false,
            FieldSchemas: null,
            Factory: static (stream, environment, _) =>
                RoadsideSaleServerCommand.Decode(stream, environment)
        ),
    };
}
