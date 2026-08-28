using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">MapGameEventByteField</c>.
/// </summary>
internal sealed record MapGameEventByteField(sbyte Value) : MapGameEventField
{
    internal override MapGameEventFieldType FieldType => MapGameEventFieldType.Byte;

    internal override void Encode(MessageStream stream) =>
        stream.WriteByte(unchecked(byte.CreateTruncating(Value)));
}
