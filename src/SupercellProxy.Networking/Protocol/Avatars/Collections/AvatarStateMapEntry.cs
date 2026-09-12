using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// Represents <c language="csharp">AvatarStateMapEntry</c>.
/// </summary>
public sealed record AvatarStateMapEntry(int Key, AvatarStateValues State)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static AvatarStateMapEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVariableInt(), AvatarStateValues.Decode(stream));
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(Key);
        State.Encode(stream);
    }
}
