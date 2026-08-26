using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>CommandInt32Field</c>.
/// </summary>
public sealed record CommandInt32Field(int Value) : CommandField
{
    internal override CommandFieldType FieldType => CommandFieldType.Int32;

    internal override void Encode(MessageStream stream) => stream.WriteInt32(Value);
}
