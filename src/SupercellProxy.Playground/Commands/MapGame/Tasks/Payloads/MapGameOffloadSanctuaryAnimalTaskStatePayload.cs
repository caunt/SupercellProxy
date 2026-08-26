using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Represents <c>MapGameOffloadSanctuaryAnimalTaskStatePayload</c>.
/// </summary>
public sealed record MapGameOffloadSanctuaryAnimalTaskStatePayload(int Unknown0)
    : MapGameTaskStatePayload
{
    internal static MapGameOffloadSanctuaryAnimalTaskStatePayload Decode(MessageStream stream)
    {
        return new MapGameOffloadSanctuaryAnimalTaskStatePayload(stream.ReadVarInt());
    }

    internal override void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Unknown0);
    }
}
