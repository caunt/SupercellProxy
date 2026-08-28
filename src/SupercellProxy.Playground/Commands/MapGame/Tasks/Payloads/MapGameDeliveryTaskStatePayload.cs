using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c language="csharp">MapGameDeliveryTaskStatePayload</c>.
/// </summary>
internal sealed record MapGameDeliveryTaskStatePayload(
    int Unknown0,
    int Unknown1,
    bool UnknownBoolean0
) : MapGameTaskStatePayload
{
    internal static MapGameDeliveryTaskStatePayload Decode(MessageStream stream)
    {
        return new MapGameDeliveryTaskStatePayload(
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadBoolean()
        );
    }

    internal override void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteBoolean(UnknownBoolean0);
    }
}
