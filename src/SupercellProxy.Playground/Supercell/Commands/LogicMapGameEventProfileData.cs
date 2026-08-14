using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Native text-or-binary structure used by map-game event 39.
/// </summary>
public sealed record LogicMapGameEventProfileData
{
    public const int UnknownValueCount = 11;

    public LogicMapGameEventProfileData(
        bool usesBinaryData,
        ReadOnlyMemory<byte>? binaryData,
        string? optionalTextData,
        int unknown0,
        int unknown1,
        string unknownString0,
        ReadOnlyMemory<int> unknownValues,
        string unknownString1)
    {
        if (usesBinaryData && optionalTextData is not null)
            throw new InvalidDataException("A binary map-game event profile cannot contain optional text data.");

        if (!usesBinaryData && binaryData is not null)
            throw new InvalidDataException("A text map-game event profile cannot contain binary data.");

        if (unknownValues.Length != UnknownValueCount)
            throw new InvalidDataException($"A map-game event profile must contain exactly {UnknownValueCount} trailing values.");

        UsesBinaryData = usesBinaryData;
        BinaryData = binaryData?.ToArray();
        OptionalTextData = optionalTextData;
        Unknown0 = unknown0;
        Unknown1 = unknown1;
        UnknownString0 = unknownString0;
        UnknownValues = unknownValues.ToArray();
        UnknownString1 = unknownString1;
    }

    public bool UsesBinaryData { get; }
    public ReadOnlyMemory<byte>? BinaryData { get; }
    public string? OptionalTextData { get; }
    public int Unknown0 { get; }
    public int Unknown1 { get; }
    public string UnknownString0 { get; }
    public ReadOnlyMemory<int> UnknownValues { get; }
    public string UnknownString1 { get; }

    internal static LogicMapGameEventProfileData Decode(SupercellStream stream)
    {
        var usesBinaryData = stream.ReadBoolean();
        var binaryData = usesBinaryData ? stream.ReadOptionalByteArray() : null;
        var optionalTextData = usesBinaryData ? null : stream.ReadOptionalString();
        var unknown0 = stream.ReadVarInt();
        var unknown1 = stream.ReadVarInt();
        var unknownString0 = stream.ReadString();
        var unknownValues = new int[UnknownValueCount];

        for (var i = 0; i < unknownValues.Length; i++)
            unknownValues[i] = stream.ReadVarInt();

        return new LogicMapGameEventProfileData(
            usesBinaryData,
            binaryData,
            optionalTextData,
            unknown0,
            unknown1,
            unknownString0,
            unknownValues,
            stream.ReadString());
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteBoolean(UsesBinaryData);

        if (UsesBinaryData)
            stream.WriteOptionalByteArray(BinaryData);
        else
            stream.WriteOptionalString(OptionalTextData);

        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteString(UnknownString0);

        foreach (var value in UnknownValues.Span)
            stream.WriteVarInt(value);

        stream.WriteString(UnknownString1);
    }
}
