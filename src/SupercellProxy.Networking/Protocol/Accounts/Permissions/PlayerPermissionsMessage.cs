using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Permissions;

/// <summary>Provides player feature permissions and the optional signed player token used by onboarding services.</summary>
public sealed record PlayerPermissionsMessage(int State, PlayerPermission[] Permissions, bool UnknownFlag, ReadOnlyMemory<byte>? CompressedPlayerToken) : CodecMessage<PlayerPermissionsMessage>, ICodecProvider<PlayerPermissionsMessage>
{
    /// <inheritdoc />
    public static ICodec<PlayerPermissionsMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.State,
        Codecs.Array(PlayerPermission.Codec, CollectionCount.Bounded),
        static message => message.Permissions,
        Codecs.Boolean,
        static message => message.UnknownFlag,
        Codecs.OptionalByteArray,
        static message => message.CompressedPlayerToken,
        static (state, permissions, unknownFlag, compressedPlayerToken) => new PlayerPermissionsMessage(state, permissions, unknownFlag, compressedPlayerToken)
    );

    /// <summary>Keeps the signed player token out of diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(PlayerPermissionsMessage);
    }
}
