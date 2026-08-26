using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>CommandVarLongField</c>.
/// </summary>
public sealed record CommandVarLongField(long Value) : CommandField
{
    internal override CommandFieldType FieldType => CommandFieldType.VarLong;

    internal override void Encode(MessageStream stream) => stream.WriteVarLong(Value);
}
