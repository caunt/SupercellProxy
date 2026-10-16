using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// The settings block that closes the avatar's progression section: a numbered list of switches, each with a value. Which entry
/// controls what is not identified; the block is decoded so it can be re-encoded exactly.
/// </summary>
/// <param name="Version">Revision number of the block's layout (0 in captures).</param>
/// <param name="Entries">The switches in native order.</param>
/// <param name="Unknown0">Flag that ends the block; its meaning is not identified.</param>
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
