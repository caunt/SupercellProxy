using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">CommandLongIdField</c>.
/// </summary>
internal sealed record CommandLongIdField(LongId Value) : CommandField
{
    internal override CommandFieldType FieldType => CommandFieldType.LongId;

    internal override void Encode(MessageStream stream) => stream.WriteLongId(Value);
}
