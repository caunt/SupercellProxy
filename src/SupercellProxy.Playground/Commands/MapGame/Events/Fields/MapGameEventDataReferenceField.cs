using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>MapGameEventDataReferenceField</c>.
/// </summary>
public sealed record MapGameEventDataReferenceField(int GlobalId, int ExpectedTableId = -1)
    : MapGameEventField
{
    internal override MapGameEventFieldType FieldType => MapGameEventFieldType.DataReference;

    internal override void Encode(MessageStream stream) => stream.WriteVarInt(GlobalId);
}
