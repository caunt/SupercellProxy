using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c>AvatarManagerBMapEntry</c>.
/// </summary>
public record AvatarManagerBMapEntry(int Key, AvatarManagerBState State)
{
    internal static AvatarManagerBMapEntry Decode(MessageStream stream) =>
        new(stream.ReadVarInt(), AvatarManagerBState.Decode(stream));

    internal void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Key);
        State.Encode(stream);
    }
}
