using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c>AvatarEntryC</c>.
/// </summary>
public record AvatarEntryC(LongId? UnknownId, int Unknown0, int Unknown1, bool Unknown2)
{
    internal static AvatarEntryC Decode(MessageStream stream) =>
        new(
            stream.ReadOptionalLongId(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadBoolean()
        );

    internal void Encode(MessageStream stream)
    {
        stream.WriteOptionalLongId(UnknownId);
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteBoolean(Unknown2);
    }
}
