using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c language="csharp">AvatarEntryA</c>.
/// </summary>
internal sealed record AvatarEntryA(int Unknown0, int Unknown1, int Unknown2, LongId? UnknownId)
{
    internal static AvatarEntryA Decode(MessageStream stream) =>
        new(
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadOptionalLongId()
        );

    internal void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        stream.WriteOptionalLongId(UnknownId);
    }
}
