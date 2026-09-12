using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding.ScalarFields;

/// <summary>
/// Represents <c language="csharp">CommandByteField</c>.
/// </summary>
public sealed record CommandByteField(sbyte Value) : CommandField
{
    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.Byte;

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteByte(unchecked(byte.CreateTruncating(Value)));
    }
}
