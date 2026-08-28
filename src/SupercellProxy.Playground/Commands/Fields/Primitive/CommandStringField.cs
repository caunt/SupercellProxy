using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">CommandStringField</c>.
/// </summary>
internal sealed record CommandStringField(string Value) : CommandField
{
    internal override CommandFieldType FieldType => CommandFieldType.String;

    internal override void Encode(MessageStream stream) => stream.WriteString(Value);
}
