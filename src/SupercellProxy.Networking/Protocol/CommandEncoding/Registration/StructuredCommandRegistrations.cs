using SupercellProxy.Networking.Protocol.CommandEncoding.FieldSchemas;
using SupercellProxy.Networking.Protocol.MessageEncoding;

using static SupercellProxy.Networking.Protocol.CommandEncoding.Registration.CommandRegistry;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.Registration;

internal static class StructuredCommandRegistrations
{
    internal static void AddStructuredCommands(Dictionary<int, CommandRegistryEntry> entries)
    {
        CommandFieldSchema int32Schema = CommandFieldSchema.Primitive(CommandFieldType.Int32);
        CommandFieldSchema varIntSchema = CommandFieldSchema.Primitive(CommandFieldType.VarInt);
        CommandFieldSchema booleanSchema = CommandFieldSchema.Primitive(CommandFieldType.Boolean);
        CommandFieldSchema stringSchema = CommandFieldSchema.Primitive(CommandFieldType.String);
        CommandFieldSchema logicLongSchema = CommandFieldSchema.Primitive(CommandFieldType.LongId);

        CommandFieldSchema dataReferenceSchema = CommandFieldSchema.Primitive(CommandFieldType.DataReference);

        CommandFieldSchema varIntArraySchema = CommandFieldSchema.Primitive(CommandFieldType.VarIntArray);

        CommandFieldSchema byteArraySchema = CommandFieldSchema.Primitive(CommandFieldType.ByteArray);

        CommandFieldSchema optionalInt32PairSchema = CommandFieldSchema.Optional(int32Schema, int32Schema);

        CommandFieldSchema optionalByteArraySchema = CommandFieldSchema.Optional(byteArraySchema);

        CommandFieldSchema type148ElementSchema = CommandFieldSchema.Array(
            nullable: false,
            dataReferenceSchema,
            booleanSchema,
            booleanSchema,
            CommandFieldSchema.Array(nullable: false, dataReferenceSchema, varIntSchema)
        );

        CommandFieldSchema dataReferenceVarIntArraySchema = CommandFieldSchema.Array(nullable: false, dataReferenceSchema, varIntSchema);

        AddStructuredCommands148And();
        AddStructuredCommands197To();
        AddStructuredCommands252To();
        AddStructuredCommands263To();
        AddStructuredCommands296To();
        void AddStructuredCommands148And()
        {
            AddStructuredFieldCommands(
                entries,
                [ServerCommand148Type],
                [
                    varIntSchema,
                    CommandFieldSchema.Optional(logicLongSchema),
                    varIntArraySchema,
                    varIntArraySchema,
                    type148ElementSchema,
                    dataReferenceSchema,
                    int32Schema,
                ],
                MessageDirection.Clientbound,
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
                    varIntSchema,
                    types197To200NestedSchema,
                ],
                MessageDirection.Clientbound,
                baseFirst: false
            );
            AddStructuredFieldCommands(
                entries,
                [198],
                [
                    stringSchema,
                    stringSchema,
                    stringSchema,
                    varIntSchema,
                    types197To200NestedSchema,
                    varIntSchema,
                ],
                MessageDirection.Clientbound,
                baseFirst: false
            );
            AddStructuredFieldCommands(
                entries,
                [200],
                [
                    stringSchema,
                    stringSchema,
                    varIntSchema,
                    types197To200NestedSchema,
                ],
                MessageDirection.Clientbound,
                baseFirst: false
            );

            CommandFieldSchema CreateTypes197To200NestedSchema()
            {
                return CommandFieldSchema.Optional(
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
        }
        void AddStructuredCommands252To()
        {
            AddStructuredFieldCommands(
                entries,
                [252],
                [
                    stringSchema,
                    stringSchema,
                    varIntSchema,
                    varIntSchema,
                    dataReferenceVarIntArraySchema,
                    dataReferenceVarIntArraySchema,
                ],
                MessageDirection.Clientbound,
                baseFirst: false
            );
            AddStructuredCommand262(entries, logicLongSchema, varIntSchema, dataReferenceSchema);
            AddStructuredFieldCommands(
                entries,
                [261],
                [
                    stringSchema,
                    stringSchema,
                    varIntSchema,
                    dataReferenceVarIntArraySchema,
                    dataReferenceVarIntArraySchema,
                    varIntSchema,
                ],
                MessageDirection.Clientbound,
                baseFirst: false
            );
            AddStructuredFieldCommands(
                entries,
                [TreeRevivalServerCommandType],
                [
                    logicLongSchema,
                    logicLongSchema,
                    varIntSchema,
                    dataReferenceVarIntArraySchema,
                ],
                MessageDirection.Clientbound,
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
                    ],
                    MessageDirection.Clientbound,
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
                        CommandFieldSchema.Optional(CommandFieldSchema.Optional(dataReferenceSchema, varIntSchema), booleanSchema),
                        booleanSchema
                    ),
                ],
                MessageDirection.Clientbound,
                baseFirst: false
            );
        }
        void AddStructuredCommands296To()
        {
            CommandFieldSchema dataReferenceVarIntPairArraySchema = CommandFieldSchema.Primitive(CommandFieldType.DataReferenceVarIntPairArray);

            AddStructuredFieldCommands(
                entries,
                [296],
                [
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
                    CommandFieldSchema.Optional(CommandFieldSchema.Optional(dataReferenceSchema, varIntSchema), booleanSchema),
                ],
                MessageDirection.Serverbound
            );

            CommandFieldSchema types305And306Schema = CommandFieldSchema.Optional(dataReferenceVarIntPairArraySchema, varIntSchema);

            AddStructuredFieldCommands(entries, [305], [types305And306Schema], MessageDirection.Clientbound, baseFirst: false);
            AddStructuredFieldCommands(entries, [306], [types305And306Schema], MessageDirection.Serverbound, baseFirst: false);
            CommandFieldSchema type755Schema = CreateType755Schema();
            AddStructuredFieldCommands(entries, [DecorationVoteCandidatesServerCommandType], [type755Schema], MessageDirection.Clientbound);

            CommandFieldSchema CreateType755Schema()
            {
                CommandFieldSchema nestedSchema = CommandFieldSchema.Optional(
                    CommandFieldSchema.Primitive(CommandFieldType.String),
                    optionalByteArraySchema,
                    varIntSchema,
                    varIntSchema,
                    varIntSchema,
                    optionalInt32PairSchema,
                    varIntSchema
                );

                return CommandFieldSchema.Optional(varIntSchema, nestedSchema, nestedSchema, varIntSchema);
            }
        }
    }

    private static void AddStructuredCommand262(
        Dictionary<int, CommandRegistryEntry> entries,
        CommandFieldSchema logicLongSchema,
        CommandFieldSchema varIntSchema,
        CommandFieldSchema dataReferenceSchema
    )
    {
        AddStructuredFieldCommands(
            entries,
            [RemoteOrderCompletionServerCommandType],
            [
                logicLongSchema,
                logicLongSchema,
                varIntSchema,
                varIntSchema,
                dataReferenceSchema,
                varIntSchema,
                dataReferenceSchema,
                varIntSchema,
                dataReferenceSchema,
                varIntSchema,
                dataReferenceSchema,
                varIntSchema,
                dataReferenceSchema,
                varIntSchema,
                varIntSchema,
                dataReferenceSchema,
                varIntSchema,
                dataReferenceSchema,
                varIntSchema,
            ],
            MessageDirection.Clientbound,
            baseFirst: false
        );
    }
}
