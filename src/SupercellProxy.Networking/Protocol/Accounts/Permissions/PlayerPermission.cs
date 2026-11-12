using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Permissions;

/// <summary>A player feature permission and its accompanying native restriction value.</summary>
/// <param name="Enabled">Whether the server allows the feature this entry stands for; written as a one-byte boolean before the restriction value.</param>
/// <param name="RestrictionValue">A VarInt the native client stores with the permission; what the number limits is not yet identified.</param>
public sealed record PlayerPermission(bool Enabled, int RestrictionValue) : ICodecProvider<PlayerPermission>
{
    /// <inheritdoc />
    public static ICodec<PlayerPermission> Codec { get; } = Codecs.Composite(Codecs.Boolean,
        static value => value.Enabled,
        Codecs.VarInt,
        static value => value.RestrictionValue,
        static (enabled, restrictionValue) => new PlayerPermission(enabled, restrictionValue));
}
