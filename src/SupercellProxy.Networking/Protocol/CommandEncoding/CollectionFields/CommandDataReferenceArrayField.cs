using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;

/// <summary>
/// Represents <c language="csharp">CommandDataReferenceArrayField</c>.
/// </summary>
public sealed record CommandDataReferenceArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandDataReferenceArrayField"/> instance.
    /// </summary>
    public CommandDataReferenceArrayField(ReadOnlyMemory<int> globalIdentifiers)
    {
        GlobalIdentifiers = globalIdentifiers.ToArray();
    }

    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.DataReferenceArray;

    /// <summary>
    /// Gets the <c language="csharp">GlobalIds</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("GlobalIds")]
    public ReadOnlyMemory<int> GlobalIdentifiers { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CommandDataReferenceArrayField Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(CommandVariableIntArrayField.DecodeValues(stream.ReadVariableInt(), stream));
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteVariableInt(GlobalIdentifiers.Length);

        foreach (int globalIdentifier in GlobalIdentifiers.Span)
            stream.WriteVariableInt(globalIdentifier);
    }
}
