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
        int count = stream.ReadVariableInt();

        return count < 0 || count > stream.Length - stream.Position
            ? throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Invalid map-game {name} count: {count}."))
            : count;
    }

    /// <summary>
    /// Provides the Read Data Reference Var Int Pairs value or operation.
    /// </summary>
    public static CommandDataReferenceVariableIntPair[] ReadDataReferenceVariableIntPairs(MessageStream stream)
    {
        return CommandDataReferenceVariableIntPairArrayField.Decode(stream).Values.ToArray();
    }

    /// <summary>
    /// Provides the Read Long Ids value or operation.
    /// </summary>
    public static LongIdentifier[] ReadLongIdentifiers(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int count = ReadCount(stream, name: "logic-long");
        LongIdentifier[] values = new LongIdentifier[count];

        for (int index = 0; index < values.Length; index++)
            values[index] = stream.ReadLongIdentifier();

        return values;
    }

    /// <summary>
    /// Provides the Read Optional Data Reference Var Int Pairs value or operation.
    /// </summary>
    public static CommandDataReferenceVariableIntPair[]? ReadOptionalDataReferenceVariableIntPairs(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return stream.ReadBoolean() ? ReadDataReferenceVariableIntPairs(stream) : null;
    }

    /// <summary>
    /// Provides the Read Optional Long Id value or operation.
    /// </summary>
    public static LongIdentifier? ReadOptionalLongIdentifier(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return stream.ReadBoolean() ? stream.ReadLongIdentifier() : null;
    }

    /// <summary>
    /// Provides the Write Data Reference Var Int Pairs value or operation.
    /// </summary>
    public static void WriteDataReferenceVariableIntPairs(MessageStream stream, ReadOnlySpan<CommandDataReferenceVariableIntPair> values)
    {
        new CommandDataReferenceVariableIntPairArrayField(values.ToArray()).Encode(stream);
    }

    /// <summary>
    /// Provides the Write Long Ids value or operation.
    /// </summary>
    public static void WriteLongIdentifiers(MessageStream stream, ReadOnlySpan<LongIdentifier> values)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(values.Length);

        foreach (LongIdentifier value in values)
            stream.WriteLongIdentifier(value);
    }

    /// <summary>
    /// Provides the Write Optional Data Reference Var Int Pairs value or operation.
    /// </summary>
    public static void WriteOptionalDataReferenceVariableIntPairs(MessageStream stream, ReadOnlyMemory<CommandDataReferenceVariableIntPair>? values)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteBoolean(values is not null);

        if (values is not null)
            WriteDataReferenceVariableIntPairs(stream, values.Value.Span);
    }

    /// <summary>
    /// Provides the Write Optional Long Id value or operation.
    /// </summary>
    public static void WriteOptionalLongIdentifier(MessageStream stream, LongIdentifier? value)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteBoolean(value is not null);

        if (value is not null)
            stream.WriteLongIdentifier(value.Value);
    }
}
