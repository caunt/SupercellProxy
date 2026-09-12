using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents <c language="csharp">AvatarSettings</c>.
/// </summary>
public sealed record AvatarSettings(int Version, AvatarSetting[] Entries, bool Unknown0)
{
    private AvatarSettings(int version, ReadOnlySpan<AvatarSetting> entries, bool unknown0)
        : this(version, entries.ToArray(), unknown0) { }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static AvatarSettings Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int version = stream.ReadVariableInt();
        AvatarSetting[] entries = stream.ReadArray(AvatarSetting.Decode);

        return new AvatarSettings(version, entries.AsSpan(), stream.ReadBoolean());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(Version);
        stream.WriteArray(Entries, static (valueStream, value) => value.Encode(valueStream));
        stream.WriteBoolean(Unknown0);
    }
}
