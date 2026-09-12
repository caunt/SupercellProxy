using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding.ScalarFields;

/// <summary>
/// Represents <c language="csharp">CommandDataReferenceField</c>.
/// </summary>
public sealed record CommandDataReferenceField([property: System.Text.Json.Serialization.JsonPropertyName("GlobalId")] int GlobalIdentifier) : CommandField
{
    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.DataReference;

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteVariableInt(GlobalIdentifier);
    }
}
