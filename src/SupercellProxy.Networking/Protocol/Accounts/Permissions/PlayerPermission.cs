using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Permissions;

/// <summary>A player feature permission and its accompanying native restriction value.</summary>
public sealed record PlayerPermission(bool Enabled, int RestrictionValue) : ICodecProvider<PlayerPermission>
{
    /// <inheritdoc />
    public static ICodec<PlayerPermission> Codec { get; } = Codecs.Composite(
        Codecs.Boolean,
        static value => value.Enabled,
        Codecs.VarInt,
        static value => value.RestrictionValue,
        static (enabled, restrictionValue) => new PlayerPermission(enabled, restrictionValue)
    );
}
