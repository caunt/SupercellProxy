using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Tracks <c>AvatarManagerBState</c> during turn simulation.
/// </summary>
public record AvatarManagerBState(int Unknown0, int Unknown1, int Unknown2, long Unknown3)
{
    internal static AvatarManagerBState Decode(MessageStream stream) =>
        new(stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadInt64());

    internal void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        stream.WriteInt64(Unknown3);
    }
}
