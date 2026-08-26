using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c>AvatarEntryB</c>.
/// </summary>
public record AvatarEntryB(LongId? UnknownId, int Unknown0, bool Unknown1)
{
    internal static AvatarEntryB Decode(MessageStream stream) =>
        new(stream.ReadOptionalLongId(), stream.ReadVarInt(), stream.ReadBoolean());

    internal void Encode(MessageStream stream)
    {
        stream.WriteOptionalLongId(UnknownId);
        stream.WriteVarInt(Unknown0);
        stream.WriteBoolean(Unknown1);
    }
}
