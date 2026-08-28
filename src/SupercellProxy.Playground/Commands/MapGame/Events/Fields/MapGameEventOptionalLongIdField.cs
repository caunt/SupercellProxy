using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">MapGameEventOptionalLongIdField</c>.
/// </summary>
internal sealed record MapGameEventOptionalLongIdField(LongId? Value) : MapGameEventField
{
    internal override MapGameEventFieldType FieldType => MapGameEventFieldType.OptionalLongId;

    internal override void Encode(MessageStream stream) =>
        MapGameWire.WriteOptionalLongId(stream, Value);
}
