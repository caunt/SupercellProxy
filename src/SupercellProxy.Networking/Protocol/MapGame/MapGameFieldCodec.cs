using System.Globalization;

using SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// Defines the Map Game Wire contract.
/// </summary>
public static class MapGameFieldCodec
{
    /// <summary>
    /// Provides the Read Count value or operation.
    /// </summary>
    public static int ReadCount(MessageStream stream, string name)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int count = stream.ReadVarInt();

        return count < 0 || count > stream.Length - stream.Position
            ? throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Invalid map-game {name} count: {count}."))
            : count;
    }

    /// <summary>
    /// Provides the Read Data Reference Var Int Pairs value or operation.
    /// </summary>
    public static CommandDataReferenceVarIntPair[] ReadDataReferenceVarIntPairs(MessageStream stream)
    {
        return CommandDataReferenceVarIntPairArrayField.Decode(stream).Values.ToArray();
    }

    /// <summary>
    /// Provides the Read Long Ids value or operation.
    /// </summary>
    public static LongId[] ReadLongIds(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int count = ReadCount(stream, name: "logic-long");
        LongId[] values = new LongId[count];

        for (int index = 0; index < values.Length; index++)
            values[index] = stream.ReadLongId();

        return values;
    }

    /// <summary>
    /// Provides the Read Optional Data Reference Var Int Pairs value or operation.
    /// </summary>
    public static ReadOnlyMemory<CommandDataReferenceVarIntPair>? ReadOptionalDataReferenceVarIntPairs(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return !stream.ReadBoolean() ? null : (ReadOnlyMemory<CommandDataReferenceVarIntPair>?)ReadDataReferenceVarIntPairs(stream);
    }

    /// <summary>
    /// Provides the Read Optional Long Id value or operation.
    /// </summary>
    public static LongId? ReadOptionalLongId(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return stream.ReadBoolean() ? stream.ReadLongId() : null;
    }

    /// <summary>
    /// Provides the Write Data Reference Var Int Pairs value or operation.
    /// </summary>
    public static void WriteDataReferenceVarIntPairs(MessageStream stream, ReadOnlySpan<CommandDataReferenceVarIntPair> values)
    {
        new CommandDataReferenceVarIntPairArrayField(values.ToArray()).Encode(stream);
    }

    /// <summary>
    /// Provides the Write Long Ids value or operation.
    /// </summary>
    public static void WriteLongIds(MessageStream stream, ReadOnlySpan<LongId> values)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(values.Length);

        foreach (LongId value in values)
            stream.WriteLongId(value);
    }

    /// <summary>
    /// Provides the Write Optional Data Reference Var Int Pairs value or operation.
    /// </summary>
    public static void WriteOptionalDataReferenceVarIntPairs(MessageStream stream, ReadOnlyMemory<CommandDataReferenceVarIntPair>? values)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteBoolean(values is not null);

        if (values is not null)
            WriteDataReferenceVarIntPairs(stream, values.Value.Span);
    }

    /// <summary>
    /// Provides the Write Optional Long Id value or operation.
    /// </summary>
    public static void WriteOptionalLongId(MessageStream stream, LongId? value)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteBoolean(value is not null);

        if (value is not null)
            stream.WriteLongId(value.Value);
    }
}
