namespace SupercellProxy.Playground.Commands;

internal static partial class CommandRegistry
{
    private static readonly CommandPrimitiveSchema[] ProductionPrimitiveSchemas =
    [
        new CommandPrimitiveSchema(
            new int[41]
            {
                502,
                511,
                512,
                516,
                518,
                519,
                520,
                522,
                532,
                534,
                538,
                556,
                558,
                559,
                565,
                569,
                570,
                576,
                586,
                589,
                597,
                602,
                605,
                610,
                611,
                612,
                617,
                618,
                623,
                624,
                626,
                627,
                629,
                632,
                651,
                656,
                665,
                667,
                669,
                693,
                696,
            },
            new CommandFieldType[1]
        ),
        new CommandPrimitiveSchema(
            new int[6] { 561, 585, 591, 600, 609, 673 },
            new CommandFieldType[1] { CommandFieldType.Boolean }
        ),
        new CommandPrimitiveSchema(
            new int[1] { 692 },
            new CommandFieldType[1] { CommandFieldType.String }
        ),
        new CommandPrimitiveSchema(
            new int[2] { 525, 625 },
            new CommandFieldType[2] { CommandFieldType.Boolean, CommandFieldType.VarInt }
        ),
        new CommandPrimitiveSchema(
            new int[1] { 679 },
            new CommandFieldType[2] { CommandFieldType.String, CommandFieldType.VarInt }
        ),
        new CommandPrimitiveSchema(
            new int[1] { 560 },
            new CommandFieldType[2] { CommandFieldType.VarInt, CommandFieldType.Boolean }
        ),
        new CommandPrimitiveSchema(
            new int[16]
            {
                501,
                509,
                523,
                549,
                550,
                568,
                606,
                607,
                620,
                621,
                642,
                650,
                655,
                658,
                675,
                691,
            },
            new CommandFieldType[2]
        ),
        new CommandPrimitiveSchema(
            new int[1] { 661 },
            new CommandFieldType[3]
            {
                CommandFieldType.Boolean,
                CommandFieldType.String,
                CommandFieldType.Boolean,
            }
        ),
        new CommandPrimitiveSchema(
            new int[1] { 634 },
            new CommandFieldType[3]
            {
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
            }
        ),
        new CommandPrimitiveSchema(
            new int[2] { 514, 594 },
            new CommandFieldType[3]
            {
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
            }
        ),
        new CommandPrimitiveSchema(new int[2] { 608, 666 }, new CommandFieldType[3]),
        new CommandPrimitiveSchema(
            new int[1] { 641 },
            new CommandFieldType[5]
            {
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            }
        ),
        new CommandPrimitiveSchema(
            new int[1] { 574 },
            new CommandFieldType[6]
            {
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
            }
        ),
        new CommandPrimitiveSchema(
            new int[1] { 577 },
            new CommandFieldType[7]
            {
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
                CommandFieldType.VarInt,
                CommandFieldType.VarInt,
                CommandFieldType.Boolean,
            }
        ),
        new CommandPrimitiveSchema(
            new int[1] { 810 },
            new CommandFieldType[7],
            isServerCommand: true
        ),
        new CommandPrimitiveSchema(
            new int[1] { 839 },
            new CommandFieldType[2] { CommandFieldType.Boolean, CommandFieldType.Boolean },
            isServerCommand: true
        ),
    ];
}
