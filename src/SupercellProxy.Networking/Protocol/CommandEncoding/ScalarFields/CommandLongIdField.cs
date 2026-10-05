using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding.ScalarFields;

/// <summary>
/// Represents <c language="csharp">CommandLongIdField</c>.
/// </summary>
public sealed record CommandLongIdField(LongId Value) : CommandField
{
    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.LongId;

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteLongId(Value);
    }
}
