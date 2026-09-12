using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents <c language="csharp">AvatarStringSection</c>.
/// </summary>
public sealed record AvatarStringSection(string? UnknownString0, string? UnknownString1, string? UnknownString2)
{
    /// <summary>
    /// Initializes a new <see cref="AvatarStringSection"/> instance.
    /// </summary>
    public AvatarStringSection()
        : this(UnknownString0: null, UnknownString1: null, UnknownString2: null) { }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static AvatarStringSection Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadOptionalString(), stream.ReadOptionalString(), stream.ReadOptionalString());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteOptionalString(UnknownString0);
        stream.WriteOptionalString(UnknownString1);
        stream.WriteOptionalString(UnknownString2);
    }
}
