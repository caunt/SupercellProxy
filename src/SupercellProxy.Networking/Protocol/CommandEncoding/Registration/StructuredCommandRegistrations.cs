using SupercellProxy.Networking.Protocol.CommandEncoding.FieldSchemas;

using static SupercellProxy.Networking.Protocol.CommandEncoding.Registration.CommandRegistry;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

internal static class StructuredCommandRegistrations
{
    internal static void AddStructuredCommands(Dictionary<int, CommandRegistryEntry> entries)
    {
        CommandFieldSchema int32Schema = CommandFieldSchema.Primitive(CommandFieldType.Int32);
        CommandFieldSchema variableIntSchema = CommandFieldSchema.Primitive(CommandFieldType.VariableInt);
        CommandFieldSchema booleanSchema = CommandFieldSchema.Primitive(CommandFieldType.Boolean);
        CommandFieldSchema stringSchema = CommandFieldSchema.Primitive(CommandFieldType.String);
        CommandFieldSchema logicLongSchema = CommandFieldSchema.Primitive(CommandFieldType.LongIdentifier);

        CommandFieldSchema dataReferenceSchema = CommandFieldSchema.Primitive(CommandFieldType.DataReference);

        CommandFieldSchema variableIntArraySchema = CommandFieldSchema.Primitive(CommandFieldType.VariableIntArray);

        CommandFieldSchema byteArraySchema = CommandFieldSchema.Primitive(CommandFieldType.ByteArray);

        CommandFieldSchema optionalInt32PairSchema = CommandFieldSchema.Optional(int32Schema, int32Schema);

        CommandFieldSchema optionalByteArraySchema = CommandFieldSchema.Optional(byteArraySchema);

        CommandFieldSchema type148ElementSchema = CommandFieldSchema.Array(
            nullable: false,
            dataReferenceSchema,
            booleanSchema,
            booleanSchema,
            CommandFieldSchema.Array(nullable: false, dataReferenceSchema, variableIntSchema)
        );

        CommandFieldSchema dataReferenceVariableIntArraySchema = CommandFieldSchema.Array(nullable: false, dataReferenceSchema, variableIntSchema);

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
                variableIntSchema,
                variableIntSchema,
                variableIntSchema,
                variableIntSchema,
                variableIntArraySchema,
                variableIntSchema,
                variableIntSchema,
                variableIntSchema,
                variableIntSchema,
                booleanSchema,
                booleanSchema,
                booleanSchema,
                booleanSchema,
                booleanSchema,
                variableIntSchema,
                variableIntSchema,
                CommandFieldSchema.Primitive(CommandFieldType.StringArray),
                booleanSchema,
                variableIntSchema,
                variableIntSchema,
                variableIntSchema,
                booleanSchema,
                variableIntSchema
            );

            AddStructuredFieldCommands(
                entries,
                [170],
                [
                    stringSchema,
                    stringSchema,
                    variableIntSchema,
                    variableIntSchema,
                    variableIntSchema,
                    variableIntArraySchema,
                    variableIntSchema,
                    variableIntSchema,
                    variableIntSchema,
                    booleanSchema,
                    booleanSchema,
                    booleanSchema,
                    booleanSchema,
                    booleanSchema,
                    variableIntSchema,
                    variableIntSchema,
                    variableIntSchema,
                    variableIntSchema,
                    type170NestedSchema,
                    variableIntArraySchema,
                ],
                isServerCommand: true,
                baseFirst: false
            );
        }
        void AddStructuredCommand176()
        {
            AddStructuredFieldCommands(
                entries,
                [176],
                [
                    CommandFieldSchema.Array(
                        nullable: false,
                        stringSchema,
                        variableIntSchema,
                        variableIntSchema,
                        variableIntSchema,
                        CommandFieldSchema.Optional(stringSchema)
                    ),
                    variableIntSchema,
                ],
                isServerCommand: false,
                baseFirst: false
            );
        }
        void AddStructuredCommands148And()
        {
            AddStructuredFieldCommands(
                entries,
                [ServerCommand148Type],
                [
                    variableIntSchema,
                    CommandFieldSchema.Optional(logicLongSchema),
                    variableIntArraySchema,
                    variableIntArraySchema,
                    type148ElementSchema,
                    dataReferenceSchema,
                    int32Schema,
                ],
                isServerCommand: true,
                baseFirst: false
            );
            AddStructuredFieldCommands(
                entries,
                [168],
                [
                    variableIntSchema,
                    variableIntSchema,
                    variableIntSchema,
                    variableIntSchema,
                    CommandFieldSchema.Optional(stringSchema),
                    CommandFieldSchema.Optional(
                        CommandFieldSchema.Array(
                            nullable: true,
                            variableIntSchema,
                            variableIntSchema,
                            variableIntSchema,
                            variableIntSchema,
                            variableIntSchema,
                            variableIntSchema,
                            variableIntSchema,
                            variableIntSchema,
                            booleanSchema
                        )
                    ),
                ],
                isServerCommand: true,
                baseFirst: false
            );
        }
        void AddStructuredCommands197To()
        {
            CommandFieldSchema types197To200NestedSchema = CreateTypes197To200NestedSchema();
            AddStructuredFieldCommands(
                entries,
                [197],
                [
                    stringSchema,
                    stringSchema,
                    stringSchema,
                    variableIntSchema,
                    types197To200NestedSchema,
                ],
                isServerCommand: true,
                baseFirst: false
            );
            AddStructuredFieldCommands(
                entries,
                [198],
                [
                    stringSchema,
                    stringSchema,
                    stringSchema,
                    variableIntSchema,
                    types197To200NestedSchema,
                    variableIntSchema,
                ],
                isServerCommand: true,
                baseFirst: false
            );
            AddStructuredFieldCommands(
                entries,
                [200],
                [
                    stringSchema,
                    stringSchema,
                    variableIntSchema,
                    types197To200NestedSchema,
                ],
                isServerCommand: true,
                baseFirst: false
            );

            CommandFieldSchema CreateTypes197To200NestedSchema()
            {
                return CommandFieldSchema.Optional(
                    stringSchema,
                    variableIntSchema,
                    variableIntSchema,
                    variableIntSchema,
                    variableIntSchema,
                    booleanSchema,
                    CommandFieldSchema.Primitive(CommandFieldType.VariableLong),
                    CommandFieldSchema.Array(
                        nullable: true,
                        stringSchema,
                        CommandFieldSchema.Primitive(CommandFieldType.VariableLong),
                        variableIntSchema,
                        variableIntSchema,
                        variableIntSchema,
                        variableIntSchema,
                        variableIntSchema,
                        variableIntSchema,
                        variableIntSchema,
                        variableIntSchema
                    ),
                    CommandFieldSchema.Optional(stringSchema)
                );
            }
        }
        void AddStructuredCommands252To()
        {
            AddStructuredFieldCommands(
                entries,
                [252],
                [
                    stringSchema,
                    stringSchema,
                    variableIntSchema,
                    variableIntSchema,
                    dataReferenceVariableIntArraySchema,
                    dataReferenceVariableIntArraySchema,
                ],
                isServerCommand: true,
                baseFirst: false
            );
            AddStructuredCommand262(entries, logicLongSchema, variableIntSchema, dataReferenceSchema);
            AddStructuredFieldCommands(
                entries,
                [261],
                [
                    stringSchema,
                    stringSchema,
                    variableIntSchema,
                    dataReferenceVariableIntArraySchema,
                    dataReferenceVariableIntArraySchema,
                    variableIntSchema,
                ],
                isServerCommand: true,
                baseFirst: false
            );
            AddStructuredFieldCommands(
                entries,
                [TreeRevivalServerCommandType],
                [
                    logicLongSchema,
                    logicLongSchema,
                    variableIntSchema,
                    dataReferenceVariableIntArraySchema,
                ],
                isServerCommand: true,
                baseFirst: false
            );
            AddStructuredCommand256();

            void AddStructuredCommand256()
            {
                AddStructuredFieldCommands(
                    entries,
                    [256],
                    [
                        logicLongSchema,
                        dataReferenceSchema,
                        CommandFieldSchema.Array(
                            nullable: false,
                            variableIntSchema,
                            variableIntSchema,
                            variableIntSchema,
                            variableIntSchema,
                            variableIntSchema,
                            variableIntSchema,
                            dataReferenceSchema,
                            type148ElementSchema,
                            dataReferenceVariableIntArraySchema,
                            dataReferenceVariableIntArraySchema,
                            CommandFieldSchema.Optional(stringSchema)
                        ),
                    ],
                    isServerCommand: true,
                    baseFirst: false
                );
            }
        }
        void AddStructuredCommands263To()
        {
            AddStructuredFieldCommands(
                entries,
                [RemoteOrderUpdatesServerCommandType],
                [
                    logicLongSchema,
                    CommandFieldSchema.Array(
                        nullable: false,
                        CommandFieldSchema.Primitive(CommandFieldType.DataReferenceArray),
                        variableIntArraySchema,
                        variableIntSchema,
                        variableIntSchema,
                        variableIntSchema,
                        variableIntSchema,
                        booleanSchema,
                        variableIntSchema,
                        variableIntSchema,
                        booleanSchema,
                        variableIntSchema,
                        dataReferenceSchema,
                        variableIntSchema,
                        dataReferenceSchema,
                        variableIntSchema,
                        variableIntSchema,
                        optionalInt32PairSchema,
                        variableIntSchema,
                        variableIntSchema,
                        booleanSchema,
                        booleanSchema,
                        CommandFieldSchema.Optional(CommandFieldSchema.Optional(dataReferenceSchema, variableIntSchema), booleanSchema),
                        booleanSchema
                    ),
                ],
                isServerCommand: true,
                baseFirst: false
            );
            AddStructuredFieldCommands(entries, [687], [optionalInt32PairSchema, variableIntSchema, variableIntSchema]);
        }
        void AddStructuredCommands296To()
        {
            CommandFieldSchema dataReferenceVariableIntPairArraySchema = CommandFieldSchema.Primitive(CommandFieldType.DataReferenceVariableIntPairArray);

            AddStructuredFieldCommands(
                entries,
                [296],
                [
                    logicLongSchema,
                    variableIntSchema,
                    CommandFieldSchema.Optional(
                        dataReferenceSchema,
                        variableIntSchema,
                        variableIntSchema,
                        dataReferenceVariableIntPairArraySchema,
                        dataReferenceVariableIntPairArraySchema,
                        stringSchema
                    ),
                    CommandFieldSchema.Optional(CommandFieldSchema.Optional(dataReferenceSchema, variableIntSchema), booleanSchema),
                ]
            );

            CommandFieldSchema types305And306Schema = CommandFieldSchema.Optional(dataReferenceVariableIntPairArraySchema, variableIntSchema);

            AddStructuredFieldCommands(entries, [305], [types305And306Schema], isServerCommand: true, baseFirst: false);
            AddStructuredFieldCommands(entries, [306], [types305And306Schema], isServerCommand: false, baseFirst: false);
            CommandFieldSchema type755Schema = CreateType755Schema();
            AddStructuredFieldCommands(entries, [755], [type755Schema], isServerCommand: true);

            CommandFieldSchema CreateType755Schema()
            {
                CommandFieldSchema nestedSchema = CommandFieldSchema.Optional(
                    CommandFieldSchema.Primitive(CommandFieldType.String),
                    optionalByteArraySchema,
                    variableIntSchema,
                    variableIntSchema,
                    variableIntSchema,
                    optionalInt32PairSchema,
                    variableIntSchema
                );

                return CommandFieldSchema.Optional(variableIntSchema, nestedSchema, nestedSchema, variableIntSchema);
            }
        }
    }

    private static void AddStructuredCommand262(
        Dictionary<int, CommandRegistryEntry> entries,
        CommandFieldSchema logicLongSchema,
        CommandFieldSchema variableIntSchema,
        CommandFieldSchema dataReferenceSchema
    )
    {
        AddStructuredFieldCommands(
            entries,
            [RemoteOrderCompletionServerCommandType],
            [
                logicLongSchema,
                logicLongSchema,
                variableIntSchema,
                variableIntSchema,
                dataReferenceSchema,
                variableIntSchema,
                dataReferenceSchema,
                variableIntSchema,
                dataReferenceSchema,
                variableIntSchema,
                dataReferenceSchema,
                variableIntSchema,
                dataReferenceSchema,
                variableIntSchema,
                variableIntSchema,
                dataReferenceSchema,
                variableIntSchema,
                dataReferenceSchema,
                variableIntSchema,
            ],
            isServerCommand: true,
            baseFirst: false
        );
    }

}
