using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>CommandUInt16Field</c>.
/// </summary>
public sealed record CommandUInt16Field(ushort Value) : CommandField
{
    internal override CommandFieldType FieldType => CommandFieldType.UInt16;

    internal override void Encode(MessageStream stream) => stream.WriteUInt16(Value);
}
