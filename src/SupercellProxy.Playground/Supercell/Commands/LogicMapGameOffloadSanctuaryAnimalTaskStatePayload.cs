using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

public sealed record LogicMapGameOffloadSanctuaryAnimalTaskStatePayload(int Unknown0) : LogicMapGameTaskStatePayload
{
    internal static LogicMapGameOffloadSanctuaryAnimalTaskStatePayload Decode(SupercellStream stream)
    {
        return new LogicMapGameOffloadSanctuaryAnimalTaskStatePayload(stream.ReadVarInt());
    }

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Unknown0);
    }
}
