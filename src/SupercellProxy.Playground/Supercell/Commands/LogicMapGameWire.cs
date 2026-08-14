using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

internal static class LogicMapGameWire
{
    internal static LogicCommandDataReferenceVarIntPair[] ReadDataReferenceVarIntPairs(SupercellStream stream)
    {
        return LogicCommandDataReferenceVarIntPairArrayField.Decode(stream).Values.ToArray();
    }

    internal static void WriteDataReferenceVarIntPairs(SupercellStream stream, ReadOnlySpan<LogicCommandDataReferenceVarIntPair> values)
    {
        new LogicCommandDataReferenceVarIntPairArrayField(values.ToArray()).Encode(stream);
    }

    internal static LogicCommandDataReferenceVarIntPair[]? ReadOptionalDataReferenceVarIntPairs(SupercellStream stream)
    {
        return stream.ReadBoolean() ? ReadDataReferenceVarIntPairs(stream) : null;
    }

    internal static void WriteOptionalDataReferenceVarIntPairs(SupercellStream stream, ReadOnlyMemory<LogicCommandDataReferenceVarIntPair>? values)
    {
        stream.WriteBoolean(values is not null);

        if (values is not null)
            WriteDataReferenceVarIntPairs(stream, values.Value.Span);
    }

    internal static LogicLong? ReadOptionalLogicLong(SupercellStream stream)
    {
        return stream.ReadBoolean() ? stream.ReadLogicLong() : null;
    }

    internal static void WriteOptionalLogicLong(SupercellStream stream, LogicLong? value)
    {
        stream.WriteBoolean(value is not null);

        if (value is not null)
            stream.WriteLogicLong(value.Value);
    }

    internal static LogicLong[] ReadLogicLongs(SupercellStream stream)
    {
        var count = ReadCount(stream, "logic-long");
        var values = new LogicLong[count];

        for (var i = 0; i < values.Length; i++)
            values[i] = stream.ReadLogicLong();

        return values;
    }

    internal static void WriteLogicLongs(SupercellStream stream, ReadOnlySpan<LogicLong> values)
    {
        stream.WriteVarInt(values.Length);

        foreach (var value in values)
            stream.WriteLogicLong(value);
    }

    internal static int ReadCount(SupercellStream stream, string name)
    {
        var count = stream.ReadVarInt();

        if (count < 0 || count > stream.Length - stream.Position)
            throw new InvalidDataException($"Invalid map-game {name} count: {count}.");

        return count;
    }
}
