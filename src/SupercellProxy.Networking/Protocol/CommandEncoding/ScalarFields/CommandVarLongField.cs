using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding.ScalarFields;

/// <summary>
/// Represents <c language="csharp">CommandVarLongField</c>.
/// </summary>
public sealed record CommandVarLongField(long Value) : CommandField
{
    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.VarLong;

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteVarLong(Value);
    }
}
