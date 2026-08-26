using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c>AvatarManagerBEntry</c>.
/// </summary>
public record AvatarManagerBEntry(
    long Unknown0,
    int Unknown1,
    int Unknown2,
    KeyValuePair<int, int>[] Values
)
{
    private AvatarManagerBEntry(
        long unknown0,
        int unknown1,
        int unknown2,
        ReadOnlySpan<KeyValuePair<int, int>> values
    )
        : this(unknown0, unknown1, unknown2, values.ToArray()) { }

    internal static AvatarManagerBEntry Decode(MessageStream stream)
    {
        var unknown0 = stream.ReadInt64();
        var unknown1 = stream.ReadVarInt();
        var unknown2 = stream.ReadVarInt();
        var values = stream.ReadArray(static valueStream => new KeyValuePair<int, int>(
            valueStream.ReadInt32(),
            valueStream.ReadVarInt()
        ));
        return new AvatarManagerBEntry(unknown0, unknown1, unknown2, values.AsSpan());
    }

    internal void Encode(MessageStream stream)
    {
        stream.WriteInt64(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        stream.WriteArray(
            Values,
            static (valueStream, value) =>
            {
                valueStream.WriteInt32(value.Key);
                valueStream.WriteVarInt(value.Value);
            }
        );
    }
}
