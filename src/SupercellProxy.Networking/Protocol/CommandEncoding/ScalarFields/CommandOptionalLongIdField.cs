using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding.ScalarFields;

/// <summary>
/// Represents <c language="csharp">CommandOptionalLongIdField</c>.
/// </summary>
public sealed record CommandOptionalLongIdField(LongId? Value) : CommandField
{
    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.OptionalLongId;

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteBoolean(Value is not null);

        if (Value is not null)
            stream.WriteLongId(Value.Value);
    }
}
