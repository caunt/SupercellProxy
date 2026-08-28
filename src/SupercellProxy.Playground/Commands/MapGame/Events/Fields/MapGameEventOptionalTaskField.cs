using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">MapGameEventOptionalTaskField</c>.
/// </summary>
internal sealed record MapGameEventOptionalTaskField(MapGameTask? Value) : MapGameEventField
{
    internal override MapGameEventFieldType FieldType => MapGameEventFieldType.OptionalTask;

    internal override void Encode(MessageStream stream)
    {
        stream.WriteBoolean(Value is not null);
        Value?.Encode(stream);
    }
}
