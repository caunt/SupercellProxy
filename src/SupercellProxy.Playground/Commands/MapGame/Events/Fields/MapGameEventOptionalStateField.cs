using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">MapGameEventOptionalStateField</c>.
/// </summary>
internal sealed record MapGameEventOptionalStateField(MapGameState? Value) : MapGameEventField
{
    internal override MapGameEventFieldType FieldType => MapGameEventFieldType.OptionalState;

    internal override void Encode(MessageStream stream)
    {
        stream.WriteBoolean(Value is not null);
        Value?.Encode(stream);
    }
}
