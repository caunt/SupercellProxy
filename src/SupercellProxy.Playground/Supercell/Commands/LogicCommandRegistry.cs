using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Resolves command type IDs to their typed wire models.
/// </summary>
public static class LogicCommandRegistry
{
    private sealed record Entry(Type Type, bool IsServerCommand, bool BaseFirst, LogicCommandFieldSchema[]? FieldSchemas, Func<SupercellStream, LogicEnvironment, ILogicCommandDataResolver?, LogicCommand> Factory);

    private static readonly Dictionary<int, Entry> _entries = CreateEntries();
    private static readonly HashSet<int> _nonProductionCommandTypes = [7, 84, 85];

    private static Dictionary<int, Entry> CreateEntries()
    {
        var entries = new Dictionary<int, Entry>
        {
            [LogicServerCommand210.CommandType] = new Entry(typeof(LogicServerCommand210), true, false, null, (stream, environment, _) => LogicServerCommand210.Decode(stream, environment)),
            [LogicServerCommand274.CommandType] = new Entry(typeof(LogicServerCommand274), true, true, null, (stream, environment, dataResolver) => LogicServerCommand274.Decode(stream, environment, dataResolver)),
            [LogicServerCommand355.CommandType] = new Entry(typeof(LogicServerCommand355), true, false, null, (stream, environment, _) => LogicServerCommand355.Decode(stream, environment)),
            [LogicCollectAllLettersCommand.CommandType] = new Entry(typeof(LogicCollectAllLettersCommand), false, true, null, (stream, environment, _) => LogicCollectAllLettersCommand.Decode(stream, environment)),
            [LogicCommand247.CommandType] = new Entry(typeof(LogicCommand247), false, true, null, (stream, environment, _) => LogicCommand247.Decode(stream, environment)),
            [LogicCommand321.CommandType] = new Entry(typeof(LogicCommand321), false, true, null, (stream, environment, dataResolver) => LogicCommand321.Decode(stream, environment, dataResolver)),
            [LogicCommand599.CommandType] = new Entry(typeof(LogicCommand599), false, true, null, (stream, environment, _) => LogicCommand599.Decode(stream, environment))
        };

        foreach (var type in LogicCommandWithNoFields.CommandTypes)
        {
            var commandType = type;
            entries.Add(commandType, new Entry(
                typeof(LogicCommandWithNoFields),
                false,
                true,
                null,
                (stream, environment, _) => LogicCommandWithNoFields.Decode(commandType, stream, environment)));
        }

        foreach (var type in LogicMapGameTaskCommand.CommandTypes)
        {
            var commandType = type;
            entries.Add(commandType, new Entry(
                typeof(LogicMapGameTaskCommand),
                false,
                true,
                null,
                (stream, environment, dataResolver) => LogicMapGameTaskCommand.Decode(commandType, stream, environment, dataResolver)));
        }

        AddFieldCommands(entries, [502, 506, 511, 512, 516, 518, 519, 520, 522, 532, 534, 538, 544, 556, 558, 559, 565, 569, 570, 576, 586, 589, 597, 602, 605, 610, 611, 612, 617, 618, 623, 624, 626, 627, 629, 632, 649, 651, 654, 656, 657, 665, 667, 669, 693, 696], [LogicCommandFieldType.VarInt]);
        AddFieldCommands(entries, [561, 585, 591, 600, 609, 673], [LogicCommandFieldType.Boolean]);
        AddFieldCommands(entries, [692], [LogicCommandFieldType.String]);
        AddFieldCommands(entries, [525, 625], [LogicCommandFieldType.Boolean, LogicCommandFieldType.VarInt]);
        AddFieldCommands(entries, [679], [LogicCommandFieldType.String, LogicCommandFieldType.VarInt]);
        AddFieldCommands(entries, [560], [LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean]);
        AddFieldCommands(entries, [501, 509, 523, 549, 550, 568, 606, 607, 620, 621, 642, 650, 655, 658, 675, 691], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt]);
        AddFieldCommands(entries, [661], [LogicCommandFieldType.Boolean, LogicCommandFieldType.String, LogicCommandFieldType.Boolean]);
        AddFieldCommands(entries, [634], [LogicCommandFieldType.Boolean, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean]);
        AddFieldCommands(entries, [514, 594], [LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean, LogicCommandFieldType.VarInt]);
        AddFieldCommands(entries, [608, 666], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt]);
        AddFieldCommands(entries, [641], [LogicCommandFieldType.Boolean, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt]);
        AddFieldCommands(entries, [574], [LogicCommandFieldType.Boolean, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt]);
        AddFieldCommands(entries, [577], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean]);
        AddFieldCommands(entries, [810], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], isServerCommand: true);
        AddFieldCommands(entries, [839], [LogicCommandFieldType.Boolean, LogicCommandFieldType.Boolean], isServerCommand: true);

        AddFieldCommands(entries, [80, 130, 132, 196], [LogicCommandFieldType.Boolean], baseFirst: false);
        AddFieldCommands(entries, [138, 334], [LogicCommandFieldType.LogicLong], baseFirst: false);
        AddFieldCommands(entries, [27], [LogicCommandFieldType.Byte], baseFirst: false);
        AddFieldCommands(entries, [33], [LogicCommandFieldType.UInt16], baseFirst: false);
        AddFieldCommands(entries, [240, 300], [LogicCommandFieldType.Int32], baseFirst: false);
        AddFieldCommands(entries, [6, 15, 16, 17, 19, 20, 26, 29, 34, 35, 42, 43, 45, 46, 47, 48, 49, 51, 53, 59, 61, 63, 64, 65, 66, 68, 70, 71, 87, 88, 89, 93, 96, 98, 100, 101, 105, 107, 109, 113, 115, 116, 119, 121, 122, 123, 125, 128, 139, 141, 150, 155, 156, 157, 159, 160, 161, 166, 183, 185, 187, 188, 189, 193, 194, 202, 208, 209, 215, 217, 218, 219, 220, 225, 236, 238, 241, 286, 288, 297, 324, 326, 329, 330, 335, 338, 340, 361, 393, 394], [LogicCommandFieldType.VarInt], baseFirst: false);
        AddFieldCommands(entries, [54], [LogicCommandFieldType.VarInt], baseFirst: false);
        AddFieldCommands(entries, [152], [LogicCommandFieldType.VarIntArray], baseFirst: false);
        AddFieldCommands(entries, [285, 303, 318, 333], [LogicCommandFieldType.DataReference], baseFirst: false);
        AddFieldCommands(entries, [135], [LogicCommandFieldType.LogicLong], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [38, 181, 184, 269], [LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [25], [LogicCommandFieldType.VarInt, LogicCommandFieldType.Byte], baseFirst: false);
        AddFieldCommands(entries, [86], [LogicCommandFieldType.VarInt, LogicCommandFieldType.DataReference, LogicCommandFieldType.VarInt], baseFirst: false);
        AddFieldCommands(entries, [91], [LogicCommandFieldType.UInt16, LogicCommandFieldType.UInt16], baseFirst: false);
        AddFieldCommands(entries, [137], [LogicCommandFieldType.LogicLong, LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [272], [LogicCommandFieldType.OptionalLogicLong], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [18, 24, 44, 60, 114, 221, 346], [LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean], baseFirst: false);
        AddFieldCommands(entries, [11, 21, 90, 112, 118, 120, 126, 140, 142, 143, 144, 147, 162, 165, 191, 199, 204, 216, 223, 237, 254, 255, 259, 260, 264, 343, 363], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], baseFirst: false);
        AddFieldCommands(entries, [178, 213], [LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean, LogicCommandFieldType.Boolean], baseFirst: false);
        AddFieldCommands(entries, [131, 336, 360], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean], baseFirst: false);
        AddFieldCommands(entries, [104, 111, 145, 169, 250, 251, 359], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], baseFirst: false);
        AddFieldCommands(entries, [102], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.LogicLong], baseFirst: false);
        AddFieldCommands(entries, [179], [LogicCommandFieldType.LogicLong, LogicCommandFieldType.LogicLong, LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [171], [LogicCommandFieldType.LogicLong, LogicCommandFieldType.LogicLong, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [207], [LogicCommandFieldType.VarInt, LogicCommandFieldType.LogicLong, LogicCommandFieldType.LogicLong], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [211], [LogicCommandFieldType.LogicLong, LogicCommandFieldType.LogicLong, LogicCommandFieldType.Boolean], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [214], [LogicCommandFieldType.VarInt, LogicCommandFieldType.Int32, LogicCommandFieldType.Boolean], baseFirst: false);
        AddFieldCommands(entries, [273], [LogicCommandFieldType.OptionalLogicLong, LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [302], [LogicCommandFieldType.DataReference, LogicCommandFieldType.VarInt], baseFirst: false);
        AddFieldCommands(entries, [230], [LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32], baseFirst: false);
        AddFieldCommands(entries, [384], [LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [110], [LogicCommandFieldType.Boolean, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], baseFirst: false);
        AddFieldCommands(entries, [316], [LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], baseFirst: false);
        AddFieldCommands(entries, [212], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean, LogicCommandFieldType.Boolean], baseFirst: false);
        AddFieldCommands(entries, [124, 190, 319], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], baseFirst: false);
        AddFieldCommands(entries, [7, 85], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.String], baseFirst: false);
        AddFieldCommands(entries, [267], [LogicCommandFieldType.String, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], baseFirst: false);
        AddFieldCommands(entries, [172, 173], [LogicCommandFieldType.VarIntArray, LogicCommandFieldType.VarIntArray, LogicCommandFieldType.VarIntArray], baseFirst: false);
        AddFieldCommands(entries, [342], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], baseFirst: false);
        AddFieldCommands(entries, [350], [LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [351], [LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.VarInt], baseFirst: false);
        AddFieldCommands(entries, [392], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [154, 268], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean], baseFirst: false);
        AddFieldCommands(entries, [50], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean], baseFirst: false);
        AddFieldCommands(entries, [227], [LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], baseFirst: false);
        AddFieldCommands(entries, [228], [LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.VarInt, LogicCommandFieldType.OptionalInt32String], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [234], [LogicCommandFieldType.LogicLong, LogicCommandFieldType.LogicLong, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarIntArray, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean, LogicCommandFieldType.Boolean, LogicCommandFieldType.Boolean, LogicCommandFieldType.Boolean, LogicCommandFieldType.Boolean, LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [167], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.OptionalLogicLong, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [174], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.OptionalLogicLong], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [182], [LogicCommandFieldType.VarInt, LogicCommandFieldType.LogicLong, LogicCommandFieldType.OptionalLogicLong], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [205], [LogicCommandFieldType.Byte, LogicCommandFieldType.ByteCountedVarIntArray], baseFirst: false);
        AddFieldCommands(entries, [231], [LogicCommandFieldType.VarInt, LogicCommandFieldType.LogicLong, LogicCommandFieldType.OptionalLogicLong, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [323], [LogicCommandFieldType.VarInt, LogicCommandFieldType.LogicLong, LogicCommandFieldType.OptionalLogicLong, LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [243], [LogicCommandFieldType.LogicLong, LogicCommandFieldType.LogicLong, LogicCommandFieldType.LogicLong, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [244], [LogicCommandFieldType.LogicLong, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [245], [LogicCommandFieldType.Int32, LogicCommandFieldType.String, LogicCommandFieldType.Boolean], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [246], [LogicCommandFieldType.LogicLong, LogicCommandFieldType.LogicLong, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [249], [LogicCommandFieldType.LogicLong, LogicCommandFieldType.LogicLong, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [265], [LogicCommandFieldType.String, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean, LogicCommandFieldType.VarInt], baseFirst: false);
        AddFieldCommands(entries, [309], [LogicCommandFieldType.LogicLong, LogicCommandFieldType.LogicLong, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [304], [LogicCommandFieldType.OptionalLogicLong, LogicCommandFieldType.OptionalLogicLong, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [325], [LogicCommandFieldType.String, LogicCommandFieldType.Int32], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [331], [LogicCommandFieldType.LogicLong, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [353, 375], [LogicCommandFieldType.LogicLong, LogicCommandFieldType.LogicLong, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [382], [LogicCommandFieldType.LogicLong, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32, LogicCommandFieldType.Boolean, LogicCommandFieldType.Int32, LogicCommandFieldType.Int32], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [390], [LogicCommandFieldType.String], baseFirst: false);
        AddFieldCommands(entries, [3], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean], baseFirst: false);
        AddFieldCommands(entries, [192], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean, LogicCommandFieldType.Boolean, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], baseFirst: false);
        AddFieldCommands(entries, [235], [LogicCommandFieldType.VarInt]);
        AddFieldCommands(entries, [270], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt]);
        AddFieldCommands(entries, [317], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt]);
        AddFieldCommands(entries, [540, 563, 588], [LogicCommandFieldType.DataReference]);
        AddFieldCommands(entries, [521], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.DataReference]);
        AddFieldCommands(entries, [579], [LogicCommandFieldType.VarInt, LogicCommandFieldType.DataReference, LogicCommandFieldType.DataReference]);
        AddFieldCommands(entries, [601, 670, 686], [LogicCommandFieldType.VarIntArray]);
        AddFieldCommands(entries, [226], [LogicCommandFieldType.Int32], isServerCommand: true);
        AddFieldCommands(entries, [248], [LogicCommandFieldType.Int32, LogicCommandFieldType.VarInt], isServerCommand: true);
        AddFieldCommands(entries, [299], [LogicCommandFieldType.LogicLong]);
        AddFieldCommands(entries, [349], [LogicCommandFieldType.String, LogicCommandFieldType.Boolean, LogicCommandFieldType.VarInt], isServerCommand: true);
        AddFieldCommands(entries, [543], [LogicCommandFieldType.VarInt, LogicCommandFieldType.DataReference, LogicCommandFieldType.VarInt]);
        AddFieldCommands(entries, [552], [LogicCommandFieldType.Int32, LogicCommandFieldType.Int32]);
        AddFieldCommands(entries, [771], [LogicCommandFieldType.VarInt, LogicCommandFieldType.String, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarLong], isServerCommand: true);

        AddFieldCommands(entries, [39], [LogicCommandFieldType.DataReferenceVarIntPairArray, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], baseFirst: false);
        AddFieldCommands(entries, [149], [LogicCommandFieldType.VarInt, LogicCommandFieldType.OptionalLogicLong, LogicCommandFieldType.VarIntArray, LogicCommandFieldType.VarIntArray], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [229], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean, LogicCommandFieldType.String, LogicCommandFieldType.DataReference, LogicCommandFieldType.VarIntPairArray, LogicCommandFieldType.VarIntPairArray, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean, LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [233], [LogicCommandFieldType.DataReference, LogicCommandFieldType.DataReferenceArray, LogicCommandFieldType.VarInt]);
        AddFieldCommands(entries, [266], [LogicCommandFieldType.LogicLong, LogicCommandFieldType.DataReference, LogicCommandFieldType.StringArray], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [313], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], isServerCommand: true);
        AddFieldCommands(entries, [322], [LogicCommandFieldType.VarInt, LogicCommandFieldType.DataReferenceArray], baseFirst: false);
        AddFieldCommands(entries, [344], [LogicCommandFieldType.DataReferenceVarIntPairArray], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [366], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean, LogicCommandFieldType.VarLong, LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [367], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt]);
        AddFieldCommands(entries, [368], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean, LogicCommandFieldType.NullableVarLongArray, LogicCommandFieldType.VarLong, LogicCommandFieldType.VarInt], baseFirst: false);
        AddFieldCommands(entries, [369, 370, 373, 374, 376, 377, 380], [LogicCommandFieldType.VarInt], baseFirst: false);
        AddFieldCommands(entries, [371, 381], [LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], baseFirst: false);
        AddFieldCommands(entries, [372], [LogicCommandFieldType.VarIntPairArray, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.String], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [378], [LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [379], [LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [385], [LogicCommandFieldType.LogicLong], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [386, 387], [LogicCommandFieldType.LogicLong, LogicCommandFieldType.LogicLong, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [388], [LogicCommandFieldType.LogicLong, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [389], [LogicCommandFieldType.OptionalLogicLong, LogicCommandFieldType.OptionalLogicLong, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [636], [LogicCommandFieldType.VarIntArray, LogicCommandFieldType.VarLongArray]);

        AddFieldCommands(entries, [134], [LogicCommandFieldType.LogicLong, LogicCommandFieldType.DataReference, LogicCommandFieldType.Boolean, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.Boolean, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [136], [LogicCommandFieldType.LogicLong, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt], isServerCommand: true, baseFirst: false);
        AddFieldCommands(entries, [643], [LogicCommandFieldType.String, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt, LogicCommandFieldType.VarInt]);

        var int32Schema = LogicCommandFieldSchema.Primitive(LogicCommandFieldType.Int32);
        var varIntSchema = LogicCommandFieldSchema.Primitive(LogicCommandFieldType.VarInt);
        var booleanSchema = LogicCommandFieldSchema.Primitive(LogicCommandFieldType.Boolean);
        var stringSchema = LogicCommandFieldSchema.Primitive(LogicCommandFieldType.String);
        var logicLongSchema = LogicCommandFieldSchema.Primitive(LogicCommandFieldType.LogicLong);
        var dataReferenceSchema = LogicCommandFieldSchema.Primitive(LogicCommandFieldType.DataReference);
        var varIntArraySchema = LogicCommandFieldSchema.Primitive(LogicCommandFieldType.VarIntArray);
        var byteArraySchema = LogicCommandFieldSchema.Primitive(LogicCommandFieldType.ByteArray);
        var optionalInt32PairSchema = LogicCommandFieldSchema.Optional(int32Schema, int32Schema);
        var optionalByteArraySchema = LogicCommandFieldSchema.Optional(byteArraySchema);

        var type148ElementSchema = LogicCommandFieldSchema.Array(
            nullable: false,
            dataReferenceSchema,
            booleanSchema,
            booleanSchema,
            LogicCommandFieldSchema.Array(nullable: false, dataReferenceSchema, varIntSchema));
        AddStructuredFieldCommands(
            entries,
            [148],
            [
                varIntSchema,
                LogicCommandFieldSchema.Optional(logicLongSchema),
                varIntArraySchema,
                varIntArraySchema,
                type148ElementSchema,
                dataReferenceSchema,
                int32Schema
            ],
            isServerCommand: true,
            baseFirst: false);

        AddStructuredFieldCommands(
            entries,
            [168],
            [
                varIntSchema,
                varIntSchema,
                varIntSchema,
                varIntSchema,
                LogicCommandFieldSchema.Optional(stringSchema),
                LogicCommandFieldSchema.Optional(
                    LogicCommandFieldSchema.Array(
                        nullable: true,
                        varIntSchema,
                        varIntSchema,
                        varIntSchema,
                        varIntSchema,
                        varIntSchema,
                        varIntSchema,
                        varIntSchema,
                        varIntSchema,
                        booleanSchema))
            ],
            isServerCommand: true,
            baseFirst: false);

        var type170NestedSchema = LogicCommandFieldSchema.Optional(
            LogicCommandFieldSchema.Optional(stringSchema),
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
            LogicCommandFieldSchema.Primitive(LogicCommandFieldType.StringArray),
            booleanSchema,
            varIntSchema,
            varIntSchema,
            varIntSchema,
            booleanSchema,
            varIntSchema);
        AddStructuredFieldCommands(
            entries,
            [170],
            [
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
                varIntArraySchema
            ],
            isServerCommand: true,
            baseFirst: false);

        AddStructuredFieldCommands(
            entries,
            [176],
            [
                LogicCommandFieldSchema.Array(
                    nullable: false,
                    stringSchema,
                    varIntSchema,
                    varIntSchema,
                    varIntSchema,
                    LogicCommandFieldSchema.Optional(stringSchema)),
                varIntSchema
            ],
            baseFirst: false);

        var types197To200NestedSchema = LogicCommandFieldSchema.Optional(
            stringSchema,
            varIntSchema,
            varIntSchema,
            varIntSchema,
            varIntSchema,
            booleanSchema,
            LogicCommandFieldSchema.Primitive(LogicCommandFieldType.VarLong),
            LogicCommandFieldSchema.Array(
                nullable: true,
                stringSchema,
                LogicCommandFieldSchema.Primitive(LogicCommandFieldType.VarLong),
                varIntSchema,
                varIntSchema,
                varIntSchema,
                varIntSchema,
                varIntSchema,
                varIntSchema,
                varIntSchema,
                varIntSchema),
            LogicCommandFieldSchema.Optional(stringSchema));
        AddStructuredFieldCommands(
            entries,
            [197],
            [stringSchema, stringSchema, stringSchema, varIntSchema, types197To200NestedSchema],
            isServerCommand: true,
            baseFirst: false);
        AddStructuredFieldCommands(
            entries,
            [198],
            [stringSchema, stringSchema, stringSchema, varIntSchema, types197To200NestedSchema, varIntSchema],
            isServerCommand: true,
            baseFirst: false);
        AddStructuredFieldCommands(
            entries,
            [200],
            [stringSchema, stringSchema, varIntSchema, types197To200NestedSchema],
            isServerCommand: true,
            baseFirst: false);

        var dataReferenceVarIntArraySchema = LogicCommandFieldSchema.Array(nullable: false, dataReferenceSchema, varIntSchema);
        AddStructuredFieldCommands(
            entries,
            [252],
            [stringSchema, stringSchema, varIntSchema, varIntSchema, dataReferenceVarIntArraySchema, dataReferenceVarIntArraySchema],
            isServerCommand: true,
            baseFirst: false);
        AddStructuredFieldCommands(
            entries,
            [253, 262],
            [stringSchema, stringSchema, varIntSchema, dataReferenceVarIntArraySchema, dataReferenceVarIntArraySchema],
            isServerCommand: true,
            baseFirst: false);
        AddStructuredFieldCommands(
            entries,
            [261],
            [stringSchema, stringSchema, varIntSchema, dataReferenceVarIntArraySchema, dataReferenceVarIntArraySchema, varIntSchema],
            isServerCommand: true,
            baseFirst: false);
        AddStructuredFieldCommands(
            entries,
            [328],
            [stringSchema, stringSchema, varIntSchema, dataReferenceVarIntArraySchema],
            isServerCommand: true,
            baseFirst: false);

        AddStructuredFieldCommands(
            entries,
            [256],
            [
                stringSchema,
                varIntSchema,
                LogicCommandFieldSchema.Array(
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
                    LogicCommandFieldSchema.Optional(stringSchema))
            ],
            isServerCommand: true,
            baseFirst: false);

        AddStructuredFieldCommands(
            entries,
            [263],
            [
                stringSchema,
                LogicCommandFieldSchema.Array(
                    nullable: false,
                    LogicCommandFieldSchema.Primitive(LogicCommandFieldType.DataReferenceArray),
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
                    LogicCommandFieldSchema.Optional(
                        LogicCommandFieldSchema.Optional(dataReferenceSchema, varIntSchema),
                        booleanSchema),
                    booleanSchema)
            ],
            isServerCommand: true,
            baseFirst: false);

        AddStructuredFieldCommands(entries, [687], [optionalInt32PairSchema, varIntSchema, varIntSchema]);
        AddStructuredFieldCommands(entries, [743], [varIntSchema, optionalByteArraySchema], isServerCommand: true);

        var dataReferenceVarIntPairArraySchema = LogicCommandFieldSchema.Primitive(LogicCommandFieldType.DataReferenceVarIntPairArray);
        AddStructuredFieldCommands(
            entries,
            [296],
            [
                logicLongSchema,
                varIntSchema,
                LogicCommandFieldSchema.Optional(
                    dataReferenceSchema,
                    varIntSchema,
                    varIntSchema,
                    dataReferenceVarIntPairArraySchema,
                    dataReferenceVarIntPairArraySchema,
                    stringSchema),
                LogicCommandFieldSchema.Optional(
                    LogicCommandFieldSchema.Optional(dataReferenceSchema, varIntSchema),
                    booleanSchema)
            ]);

        var types305And306Schema = LogicCommandFieldSchema.Optional(dataReferenceVarIntPairArraySchema, varIntSchema);
        AddStructuredFieldCommands(entries, [305], [types305And306Schema], isServerCommand: true, baseFirst: false);
        AddStructuredFieldCommands(entries, [306], [types305And306Schema], baseFirst: false);

        var type755NestedSchema = LogicCommandFieldSchema.Optional(
            LogicCommandFieldSchema.Primitive(LogicCommandFieldType.String),
            optionalByteArraySchema,
            varIntSchema,
            varIntSchema,
            varIntSchema,
            optionalInt32PairSchema,
            varIntSchema);
        var type755Schema = LogicCommandFieldSchema.Optional(
            varIntSchema,
            type755NestedSchema,
            type755NestedSchema,
            varIntSchema);
        AddStructuredFieldCommands(entries, [755], [type755Schema], isServerCommand: true);

        return entries;
    }

    private static void AddFieldCommands(Dictionary<int, Entry> entries, ReadOnlySpan<int> commandTypes, LogicCommandFieldType[] fieldTypes, bool isServerCommand = false, bool baseFirst = true)
    {
        AddStructuredFieldCommands(entries, commandTypes, fieldTypes.Select(LogicCommandFieldSchema.Primitive).ToArray(), isServerCommand, baseFirst);
    }

    private static void AddStructuredFieldCommands(Dictionary<int, Entry> entries, ReadOnlySpan<int> commandTypes, LogicCommandFieldSchema[] fieldSchemas, bool isServerCommand = false, bool baseFirst = true)
    {
        foreach (var type in commandTypes)
        {
            var commandType = type;
            entries.Add(commandType, new Entry(
                isServerCommand ? typeof(LogicServerCommandWithFields) : typeof(LogicCommandWithFields),
                isServerCommand,
                baseFirst,
                fieldSchemas,
                isServerCommand
                    ? (stream, environment, _) => LogicServerCommandWithFields.Decode(commandType, fieldSchemas, baseFirst, stream, environment)
                    : (stream, environment, _) => LogicCommandWithFields.Decode(commandType, fieldSchemas, baseFirst, stream, environment)));
        }
    }

    public static LogicCommand Decode(SupercellStream stream, LogicEnvironment environment, ILogicCommandDataResolver? dataResolver = null)
    {
        var commandType = stream.ReadVarInt();

        if (!_entries.TryGetValue(commandType, out var entry))
            throw new NotSupportedException($"Logic command type {commandType} is not supported.");

        if (environment is LogicEnvironment.Production && _nonProductionCommandTypes.Contains(commandType))
            throw new NotSupportedException($"Logic command type {commandType} is not allowed in the production environment.");

        return entry.Factory(stream, environment, dataResolver);
    }

    public static void Encode(SupercellStream stream, LogicCommand command, LogicEnvironment environment)
    {
        if (!_entries.TryGetValue(command.Type, out var entry) || !entry.Type.IsInstanceOfType(command))
            throw new NotSupportedException($"Logic command type {command.Type} is not supported.");

        if (environment is LogicEnvironment.Production && _nonProductionCommandTypes.Contains(command.Type))
            throw new NotSupportedException($"Logic command type {command.Type} is not allowed in the production environment.");

        stream.WriteVarInt(command.Type);
        command.EncodeBody(stream, environment);
    }

    internal static bool ValidateFields(int type, ReadOnlySpan<LogicCommandField> fields, bool isServerCommand)
    {
        if (!_entries.TryGetValue(type, out var entry) || entry.IsServerCommand != isServerCommand || entry.FieldSchemas is null)
            throw new NotSupportedException($"Logic command type {type} does not have a registered primitive field schema.");

        if (!LogicCommandFieldSchema.AreValid(entry.FieldSchemas, fields))
            throw new InvalidDataException($"Logic command type {type} fields do not match the registered native schema.");

        return entry.BaseFirst;
    }
}
