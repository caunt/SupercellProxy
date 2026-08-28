namespace SupercellProxy.Playground.Commands;

internal static partial class CommandRegistry
{
    private static readonly CommandPrimitiveSchema[] ExtendedPrimitiveSchemas =
    [
        new CommandPrimitiveSchema(
            new int[1] { 39 },
            new CommandFieldType[4]
            {
                CommandFieldType.DataReferenceVarIntPairArray,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 149 },
            new CommandFieldType[4]
            {
                CommandFieldType.VarInt,
                CommandFieldType.OptionalLongId,
                CommandFieldType.VarIntArray,
                CommandFieldType.VarIntArray,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 229 },
            new CommandFieldType[16]
            {
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.String,
                CommandFieldType.DataReference,
                CommandFieldType.VarIntPairArray,
                CommandFieldType.VarIntPairArray,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 233 },
            new CommandFieldType[3]
            {
                CommandFieldType.DataReference,
                CommandFieldType.DataReferenceArray,
                CommandFieldType.VarInt,
            }
        ),
        new CommandPrimitiveSchema(
            new int[1] { 266 },
            new CommandFieldType[3]
            {
                CommandFieldType.LongId,
                CommandFieldType.DataReference,
                CommandFieldType.StringArray,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 313 },
            new CommandFieldType[6],
            isServerCommand: true
        ),
        new CommandPrimitiveSchema(
            new int[1] { 322 },
            new CommandFieldType[2]
            {
                CommandFieldType.VarInt,
                CommandFieldType.DataReferenceArray,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 344 },
            new CommandFieldType[1] { CommandFieldType.DataReferenceVarIntPairArray },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 366 },
            new CommandFieldType[6]
            {
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.VarLong,
                CommandFieldType.VarInt,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(new int[1] { 367 }, new CommandFieldType[2]),
        new CommandPrimitiveSchema(
            new int[1] { 368 },
            new CommandFieldType[6]
            {
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.NullableVarLongArray,
                CommandFieldType.VarLong,
                CommandFieldType.VarInt,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[7] { 369, 370, 373, 374, 376, 377, 380 },
            new CommandFieldType[1],
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[2] { 371, 381 },
            new CommandFieldType[2],
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 372 },
            new CommandFieldType[5]
            {
                CommandFieldType.VarIntPairArray,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.String,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 378 },
            new CommandFieldType[2] { CommandFieldType.VarInt, CommandFieldType.Boolean },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 379 },
            new CommandFieldType[1],
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 385 },
            new CommandFieldType[1] { CommandFieldType.LongId },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[2] { 386, 387 },
            new CommandFieldType[4]
            {
                CommandFieldType.LongId,
                CommandFieldType.LongId,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 388 },
            new CommandFieldType[3]
            {
                CommandFieldType.LongId,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 389 },
            new CommandFieldType[13]
            {
                CommandFieldType.OptionalLongId,
                CommandFieldType.OptionalLongId,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 636 },
            new CommandFieldType[2] { CommandFieldType.VarIntArray, CommandFieldType.VarLongArray }
        ),
        new CommandPrimitiveSchema(
            new int[1] { 134 },
            new CommandFieldType[12]
            {
                CommandFieldType.LongId,
                CommandFieldType.DataReference,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 136 },
            new CommandFieldType[4]
            {
                CommandFieldType.LongId,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 643 },
            new CommandFieldType[6]
            {
                CommandFieldType.String,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            }
        ),
    ];
}
