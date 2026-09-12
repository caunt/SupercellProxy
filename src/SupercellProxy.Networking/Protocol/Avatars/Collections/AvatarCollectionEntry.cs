using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// Represents <c language="csharp">AvatarCollectionEntry</c>.
/// </summary>
public sealed record AvatarCollectionEntry(int Unknown0, int Kind, int Unknown1, int? KindValue, int Unknown2)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static AvatarCollectionEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int unknown0 = stream.ReadVariableInt();
        int kind = stream.ReadVariableInt();
        int unknown1 = stream.ReadVariableInt();
        int? kindValue = kind is 1 ? stream.ReadVariableInt() : null;

        return new AvatarCollectionEntry(unknown0, kind, unknown1, kindValue, stream.ReadVariableInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(Unknown0);
        stream.WriteVariableInt(Kind);
        stream.WriteVariableInt(Unknown1);

        if (Kind is 1)
            stream.WriteVariableInt(KindValue ?? throw new InvalidOperationException($"{nameof(KindValue)} is null."));

        stream.WriteVariableInt(Unknown2);
    }
}
