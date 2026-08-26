using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c>AvatarManagerAOptional</c>.
/// </summary>
public record AvatarManagerAOptional(int Unknown0, AvatarManagerASpecial[] Entries)
{
    private AvatarManagerAOptional(int unknown0, ReadOnlySpan<AvatarManagerASpecial> entries)
        : this(unknown0, entries.ToArray()) { }

    internal static AvatarManagerAOptional Decode(MessageStream stream)
    {
        var unknown0 = stream.ReadVarInt();
        var entries = stream.ReadArray(AvatarManagerASpecial.Decode);
        return new AvatarManagerAOptional(unknown0, entries.AsSpan());
    }

    internal void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteArray(Entries, static (valueStream, value) => value.Encode(valueStream));
    }
}
