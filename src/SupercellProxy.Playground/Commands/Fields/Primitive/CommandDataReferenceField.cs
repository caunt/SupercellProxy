using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">CommandDataReferenceField</c>.
/// </summary>
internal sealed record CommandDataReferenceField(int GlobalId) : CommandField
{
    internal override CommandFieldType FieldType => CommandFieldType.DataReference;

    internal override void Encode(MessageStream stream) => stream.WriteVarInt(GlobalId);
}
