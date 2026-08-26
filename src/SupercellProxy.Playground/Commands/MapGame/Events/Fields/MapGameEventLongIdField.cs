using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>MapGameEventLongIdField</c>.
/// </summary>
public sealed record MapGameEventLongIdField(LongId Value) : MapGameEventField
{
    internal override MapGameEventFieldType FieldType => MapGameEventFieldType.LongId;

    internal override void Encode(MessageStream stream) => stream.WriteLongId(Value);
}
