using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>MapGameEventOptionalDumpTaskStateField</c>.
/// </summary>
public sealed record MapGameEventOptionalDumpTaskStateField(MapGameDumpTaskStatePayload? Value)
    : MapGameEventField
{
    internal override MapGameEventFieldType FieldType =>
        MapGameEventFieldType.OptionalDumpTaskState;

    internal override void Encode(MessageStream stream)
    {
        stream.WriteBoolean(Value is not null);
        Value?.Encode(stream);
    }
}
