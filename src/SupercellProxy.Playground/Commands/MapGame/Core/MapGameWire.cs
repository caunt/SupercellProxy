using System.Globalization;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

internal static class MapGameWire
{
    internal static CommandDataReferenceVarIntPair[] ReadDataReferenceVarIntPairs(
        MessageStream stream
    )
    {
        return CommandDataReferenceVarIntPairArrayField.Decode(stream).Values.ToArray();
    }

    internal static void WriteDataReferenceVarIntPairs(
        MessageStream stream,
        ReadOnlySpan<CommandDataReferenceVarIntPair> values
    )
    {
        new CommandDataReferenceVarIntPairArrayField(values.ToArray()).Encode(stream);
    }

    internal static CommandDataReferenceVarIntPair[]? ReadOptionalDataReferenceVarIntPairs(
        MessageStream stream
    )
    {
        return stream.ReadBoolean() ? ReadDataReferenceVarIntPairs(stream) : null;
    }

    internal static void WriteOptionalDataReferenceVarIntPairs(
        MessageStream stream,
        ReadOnlyMemory<CommandDataReferenceVarIntPair>? values
    )
    {
        stream.WriteBoolean(values is not null);

        if (values is not null)
            WriteDataReferenceVarIntPairs(stream, values.Value.Span);
    }

    internal static LongId? ReadOptionalLongId(MessageStream stream)
    {
        return stream.ReadBoolean() ? stream.ReadLongId() : null;
    }

    internal static void WriteOptionalLongId(MessageStream stream, LongId? value)
    {
        stream.WriteBoolean(value is not null);

        if (value is not null)
            stream.WriteLongId(value.Value);
    }

    internal static LongId[] ReadLongIds(MessageStream stream)
    {
        var count = ReadCount(stream, "logic-long");
        var values = new LongId[count];

        for (var i = 0; i < values.Length; i++)
            values[i] = stream.ReadLongId();

        return values;
    }

    internal static void WriteLongIds(MessageStream stream, ReadOnlySpan<LongId> values)
    {
        stream.WriteVarInt(values.Length);

        foreach (var value in values)
            stream.WriteLongId(value);
    }

    internal static int ReadCount(MessageStream stream, string name)
    {
        var count = stream.ReadVarInt();

        if (count < 0 || count > stream.Length - stream.Position)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Invalid map-game {name} count: {count}."
                )
            );

        return count;
    }
}
