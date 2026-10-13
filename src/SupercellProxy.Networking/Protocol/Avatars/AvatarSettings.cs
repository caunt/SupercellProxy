using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents <c language="csharp">AvatarSettings</c>.
/// </summary>
public sealed record AvatarSettings(int Version, AvatarSetting[] Entries, bool Unknown0) : ICodecProvider<AvatarSettings>
{
    /// <inheritdoc />
    public static ICodec<AvatarSettings> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static settings => settings.Version,
        Codecs.Array(AvatarSetting.Codec, CollectionCount.Bounded),
        static settings => settings.Entries,
        Codecs.Boolean,
        static settings => settings.Unknown0,
        static (version, entries, unknown0) => new AvatarSettings(version, entries, unknown0)
    );
}
