using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>MapGameEventVarIntField</c>.
/// </summary>
public sealed record MapGameEventVarIntField(int Value) : MapGameEventField
{
    internal override MapGameEventFieldType FieldType => MapGameEventFieldType.VarInt;

    internal override void Encode(MessageStream stream) => stream.WriteVarInt(Value);
}
