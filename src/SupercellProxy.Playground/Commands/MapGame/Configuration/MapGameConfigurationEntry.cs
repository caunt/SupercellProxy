using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Native optional logic-long and three-value map-game configuration entry.</para>
/// </summary>
public sealed record MapGameConfigurationEntry(
    LongId? UnknownLongId,
    int Unknown0,
    int Unknown1,
    int Unknown2
)
{
    internal static MapGameConfigurationEntry Decode(MessageStream stream)
    {
        return new MapGameConfigurationEntry(
            MapGameWire.ReadOptionalLongId(stream),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt()
        );
    }

    internal void Encode(MessageStream stream)
    {
        MapGameWire.WriteOptionalLongId(stream, UnknownLongId);
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
    }
}
