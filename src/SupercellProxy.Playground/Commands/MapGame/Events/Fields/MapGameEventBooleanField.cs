using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>MapGameEventBooleanField</c>.
/// </summary>
public sealed record MapGameEventBooleanField(bool Value) : MapGameEventField
{
    internal override MapGameEventFieldType FieldType => MapGameEventFieldType.Boolean;

    internal override void Encode(MessageStream stream) => stream.WriteBoolean(Value);
}
