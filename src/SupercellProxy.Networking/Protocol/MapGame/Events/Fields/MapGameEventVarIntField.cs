using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Events.Fields;

/// <summary>
/// Represents <c language="csharp">MapGameEventVarIntField</c>.
/// </summary>
public sealed record MapGameEventVarIntField(int Value) : MapGameEventField
{
    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override MapGameEventFieldType FieldType => MapGameEventFieldType.VarInt;

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Value);
    }
}
