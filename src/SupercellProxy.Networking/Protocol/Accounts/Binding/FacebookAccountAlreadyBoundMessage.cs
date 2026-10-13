using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Binding;

/// <summary>Reports that the Facebook account already belongs to another farm, which the player may load instead.</summary>
/// <param name="SecondsSinceLastPlayed">Seconds since the other farm was last played; the choose-farm popup shows it as the last played time.</param>
/// <param name="FacebookId">The facebook id.</param>
/// <param name="AccountId">The account id of the farm bound to the Facebook account.</param>
/// <param name="PassToken">The pass token of the farm bound to the Facebook account.</param>
/// <param name="IsSupercellId">Whether the other farm is bound to a Supercell ID, in which case the client asks to load it through Supercell ID.</param>
/// <param name="Avatar">The avatar of the farm bound to the Facebook account.</param>
public sealed record FacebookAccountAlreadyBoundMessage(
    int SecondsSinceLastPlayed,
    string? FacebookId,
    LongId? AccountId,
    string? PassToken,
    bool IsSupercellId,
    SupercellProxy.Networking.Protocol.Avatars.ClientAvatar Avatar
) : CodecMessage<FacebookAccountAlreadyBoundMessage>, ICodecProvider<FacebookAccountAlreadyBoundMessage>
{
    /// <inheritdoc />
    public static ICodec<FacebookAccountAlreadyBoundMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.SecondsSinceLastPlayed,
        Codecs.OptionalString,
        static message => message.FacebookId,
        Codecs.OptionalLongId,
        static message => message.AccountId,
        Codecs.OptionalString,
        static message => message.PassToken,
        Codecs.Boolean,
        static message => message.IsSupercellId,
        SupercellProxy.Networking.Protocol.Avatars.ClientAvatar.Codec,
        static message => message.Avatar,
        static (secondsSinceLastPlayed, facebookId, accountId, passToken, isSupercellId, avatar) => new FacebookAccountAlreadyBoundMessage(secondsSinceLastPlayed, facebookId, accountId, passToken, isSupercellId, avatar)
    );
}
