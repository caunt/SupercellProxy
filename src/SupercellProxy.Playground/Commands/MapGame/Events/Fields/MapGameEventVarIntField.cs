using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">MapGameEventVarIntField</c>.
/// </summary>
internal sealed record MapGameEventVarIntField(int Value) : MapGameEventField
{
    internal override MapGameEventFieldType FieldType => MapGameEventFieldType.VarInt;

    internal override void Encode(MessageStream stream) => stream.WriteVarInt(Value);
}
