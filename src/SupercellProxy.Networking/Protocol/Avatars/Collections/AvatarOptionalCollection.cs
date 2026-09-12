using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// Represents <c language="csharp">AvatarOptionalCollection</c>.
/// </summary>
public sealed record AvatarOptionalCollection(int Unknown0, AvatarEncodedCollectionEntry[] Entries)
{
    private AvatarOptionalCollection(int unknown0, ReadOnlySpan<AvatarEncodedCollectionEntry> entries)
        : this(unknown0, entries.ToArray()) { }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static AvatarOptionalCollection Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int unknown0 = stream.ReadVariableInt();
        AvatarEncodedCollectionEntry[] entries = stream.ReadArray(AvatarEncodedCollectionEntry.Decode);

        return new AvatarOptionalCollection(unknown0, entries.AsSpan());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(Unknown0);
        stream.WriteArray(Entries, static (valueStream, value) => value.Encode(valueStream));
    }
}
