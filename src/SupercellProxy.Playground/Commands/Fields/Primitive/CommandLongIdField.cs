using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>CommandLongIdField</c>.
/// </summary>
public sealed record CommandLongIdField(LongId Value) : CommandField
{
    internal override CommandFieldType FieldType => CommandFieldType.LongId;

    internal override void Encode(MessageStream stream) => stream.WriteLongId(Value);
}
