using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Events.Fields;

/// <summary>
/// Represents <c language="csharp">MapGameEventDataReferenceField</c>.
/// </summary>
public sealed record MapGameEventDataReferenceField(
    [property: System.Text.Json.Serialization.JsonPropertyName("GlobalId")] int GlobalIdentifier,
    [property: System.Text.Json.Serialization.JsonPropertyName("ExpectedTableId")] int ExpectedTableIdentifier = -1
)
    : MapGameEventField
{
    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override MapGameEventFieldType FieldType => MapGameEventFieldType.DataReference;

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteVariableInt(GlobalIdentifier);
    }
}
