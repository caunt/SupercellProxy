using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Events.Fields;

/// <summary>
/// Represents <c language="csharp">MapGameEventOptionalLongIdField</c>.
/// </summary>
public sealed record MapGameEventOptionalLongIdentifierField(LongIdentifier? Value) : MapGameEventField
{
    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override MapGameEventFieldType FieldType => MapGameEventFieldType.OptionalLongIdentifier;

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        MapGameFieldCodec.WriteOptionalLongIdentifier(stream, Value);
    }
}
