using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Events;

/// <summary>
/// <para>Native text-or-binary structure used by map-game event 39.</para>
/// </summary>
public sealed record MapGameEventProfileData
{
    /// <summary>
    /// Defines the <c language="csharp">UnknownValueCount</c> value.
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
            throw new InvalidDataException(message: "A binary map-game event profile cannot contain optional text data.");

        if (!usesBinaryData && binaryData is not null)
            throw new InvalidDataException(message: "A text map-game event profile cannot contain binary data.");

        if (unknownValues.Length != UnknownValueCount)
            throw new InvalidDataException($"A map-game event profile must contain exactly {UnknownValueCount} trailing values.");

        UsesBinaryData = usesBinaryData;
        BinaryData = binaryData is null ? null : (ReadOnlyMemory<byte>?)binaryData.Value.ToArray();
        OptionalTextData = optionalTextData;
        Unknown0 = unknown0;
        Unknown1 = unknown1;
        UnknownString0 = unknownString0;
        UnknownValues = unknownValues.ToArray();
        UnknownString1 = unknownString1;
    }

    /// <summary>
    /// Gets the <c language="csharp">BinaryData</c> value.
    /// </summary>
    public ReadOnlyMemory<byte>? BinaryData { get; }

    /// <summary>
    /// Gets the <c language="csharp">OptionalTextData</c> value.
    /// </summary>
    public string? OptionalTextData { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public int Unknown1 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownString0</c> value.
    /// </summary>
    public string UnknownString0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownString1</c> value.
    /// </summary>
    public string UnknownString1 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownValues</c> value.
    /// </summary>
    public ReadOnlyMemory<int> UnknownValues { get; }

    /// <summary>
    /// Gets the <c language="csharp">UsesBinaryData</c> value.
    /// </summary>
    public bool UsesBinaryData { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameEventProfileData Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        bool usesBinaryData = stream.ReadBoolean();
        Memory<byte>? binaryData = usesBinaryData ? stream.ReadOptionalByteArray() : null;
        string? optionalTextData = usesBinaryData ? null : stream.ReadOptionalString();
        int unknown0 = stream.ReadVariableInt();
        int unknown1 = stream.ReadVariableInt();
        string unknownString0 = stream.ReadString();
        int[] unknownValues = new int[UnknownValueCount];

        for (int index = 0; index < unknownValues.Length; index++)
            unknownValues[index] = stream.ReadVariableInt();

        return new MapGameEventProfileData(usesBinaryData, binaryData, optionalTextData, unknown0, unknown1, unknownString0, unknownValues, stream.ReadString());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteBoolean(UsesBinaryData);

        if (UsesBinaryData)
            stream.WriteOptionalByteArray(BinaryData);
        else
            stream.WriteOptionalString(OptionalTextData);

        stream.WriteVariableInt(Unknown0);
        stream.WriteVariableInt(Unknown1);
        stream.WriteString(UnknownString0);

        foreach (int value in UnknownValues.Span)
            stream.WriteVariableInt(value);

        stream.WriteString(UnknownString1);
    }
}
