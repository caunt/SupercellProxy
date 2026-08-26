namespace SupercellProxy.Playground.Commands;

public static partial class CommandRegistry
{
    private static readonly CommandPrimitiveSchema[] LegacyPrimitiveSchemasA =
    [
        new CommandPrimitiveSchema(
            new int[4] { 80, 130, 132, 196 },
            new CommandFieldType[1] { CommandFieldType.Boolean },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[2] { 138, 334 },
            new CommandFieldType[1] { CommandFieldType.LongId },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 27 },
            new CommandFieldType[1] { CommandFieldType.Byte },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 33 },
            new CommandFieldType[1] { CommandFieldType.UInt16 },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[2] { 240, 300 },
            new CommandFieldType[1] { CommandFieldType.Int32 },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[89]
            {
                6,
                15,
                16,
                17,
                19,
                20,
                26,
                29,
                34,
                42,
                43,
                45,
                46,
                47,
                48,
                49,
                51,
                53,
                59,
                61,
                63,
                64,
                65,
                66,
                68,
                70,
                71,
                87,
                88,
                89,
                93,
                96,
                98,
                100,
                101,
                105,
                107,
                109,
                113,
                115,
                116,
                119,
                121,
                122,
                123,
                125,
                128,
                139,
                141,
                150,
                155,
                156,
                157,
                159,
                160,
                161,
                166,
                183,
                185,
                187,
                188,
                189,
                193,
                194,
                202,
                208,
                209,
                215,
                217,
                218,
                219,
                220,
                225,
                236,
                238,
                241,
                286,
                288,
                297,
                324,
                326,
                329,
                330,
                335,
                338,
                340,
                361,
                393,
                394,
            },
            new CommandFieldType[1],
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 54 },
            new CommandFieldType[1],
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 152 },
            new CommandFieldType[1] { CommandFieldType.VarIntArray },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[4] { 285, 303, 318, 333 },
            new CommandFieldType[1] { CommandFieldType.DataReference },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 135 },
            new CommandFieldType[1] { CommandFieldType.LongId },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[4] { 38, 181, 184, 269 },
            new CommandFieldType[1],
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 25 },
            new CommandFieldType[2] { CommandFieldType.VarInt, CommandFieldType.Byte },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 86 },
            new CommandFieldType[3]
            {
                CommandFieldType.VarInt,
                CommandFieldType.DataReference,
                CommandFieldType.VarInt,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 91 },
            new CommandFieldType[2] { CommandFieldType.UInt16, CommandFieldType.UInt16 },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 137 },
            new CommandFieldType[2] { CommandFieldType.LongId, CommandFieldType.VarInt },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 272 },
            new CommandFieldType[1] { CommandFieldType.OptionalLongId },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[7] { 18, 24, 44, 60, 114, 221, 346 },
            new CommandFieldType[2] { CommandFieldType.VarInt, CommandFieldType.Boolean },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[27]
            {
                11,
                21,
                90,
                112,
                118,
                120,
                126,
                140,
                142,
                143,
                144,
                147,
                162,
                165,
                191,
                199,
                204,
                216,
                223,
                237,
                254,
                255,
                259,
                260,
                264,
                343,
                363,
            },
            new CommandFieldType[2],
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[2] { 178, 213 },
            new CommandFieldType[3]
            {
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.Boolean,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[3] { 131, 336, 360 },
            new CommandFieldType[3]
            {
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[7] { 104, 111, 145, 169, 250, 251, 359 },
            new CommandFieldType[3],
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 102 },
            new CommandFieldType[3]
            {
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.LongId,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 179 },
            new CommandFieldType[3]
            {
                CommandFieldType.LongId,
                CommandFieldType.LongId,
                CommandFieldType.VarInt,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 171 },
            new CommandFieldType[7]
            {
                CommandFieldType.LongId,
                CommandFieldType.LongId,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 207 },
            new CommandFieldType[3]
            {
                CommandFieldType.VarInt,
                CommandFieldType.LongId,
                CommandFieldType.LongId,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 211 },
            new CommandFieldType[3]
            {
                CommandFieldType.LongId,
                CommandFieldType.LongId,
                CommandFieldType.Boolean,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 214 },
            new CommandFieldType[3]
            {
                CommandFieldType.VarInt,
                CommandFieldType.Int32,
                CommandFieldType.Boolean,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 273 },
            new CommandFieldType[2] { CommandFieldType.OptionalLongId, CommandFieldType.VarInt },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 302 },
            new CommandFieldType[2] { CommandFieldType.DataReference, CommandFieldType.VarInt },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 230 },
            new CommandFieldType[3]
            {
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 384 },
            new CommandFieldType[3]
            {
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 110 },
            new CommandFieldType[4]
            {
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 316 },
            new CommandFieldType[4]
            {
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 212 },
            new CommandFieldType[4]
            {
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.Boolean,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[2] { 190, 319 },
            new CommandFieldType[4],
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[2] { 7, 85 },
            new CommandFieldType[5]
            {
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.String,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 267 },
            new CommandFieldType[5]
            {
                CommandFieldType.String,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[2] { 172, 173 },
            new CommandFieldType[3]
            {
                CommandFieldType.VarIntArray,
                CommandFieldType.VarIntArray,
                CommandFieldType.VarIntArray,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 342 },
            new CommandFieldType[3],
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 350 },
            new CommandFieldType[4]
            {
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.VarInt,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 351 },
            new CommandFieldType[4]
            {
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.VarInt,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 392 },
            new CommandFieldType[4],
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[2] { 154, 268 },
            new CommandFieldType[5]
            {
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 50 },
            new CommandFieldType[6]
            {
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 227 },
            new CommandFieldType[5]
            {
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            },
            isServerCommand: false,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 228 },
            new CommandFieldType[5]
            {
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.Int32,
                CommandFieldType.VarInt,
                CommandFieldType.OptionalInt32String,
            },
            isServerCommand: true,
            baseFirst: false
        ),
        new CommandPrimitiveSchema(
            new int[1] { 234 },
            new CommandFieldType[13]
            {
                CommandFieldType.LongId,
                CommandFieldType.LongId,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarIntArray,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.Boolean,
                CommandFieldType.Boolean,
                CommandFieldType.Boolean,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
            },
            isServerCommand: true,
            baseFirst: false
        ),
    ];
}
