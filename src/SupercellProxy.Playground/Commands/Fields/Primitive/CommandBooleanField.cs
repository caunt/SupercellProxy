using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">CommandBooleanField</c>.
/// </summary>
internal sealed record CommandBooleanField(bool Value) : CommandField
{
    internal override CommandFieldType FieldType => CommandFieldType.Boolean;

    internal override void Encode(MessageStream stream) => stream.WriteBoolean(Value);
}
