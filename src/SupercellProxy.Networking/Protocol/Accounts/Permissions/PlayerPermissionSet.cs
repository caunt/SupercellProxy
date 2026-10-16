using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Permissions;

/// <summary>A player permission set as LoginOkMessage carries it, the same fields PlayerPermissionsMessage sends without its token.</summary>
/// <param name="State">The permission state.</param>
/// <param name="Permissions">One entry per game feature the server can restrict, each with whether it is enabled and its restriction value.</param>
/// <param name="Flag">The flag native reads after the permissions.</param>
public sealed record PlayerPermissionSet(int State, PlayerPermission[] Permissions, bool Flag) : ICodecProvider<PlayerPermissionSet>
{
    /// <inheritdoc />
    public static ICodec<PlayerPermissionSet> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.State,
        Codecs.Array(PlayerPermission.Codec, CollectionCount.Bounded),
        static value => value.Permissions,
        Codecs.Boolean,
        static value => value.Flag,
        static (state, permissions, flag) => new PlayerPermissionSet(state, permissions, flag)
    );
}
