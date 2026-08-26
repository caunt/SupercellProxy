using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c>AvatarSetting</c>.
/// </summary>
public record AvatarSetting(bool Enabled, int Value)
{
    internal static AvatarSetting Decode(MessageStream stream) =>
        new(stream.ReadBoolean(), stream.ReadVarInt());

    internal void Encode(MessageStream stream)
    {
        stream.WriteBoolean(Enabled);
        stream.WriteVarInt(Value);
    }
}
