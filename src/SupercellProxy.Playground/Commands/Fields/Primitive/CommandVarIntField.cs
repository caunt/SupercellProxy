using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>CommandVarIntField</c>.
/// </summary>
public sealed record CommandVarIntField(int Value) : CommandField
{
    internal override CommandFieldType FieldType => CommandFieldType.VarInt;

    internal override void Encode(MessageStream stream) => stream.WriteVarInt(Value);
}
