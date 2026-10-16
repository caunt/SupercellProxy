using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// One switch of the avatar settings block.
/// </summary>
/// <param name="Enabled">Whether the switch is on (all on in captures).</param>
/// <param name="Value">Number stored with the switch (0 in captures).</param>
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
