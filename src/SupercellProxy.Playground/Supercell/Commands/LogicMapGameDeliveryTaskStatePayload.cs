using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

public sealed record LogicMapGameDeliveryTaskStatePayload(int Unknown0, int Unknown1, bool UnknownBoolean0) : LogicMapGameTaskStatePayload
{
    internal static LogicMapGameDeliveryTaskStatePayload Decode(SupercellStream stream)
    {
        return new LogicMapGameDeliveryTaskStatePayload(stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadBoolean());
    }

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteBoolean(UnknownBoolean0);
    }
}
