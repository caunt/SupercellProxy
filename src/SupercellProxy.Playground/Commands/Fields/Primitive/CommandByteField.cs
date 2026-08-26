using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>CommandByteField</c>.
/// </summary>
public sealed record CommandByteField(sbyte Value) : CommandField
{
    internal override CommandFieldType FieldType => CommandFieldType.Byte;

    internal override void Encode(MessageStream stream) =>
        stream.WriteByte(unchecked(byte.CreateTruncating(Value)));
}
