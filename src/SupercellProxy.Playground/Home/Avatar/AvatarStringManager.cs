using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c language="csharp">AvatarStringManager</c>.
/// </summary>
internal sealed record AvatarStringManager(
    string? UnknownString0,
    string? UnknownString1,
    string? UnknownString2
)
{
    /// <summary>
    /// Initializes a new <see cref="AvatarStringManager"/> instance.
    /// </summary>
    public AvatarStringManager()
        : this(UnknownString0: null, UnknownString1: null, UnknownString2: null) { }

    internal static AvatarStringManager Decode(MessageStream stream) =>
        new(stream.ReadOptionalString(), stream.ReadOptionalString(), stream.ReadOptionalString());

    internal void Encode(MessageStream stream)
    {
        stream.WriteOptionalString(UnknownString0);
        stream.WriteOptionalString(UnknownString1);
        stream.WriteOptionalString(UnknownString2);
    }
}
