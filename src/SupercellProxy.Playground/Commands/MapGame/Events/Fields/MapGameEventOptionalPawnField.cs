using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">MapGameEventOptionalPawnField</c>.
/// </summary>
internal sealed record MapGameEventOptionalPawnField(MapGamePawn? Value) : MapGameEventField
{
    internal override MapGameEventFieldType FieldType => MapGameEventFieldType.OptionalPawn;

    internal override void Encode(MessageStream stream)
    {
        stream.WriteBoolean(Value is not null);
        Value?.Encode(stream);
    }
}
