using SupercellProxy.Networking.Protocol.MapGame.Tasks;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Events.Fields;

/// <summary>
/// Represents <c language="csharp">MapGameEventOptionalDumpTaskStateField</c>.
/// </summary>
public sealed record MapGameEventOptionalDumpTaskStateField(MapGameDumpTaskStatePayload? Value)
    : MapGameEventField
{
    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override MapGameEventFieldType FieldType => MapGameEventFieldType.OptionalDumpTaskState;

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteBoolean(Value is not null);
        Value?.Encode(stream);
    }
}
