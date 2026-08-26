namespace SupercellProxy.Playground.Commands;

public static partial class CommandRegistry
{
    private static void AddStructuredCommands(Dictionary<int, CommandRegistryEntry> entries)
    {
        CommandFieldSchema int32Schema = CommandFieldSchema.Primitive(CommandFieldType.Int32);
        CommandFieldSchema varIntSchema = CommandFieldSchema.Primitive(CommandFieldType.VarInt);
        CommandFieldSchema booleanSchema = CommandFieldSchema.Primitive(CommandFieldType.Boolean);
        CommandFieldSchema stringSchema = CommandFieldSchema.Primitive(CommandFieldType.String);
        CommandFieldSchema logicLongSchema = CommandFieldSchema.Primitive(CommandFieldType.LongId);
        CommandFieldSchema dataReferenceSchema = CommandFieldSchema.Primitive(
            CommandFieldType.DataReference
        );
        CommandFieldSchema varIntArraySchema = CommandFieldSchema.Primitive(
            CommandFieldType.VarIntArray
        );
        CommandFieldSchema byteArraySchema = CommandFieldSchema.Primitive(
            CommandFieldType.ByteArray
        );
        CommandFieldSchema optionalInt32PairSchema = CommandFieldSchema.Optional(
            int32Schema,
            int32Schema
        );
        CommandFieldSchema optionalByteArraySchema = CommandFieldSchema.Optional(byteArraySchema);
        CommandFieldSchema type148ElementSchema = CommandFieldSchema.Array(
            nullable: false,
            dataReferenceSchema,
            booleanSchema,
            booleanSchema,
            CommandFieldSchema.Array(nullable: false, dataReferenceSchema, varIntSchema)
        );
        CommandFieldSchema dataReferenceVarIntArraySchema = CommandFieldSchema.Array(
            nullable: false,
            dataReferenceSchema,
            varIntSchema
        );
        AddStructuredCommands148And();
        AddStructuredCommand();
        AddStructuredCommand176();
        AddStructuredCommands197To();
        AddStructuredCommands252To();
        AddStructuredCommands263To();
        AddStructuredCommands296To();
        void AddStructuredCommand()
        {
            CommandFieldSchema type170NestedSchema = CommandFieldSchema.Optional(
                CommandFieldSchema.Optional(stringSchema),
                varIntSchema,
                varIntSchema,
                varIntSchema,
                varIntSchema,
                varIntArraySchema,
                varIntSchema,
                varIntSchema,
                varIntSchema,
                varIntSchema,
                booleanSchema,
                booleanSchema,
                booleanSchema,
                booleanSchema,
                booleanSchema,
                varIntSchema,
                varIntSchema,
                CommandFieldSchema.Primitive(CommandFieldType.StringArray),
                booleanSchema,
                varIntSchema,
                varIntSchema,
                varIntSchema,
                booleanSchema,
                varIntSchema
            );
            AddStructuredFieldCommands(
                entries,
                new int[1] { 170 },
                new CommandFieldSchema[20]
                {
                    stringSchema,
                    stringSchema,
                    varIntSchema,
                    varIntSchema,
                    varIntSchema,
                    varIntArraySchema,
                    varIntSchema,
                    varIntSchema,
                    varIntSchema,
                    booleanSchema,
                    booleanSchema,
                    booleanSchema,
                    booleanSchema,
                    booleanSchema,
                    varIntSchema,
                    varIntSchema,
                    varIntSchema,
                    varIntSchema,
                    type170NestedSchema,
                    varIntArraySchema,
                },
                isServerCommand: true,
                baseFirst: false
            );
        }
        void AddStructuredCommand176()
        {
            AddStructuredFieldCommands(
                entries,
                new int[1] { 176 },
                new CommandFieldSchema[2]
                {
                    CommandFieldSchema.Array(
                        nullable: false,
                        stringSchema,
                        varIntSchema,
                        varIntSchema,
                        varIntSchema,
                        CommandFieldSchema.Optional(stringSchema)
                    ),
                    varIntSchema,
                },
                isServerCommand: false,
                baseFirst: false
            );
        }
        void AddStructuredCommands148And()
        {
            AddStructuredFieldCommands(
                entries,
                new int[1] { 148 },
                new CommandFieldSchema[7]
                {
                    varIntSchema,
                    CommandFieldSchema.Optional(logicLongSchema),
                    varIntArraySchema,
                    varIntArraySchema,
                    type148ElementSchema,
                    dataReferenceSchema,
                    int32Schema,
                },
                isServerCommand: true,
                baseFirst: false
            );
            AddStructuredFieldCommands(
                entries,
                new int[1] { 168 },
                new CommandFieldSchema[6]
                {
                    varIntSchema,
                    varIntSchema,
                    varIntSchema,
                    varIntSchema,
                    CommandFieldSchema.Optional(stringSchema),
                    CommandFieldSchema.Optional(
                        CommandFieldSchema.Array(
                            nullable: true,
                            varIntSchema,
                            varIntSchema,
                            varIntSchema,
                            varIntSchema,
                            varIntSchema,
                            varIntSchema,
                            varIntSchema,
                            varIntSchema,
                            booleanSchema
                        )
                    ),
                },
                isServerCommand: true,
                baseFirst: false
            );
        }
        void AddStructuredCommands197To()
        {
            var types197To200NestedSchema = CreateTypes197To200NestedSchema();
            AddStructuredFieldCommands(
                entries,
                new int[1] { 197 },
                new CommandFieldSchema[5]
                {
                    stringSchema,
                    stringSchema,
                    stringSchema,
                    varIntSchema,
                    types197To200NestedSchema,
                },
                isServerCommand: true,
                baseFirst: false
            );
            AddStructuredFieldCommands(
                entries,
                new int[1] { 198 },
                new CommandFieldSchema[6]
                {
                    stringSchema,
                    stringSchema,
                    stringSchema,
                    varIntSchema,
                    types197To200NestedSchema,
                    varIntSchema,
                },
                isServerCommand: true,
                baseFirst: false
            );
            AddStructuredFieldCommands(
                entries,
                new int[1] { 200 },
                new CommandFieldSchema[4]
                {
                    stringSchema,
                    stringSchema,
                    varIntSchema,
                    types197To200NestedSchema,
                },
                isServerCommand: true,
                baseFirst: false
            );

            CommandFieldSchema CreateTypes197To200NestedSchema() =>
                CommandFieldSchema.Optional(
                    stringSchema,
                    varIntSchema,
                    varIntSchema,
                    varIntSchema,
                    varIntSchema,
                    booleanSchema,
                    CommandFieldSchema.Primitive(CommandFieldType.VarLong),
                    CommandFieldSchema.Array(
                        nullable: true,
                        stringSchema,
                        CommandFieldSchema.Primitive(CommandFieldType.VarLong),
                        varIntSchema,
                        varIntSchema,
                        varIntSchema,
                        varIntSchema,
                        varIntSchema,
                        varIntSchema,
                        varIntSchema,
                        varIntSchema
                    ),
                    CommandFieldSchema.Optional(stringSchema)
                );
        }
        void AddStructuredCommands252To()
        {
            AddStructuredFieldCommands(
                entries,
                new int[1] { 252 },
                new CommandFieldSchema[6]
                {
                    stringSchema,
                    stringSchema,
                    varIntSchema,
                    varIntSchema,
                    dataReferenceVarIntArraySchema,
                    dataReferenceVarIntArraySchema,
                },
                isServerCommand: true,
                baseFirst: false
            );
            AddStructuredFieldCommands(
                entries,
                new int[2] { 253, 262 },
                new CommandFieldSchema[5]
                {
                    stringSchema,
                    stringSchema,
                    varIntSchema,
                    dataReferenceVarIntArraySchema,
                    dataReferenceVarIntArraySchema,
                },
                isServerCommand: true,
                baseFirst: false
            );
            AddStructuredFieldCommands(
                entries,
                new int[1] { 261 },
                new CommandFieldSchema[6]
                {
                    stringSchema,
                    stringSchema,
                    varIntSchema,
                    dataReferenceVarIntArraySchema,
                    dataReferenceVarIntArraySchema,
                    varIntSchema,
                },
                isServerCommand: true,
                baseFirst: false
            );
            AddStructuredFieldCommands(
                entries,
                new int[1] { 328 },
                new CommandFieldSchema[4]
                {
                    stringSchema,
                    stringSchema,
                    varIntSchema,
                    dataReferenceVarIntArraySchema,
                },
                isServerCommand: true,
                baseFirst: false
            );
            AddStructuredCommand256();

            void AddStructuredCommand256()
            {
                AddStructuredFieldCommands(
                    entries,
                    new int[1] { 256 },
                    new CommandFieldSchema[3]
                    {
                        stringSchema,
                        varIntSchema,
                        CommandFieldSchema.Array(
                            nullable: false,
                            varIntSchema,
                            varIntSchema,
                            varIntSchema,
                            varIntSchema,
                            varIntSchema,
                            varIntSchema,
                            dataReferenceSchema,
                            type148ElementSchema,
                            dataReferenceVarIntArraySchema,
                            dataReferenceVarIntArraySchema,
                            CommandFieldSchema.Optional(stringSchema)
                        ),
                    },
                    isServerCommand: true,
                    baseFirst: false
                );
            }
        }
        void AddStructuredCommands263To()
        {
            AddStructuredFieldCommands(
                entries,
                new int[1] { 263 },
                new CommandFieldSchema[2]
                {
                    stringSchema,
                    CommandFieldSchema.Array(
                        nullable: false,
                        CommandFieldSchema.Primitive(CommandFieldType.DataReferenceArray),
                        varIntArraySchema,
                        varIntSchema,
                        varIntSchema,
                        varIntSchema,
                        varIntSchema,
                        booleanSchema,
                        varIntSchema,
                        varIntSchema,
                        booleanSchema,
                        varIntSchema,
                        dataReferenceSchema,
                        varIntSchema,
                        dataReferenceSchema,
                        varIntSchema,
                        varIntSchema,
                        optionalInt32PairSchema,
                        varIntSchema,
                        varIntSchema,
                        booleanSchema,
                        booleanSchema,
                        CommandFieldSchema.Optional(
                            CommandFieldSchema.Optional(dataReferenceSchema, varIntSchema),
                            booleanSchema
                        ),
                        booleanSchema
                    ),
                },
                isServerCommand: true,
                baseFirst: false
            );
            AddStructuredFieldCommands(
                entries,
                new int[1] { 687 },
                new CommandFieldSchema[3] { optionalInt32PairSchema, varIntSchema, varIntSchema }
            );
            AddStructuredFieldCommands(
                entries,
                new int[1] { 743 },
                new CommandFieldSchema[2] { varIntSchema, optionalByteArraySchema },
                isServerCommand: true
            );
        }
        void AddStructuredCommands296To()
        {
            CommandFieldSchema dataReferenceVarIntPairArraySchema = CommandFieldSchema.Primitive(
                CommandFieldType.DataReferenceVarIntPairArray
            );
            AddStructuredFieldCommands(
                entries,
                new int[1] { 296 },
                new CommandFieldSchema[4]
                {
                    logicLongSchema,
                    varIntSchema,
                    CommandFieldSchema.Optional(
                        dataReferenceSchema,
                        varIntSchema,
                        varIntSchema,
                        dataReferenceVarIntPairArraySchema,
                        dataReferenceVarIntPairArraySchema,
                        stringSchema
                    ),
                    CommandFieldSchema.Optional(
                        CommandFieldSchema.Optional(dataReferenceSchema, varIntSchema),
                        booleanSchema
                    ),
                }
            );
            CommandFieldSchema types305And306Schema = CommandFieldSchema.Optional(
                dataReferenceVarIntPairArraySchema,
                varIntSchema
            );
            AddStructuredFieldCommands(
                entries,
                new int[1] { 305 },
                new CommandFieldSchema[1] { types305And306Schema },
                isServerCommand: true,
                baseFirst: false
            );
            AddStructuredFieldCommands(
                entries,
                new int[1] { 306 },
                new CommandFieldSchema[1] { types305And306Schema },
                isServerCommand: false,
                baseFirst: false
            );
            var type755Schema = CreateType755Schema();
            AddStructuredFieldCommands(
                entries,
                new int[1] { 755 },
                new CommandFieldSchema[1] { type755Schema },
                isServerCommand: true
            );

            CommandFieldSchema CreateType755Schema()
            {
                var nestedSchema = CommandFieldSchema.Optional(
                    CommandFieldSchema.Primitive(CommandFieldType.String),
                    optionalByteArraySchema,
                    varIntSchema,
                    varIntSchema,
                    varIntSchema,
                    optionalInt32PairSchema,
                    varIntSchema
                );
                return CommandFieldSchema.Optional(
                    varIntSchema,
                    nestedSchema,
                    nestedSchema,
                    varIntSchema
                );
            }
        }
    }
}
