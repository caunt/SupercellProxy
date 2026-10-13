using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents <c language="csharp">AvatarSetting</c>.
/// </summary>
public sealed record AvatarSetting(bool Enabled, int Value) : ICodecProvider<AvatarSetting>
{
    /// <inheritdoc />
    public static ICodec<AvatarSetting> Codec { get; } = Codecs.Composite(
        Codecs.Boolean,
        static value => value.Enabled,
        Codecs.VarInt,
        static value => value.Value,
        static (enabled, @value) => new AvatarSetting(enabled, value)
    );
}
