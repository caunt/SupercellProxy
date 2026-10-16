using SupercellProxy.Networking.Protocol.Avatars;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Binding;

/// <summary>Reports that the Amazon account already belongs to another farm, which the player may load instead.</summary>
/// <param name="SecondsSinceLastPlayed">Seconds since the other farm was last played; the choose-farm popup shows it as the last played time.</param>
/// <param name="AmazonId">Amazon user id the player tried to bind.</param>
/// <param name="AccountId">The account id of the farm bound to the Amazon account.</param>
/// <param name="PassToken">The pass token of the farm bound to the Amazon account.</param>
/// <param name="Avatar">The avatar of the farm bound to the Amazon account.</param>
public sealed record AmazonAccountAlreadyBoundMessage(int SecondsSinceLastPlayed, string? AmazonId, LongId? AccountId, string? PassToken, ClientAvatar Avatar) : CodecMessage<AmazonAccountAlreadyBoundMessage>, ICodecProvider<AmazonAccountAlreadyBoundMessage>
{
    /// <inheritdoc />
    public static ICodec<AmazonAccountAlreadyBoundMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.SecondsSinceLastPlayed,
        Codecs.OptionalString,
        static message => message.AmazonId,
        Codecs.OptionalLongId,
        static message => message.AccountId,
        Codecs.OptionalString,
        static message => message.PassToken,
        SupercellProxy.Networking.Protocol.Avatars.ClientAvatar.Codec,
        static message => message.Avatar,
        static (secondsSinceLastPlayed, amazonId, accountId, passToken, avatar) => new AmazonAccountAlreadyBoundMessage(secondsSinceLastPlayed, amazonId, accountId, passToken, avatar)
    );
}
