using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Events.Fields;

/// <summary>
/// Represents <c language="csharp">MapGameEventOptionalLongIdField</c>.
/// </summary>
public sealed record MapGameEventOptionalLongIdField(LongId? Value) : MapGameEventField
{
    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override MapGameEventFieldType FieldType => MapGameEventFieldType.OptionalLongId;

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        MapGameFieldCodec.WriteOptionalLongId(stream, Value);
    }
}
