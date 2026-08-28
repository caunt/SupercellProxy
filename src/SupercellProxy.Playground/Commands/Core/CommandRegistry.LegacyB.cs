namespace SupercellProxy.Playground.Commands;

internal static partial class CommandRegistry
{
    private static readonly CommandPrimitiveSchema[] LegacyPrimitiveSchemasB =
    [
        new CommandPrimitiveSchema(
            new int[1] { 167 },
            new CommandFieldType[5]
            {
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.OptionalLongId,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 174 },
            new CommandFieldType[7]
            {
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.OptionalLongId,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 182 },
            new CommandFieldType[3]
            {
                CommandFieldType.VarInt,
                CommandFieldType.LongId,
                CommandFieldType.OptionalLongId,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 205 },
            new CommandFieldType[2]
            {
                CommandFieldType.Byte,
                CommandFieldType.ByteCountedVarIntArray,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 231 },
            new CommandFieldType[6]
            {
                CommandFieldType.VarInt,
                CommandFieldType.LongId,
                CommandFieldType.OptionalLongId,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 323 },
            new CommandFieldType[4]
            {
                CommandFieldType.VarInt,
                CommandFieldType.LongId,
                CommandFieldType.OptionalLongId,
                CommandFieldType.VarInt,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 243 },
            new CommandFieldType[7]
            {
                CommandFieldType.LongId,
                CommandFieldType.LongId,
                CommandFieldType.LongId,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 244 },
            new CommandFieldType[5]
            {
                CommandFieldType.LongId,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 245 },
            new CommandFieldType[3]
            {
                CommandFieldType.Int32,
                CommandFieldType.String,
                CommandFieldType.Boolean,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 246 },
            new CommandFieldType[8]
            {
                CommandFieldType.LongId,
                CommandFieldType.LongId,
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
            new int[1] { 249 },
            new CommandFieldType[5]
            {
                CommandFieldType.LongId,
                CommandFieldType.LongId,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 265 },
            new CommandFieldType[7]
            {
                CommandFieldType.String,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 309 },
            new CommandFieldType[7]
            {
                CommandFieldType.LongId,
                CommandFieldType.LongId,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.VarInt,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 304 },
            new CommandFieldType[12]
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
                CommandFieldType.Boolean,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 325 },
            new CommandFieldType[2] { CommandFieldType.String, CommandFieldType.Int32 },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 331 },
            new CommandFieldType[8]
            {
                CommandFieldType.LongId,
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
            new int[1] { 353 },
            new CommandFieldType[6]
            {
                CommandFieldType.LongId,
                CommandFieldType.LongId,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 382 },
            new CommandFieldType[6]
            {
                CommandFieldType.LongId,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Boolean,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 390 },
            new CommandFieldType[1] { CommandFieldType.String },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 192 },
            new CommandFieldType[9]
            {
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(new int[1] { 235 }, new CommandFieldType[1]),
        new CommandPrimitiveSchema(new int[1] { 270 }, new CommandFieldType[2]),
        new CommandPrimitiveSchema(new int[1] { 317 }, new CommandFieldType[4]),
        new CommandPrimitiveSchema(
            new int[3] { 540, 563, 588 },
            new CommandFieldType[1] { CommandFieldType.DataReference }
        ),
        new CommandPrimitiveSchema(
            new int[1] { 521 },
            new CommandFieldType[3]
            {
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.DataReference,
            }
        ),
        new CommandPrimitiveSchema(
            new int[1] { 579 },
            new CommandFieldType[3]
            {
                CommandFieldType.VarInt,
                CommandFieldType.DataReference,
                CommandFieldType.DataReference,
            }
        ),
        new CommandPrimitiveSchema(
            new int[3] { 601, 670, 686 },
            new CommandFieldType[1] { CommandFieldType.VarIntArray }
        ),
        new CommandPrimitiveSchema(
            new int[1] { 226 },
            new CommandFieldType[1] { CommandFieldType.Int32 },
            isServerCommand: true
        ),
        new CommandPrimitiveSchema(
            new int[1] { 248 },
            new CommandFieldType[2] { CommandFieldType.Int32, CommandFieldType.VarInt },
            isServerCommand: true
        ),
        new CommandPrimitiveSchema(
            new int[1] { 299 },
            new CommandFieldType[1] { CommandFieldType.LongId }
        ),
        new CommandPrimitiveSchema(
            new int[1] { 349 },
            new CommandFieldType[3]
            {
                CommandFieldType.String,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
            },
            isServerCommand: true
        ),
        new CommandPrimitiveSchema(
            new int[1] { 543 },
            new CommandFieldType[3]
            {
                CommandFieldType.VarInt,
                CommandFieldType.DataReference,
                CommandFieldType.VarInt,
            }
        ),
        new CommandPrimitiveSchema(
            new int[1] { 552 },
            new CommandFieldType[2] { CommandFieldType.Int32, CommandFieldType.Int32 }
        ),
        new CommandPrimitiveSchema(
            new int[1] { 771 },
            new CommandFieldType[6]
            {
                CommandFieldType.VarInt,
                CommandFieldType.String,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarLong,
            },
            isServerCommand: true
        ),
    ];
}
