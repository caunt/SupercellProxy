using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// Represents <c language="csharp">AvatarStateEntry</c>.
/// </summary>
public sealed record AvatarStateEntry(long Unknown0, int Unknown1, int Unknown2, KeyValuePair<int, int>[] Values)
{
    private AvatarStateEntry(long unknown0, int unknown1, int unknown2, ReadOnlySpan<KeyValuePair<int, int>> values)
        : this(unknown0, unknown1, unknown2, values.ToArray()) { }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static AvatarStateEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        long unknown0 = stream.ReadInt64();
        int unknown1 = stream.ReadVariableInt();
        int unknown2 = stream.ReadVariableInt();

        KeyValuePair<int, int>[] values = stream.ReadArray(static valueStream => new KeyValuePair<int, int>(valueStream.ReadInt32(), valueStream.ReadVariableInt()));

        return new AvatarStateEntry(unknown0, unknown1, unknown2, values.AsSpan());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteInt64(Unknown0);
        stream.WriteVariableInt(Unknown1);
        stream.WriteVariableInt(Unknown2);
        stream.WriteArray(Values, static (valueStream, value) => { valueStream.WriteInt32(value.Key); valueStream.WriteVariableInt(value.Value); });
    }
}
