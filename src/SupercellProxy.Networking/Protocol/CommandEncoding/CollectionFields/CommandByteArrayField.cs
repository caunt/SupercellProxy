using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;

/// <summary>
/// Represents <c language="csharp">CommandByteArrayField</c>.
/// </summary>
public sealed record CommandByteArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandByteArrayField"/> instance.
    /// </summary>
    public CommandByteArrayField(ReadOnlyMemory<byte> value)
    {
        Value = value.ToArray();
    }

    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.ByteArray;

    /// <summary>
    /// Gets the <c language="csharp">Value</c> value.
    /// </summary>
    public ReadOnlyMemory<byte> Value { get; }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteByteArray(Value.Span);
    }
}
