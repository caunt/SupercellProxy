using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c>AvatarSettings</c>.
/// </summary>
public record AvatarSettings(int Version, AvatarSetting[] Entries, bool Unknown0)
{
    private AvatarSettings(int version, ReadOnlySpan<AvatarSetting> entries, bool unknown0)
        : this(version, entries.ToArray(), unknown0) { }

    internal static AvatarSettings Decode(MessageStream stream)
    {
        var version = stream.ReadVarInt();
        var entries = stream.ReadArray(AvatarSetting.Decode);
        return new AvatarSettings(version, entries.AsSpan(), stream.ReadBoolean());
    }

    internal void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Version);
        stream.WriteArray(Entries, static (valueStream, value) => value.Encode(valueStream));
        stream.WriteBoolean(Unknown0);
    }
}
