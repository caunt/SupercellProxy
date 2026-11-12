using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Permissions;

/// <summary>Provides player feature permissions and the optional signed player token used by onboarding services.</summary>
/// <param name="State">The first field, a VarInt permission state that <see cref="PlayerPermissionSet.State"/> also carries at login; its individual values are not yet identified.</param>
/// <param name="Permissions">One entry per game feature the server can restrict, in the server's order, each with whether it is enabled and its restriction value.</param>
/// <param name="UnknownFlag">The boolean native reads after the permissions, the same field as <see cref="PlayerPermissionSet.Flag"/>; its meaning is not yet identified.</param>
/// <param name="CompressedPlayerToken">The compressed signed player token as raw bytes, or <see langword="null"/> when the server sends no token.</param>
public sealed record PlayerPermissionsMessage(int State, PlayerPermission[] Permissions, bool UnknownFlag, ReadOnlyMemory<byte>? CompressedPlayerToken) : CodecMessage<PlayerPermissionsMessage>, ICodecProvider<PlayerPermissionsMessage>
{
    /// <inheritdoc />
    public static ICodec<PlayerPermissionsMessage> Codec { get; } = Codecs.Composite(Codecs.VarInt,
        static message => message.State,
        Codecs.Array(PlayerPermission.Codec, CollectionCount.Bounded),
        static message => message.Permissions,
        Codecs.Boolean,
        static message => message.UnknownFlag,
        Codecs.OptionalByteArray,
        static message => message.CompressedPlayerToken,
        static (state, permissions, unknownFlag, compressedPlayerToken) => new PlayerPermissionsMessage(state, permissions, unknownFlag, compressedPlayerToken));

    /// <summary>Keeps the signed player token out of diagnostic text.</summary>
    /// <returns>Always the text <c>PlayerPermissionsMessage</c>, without any of the message's fields.</returns>
    public override string ToString()
    {
        return nameof(PlayerPermissionsMessage);
    }
}
