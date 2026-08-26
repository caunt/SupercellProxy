using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Native text-or-binary structure used by map-game event 39.</para>
/// </summary>
public sealed record MapGameEventProfileData
{
    /// <summary>
    /// Defines the <c>UnknownValueCount</c> value.
    /// </summary>
    public const int UnknownValueCount = 11;

    /// <summary>
    /// Initializes a new <see cref="MapGameEventProfileData"/> instance.
    /// </summary>
    public MapGameEventProfileData(
        bool usesBinaryData,
        ReadOnlyMemory<byte>? binaryData,
        string? optionalTextData,
        int unknown0,
        int unknown1,
        string unknownString0,
        ReadOnlyMemory<int> unknownValues,
        string unknownString1
    )
    {
        if (usesBinaryData && optionalTextData is not null)
            throw new InvalidDataException(
                "A binary map-game event profile cannot contain optional text data."
            );

        if (!usesBinaryData && binaryData is not null)
            throw new InvalidDataException(
                "A text map-game event profile cannot contain binary data."
            );

        if (unknownValues.Length != UnknownValueCount)
            throw new InvalidDataException(
                $"A map-game event profile must contain exactly {UnknownValueCount} trailing values."
            );

        UsesBinaryData = usesBinaryData;
        BinaryData = binaryData?.ToArray();
        OptionalTextData = optionalTextData;
        Unknown0 = unknown0;
        Unknown1 = unknown1;
        UnknownString0 = unknownString0;
        UnknownValues = unknownValues.ToArray();
        UnknownString1 = unknownString1;
    }

    /// <summary>
    /// Gets the <c>UsesBinaryData</c> value.
    /// </summary>
    public bool UsesBinaryData { get; }

    /// <summary>
    /// Gets the <c>BinaryData</c> value.
    /// </summary>
    public ReadOnlyMemory<byte>? BinaryData { get; }

    /// <summary>
    /// Gets the <c>OptionalTextData</c> value.
    /// </summary>
    public string? OptionalTextData { get; }

    /// <summary>
    /// Gets the <c>Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; }

    /// <summary>
    /// Gets the <c>Unknown1</c> value.
    /// </summary>
    public int Unknown1 { get; }

    /// <summary>
    /// Gets the <c>UnknownString0</c> value.
    /// </summary>
    public string UnknownString0 { get; }

    /// <summary>
    /// Gets the <c>UnknownValues</c> value.
    /// </summary>
    public ReadOnlyMemory<int> UnknownValues { get; }

    /// <summary>
    /// Gets the <c>UnknownString1</c> value.
    /// </summary>
    public string UnknownString1 { get; }

    internal static MapGameEventProfileData Decode(MessageStream stream)
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

        return new MapGameEventProfileData(
            usesBinaryData,
            binaryData,
            optionalTextData,
            unknown0,
            unknown1,
            unknownString0,
            unknownValues,
            stream.ReadString()
        );
    }

    internal void Encode(MessageStream stream)
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
