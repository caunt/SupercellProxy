using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>MapGameEventOptionalTaskCollectionField</c>.
/// </summary>
public sealed record MapGameEventOptionalTaskCollectionField(MapGameTaskCollection? Value)
    : MapGameEventField
{
    internal override MapGameEventFieldType FieldType =>
        MapGameEventFieldType.OptionalTaskCollection;

    internal override void Encode(MessageStream stream)
    {
        stream.WriteBoolean(Value is not null);
        Value?.Encode(stream);
    }
}
