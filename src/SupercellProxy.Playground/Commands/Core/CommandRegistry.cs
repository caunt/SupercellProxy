using System.Globalization;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Maps native command IDs to typed wire models.
/// Rejects entries that do not match the registered schema.
/// </summary>
internal static partial class CommandRegistry
{
    internal const int HomeLoadedCommandType = 530;

    private static readonly Lazy<Dictionary<int, CommandRegistryEntry>> LazyEntries = new(
        CreateEntries
    );
    private static readonly HashSet<int> NonProductionCommandTypes = [7, 84, 85];
    private static Dictionary<int, CommandRegistryEntry> Entries => LazyEntries.Value;

    private static Dictionary<int, CommandRegistryEntry> CreateEntries()
    {
        var entries = new Dictionary<int, CommandRegistryEntry>(TypedEntries);
        AddVariableCommandEntries(entries);
        AddPrimitiveSchemas(entries, ProductionPrimitiveSchemas);
        AddPrimitiveSchemas(entries, LegacyPrimitiveSchemasA);
        AddPrimitiveSchemas(entries, LegacyPrimitiveSchemasB);
        AddPrimitiveSchemas(entries, ExtendedPrimitiveSchemas);
        AddStructuredCommands(entries);
        return entries;
    }

    private static void AddVariableCommandEntries(Dictionary<int, CommandRegistryEntry> entries)
    {
        int[] commandTypes = CommandWithNoFields.CommandTypes;
        foreach (int type in commandTypes)
        {
            int commandType = type;
            entries.Add(
                commandType,
                new CommandRegistryEntry(
                    Type: typeof(CommandWithNoFields),
                    IsServerCommand: false,
                    BaseFirst: true,
                    FieldSchemas: null,
                    Factory: (stream, environment, _) =>
                        CommandWithNoFields.Decode(commandType, stream, environment)
                )
            );
        }
        int[] commandTypes2 = MapGameTaskCommand.CommandTypes;
        foreach (int type2 in commandTypes2)
        {
            int commandType2 = type2;
            entries.Add(
                commandType2,
                new CommandRegistryEntry(
                    Type: typeof(MapGameTaskCommand),
                    IsServerCommand: false,
                    BaseFirst: true,
                    FieldSchemas: null,
                    Factory: (stream, environment, dataResolver) =>
                        MapGameTaskCommand.Decode(commandType2, stream, environment, dataResolver)
                )
            );
        }
    }

    private static void AddPrimitiveSchemas(
        Dictionary<int, CommandRegistryEntry> entries,
        IEnumerable<CommandPrimitiveSchema> schemas
    )
    {
        foreach (var schema in schemas)
        {
            AddFieldCommands(
                entries,
                schema.CommandTypes,
                schema.FieldTypes,
                schema.IsServerCommand,
                schema.BaseFirst
            );
        }
    }

    private static void AddFieldCommands(
        Dictionary<int, CommandRegistryEntry> entries,
        ReadOnlySpan<int> commandTypes,
        CommandFieldType[] fieldTypes,
        bool isServerCommand = false,
        bool baseFirst = true
    )
    {
        AddStructuredFieldCommands(
            entries,
            commandTypes,
            fieldTypes.Select(CommandFieldSchema.Primitive).ToArray(),
            isServerCommand,
            baseFirst
        );
    }

    private static void AddStructuredFieldCommands(
        Dictionary<int, CommandRegistryEntry> entries,
        ReadOnlySpan<int> commandTypes,
        CommandFieldSchema[] fieldSchemas,
        bool isServerCommand = false,
        bool baseFirst = true
    )
    {
        foreach (var type in commandTypes)
        {
            var commandType = type;
            entries.Add(
                commandType,
                new CommandRegistryEntry(
                    Type: isServerCommand
                        ? typeof(ServerCommandWithFields)
                        : typeof(CommandWithFields),
                    IsServerCommand: isServerCommand,
                    BaseFirst: baseFirst,
                    FieldSchemas: fieldSchemas,
                    Factory: isServerCommand
                        ? (stream, environment, _) =>
                            ServerCommandWithFields.Decode(
                                commandType,
                                fieldSchemas,
                                baseFirst,
                                stream,
                                environment
                            )
                        : (stream, environment, _) =>
                            CommandWithFields.Decode(
                                commandType,
                                fieldSchemas,
                                baseFirst,
                                stream,
                                environment
                            )
                )
            );
        }
    }

    /// <summary>
    /// Decodes one registered command from native wire data.
    /// Rejects command types that are not valid in the selected environment.
    /// </summary>
    public static Command Decode(
        MessageStream stream,
        CommandEnvironment environment,
        ICommandDataResolver? dataResolver = null
    )
    {
        var commandType = stream.ReadVarInt();

        if (!Entries.TryGetValue(commandType, out var entry))
            throw new NotSupportedException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Logic command type {commandType} is not supported."
                )
            );

        EnsureAllowedEnvironment(commandType, environment);
        return entry.Factory(stream, environment, dataResolver);
    }

    /// <summary>
    /// Encodes one registered command in native wire order.
    /// Rejects command models that do not match the registered type.
    /// </summary>
    public static void Encode(MessageStream stream, Command command, CommandEnvironment environment)
    {
        if (
            !Entries.TryGetValue(command.Type, out var entry)
            || !entry.Type.IsInstanceOfType(command)
        )
            throw new NotSupportedException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Logic command type {command.Type} is not supported."
                )
            );

        EnsureAllowedEnvironment(command.Type, environment);
        stream.WriteVarInt(command.Type);
        command.EncodeBody(stream, environment);
    }

    internal static bool ValidateFields(
        int type,
        ReadOnlySpan<CommandField> fields,
        bool isServerCommand
    )
    {
        if (
            !Entries.TryGetValue(type, out var entry)
            || entry.IsServerCommand != isServerCommand
            || entry.FieldSchemas is null
        )
            throw new NotSupportedException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Logic command type {type} does not have a registered primitive field schema."
                )
            );

        if (!CommandFieldSchema.AreValid(entry.FieldSchemas, fields))
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Logic command type {type} fields do not match the registered native schema."
                )
            );

        return entry.BaseFirst;
    }

    private static void EnsureAllowedEnvironment(int commandType, CommandEnvironment environment)
    {
        if (
            environment is CommandEnvironment.Production
            && NonProductionCommandTypes.Contains(commandType)
        )
            throw new NotSupportedException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Logic command type {commandType} is not allowed in the production environment."
                )
            );
    }
}
