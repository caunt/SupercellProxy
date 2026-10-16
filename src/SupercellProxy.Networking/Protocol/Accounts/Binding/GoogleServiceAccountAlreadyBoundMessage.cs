using SupercellProxy.Networking.Protocol.Avatars;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Binding;

/// <summary>Reports that the Google Play Games account already belongs to another farm, which the player may load instead.</summary>
/// <param name="SecondsSinceLastPlayed">Seconds since the other farm was last played; the choose-farm popup shows it as the last played time.</param>
/// <param name="GoogleServiceId">Google Play Games player id the player tried to bind.</param>
/// <param name="AccountId">The account id of the farm bound to the Google Play Games account.</param>
/// <param name="PassToken">The pass token of the farm bound to the Google Play Games account.</param>
/// <param name="IsSupercellId">Whether the other farm is bound to a Supercell ID, in which case the client asks to load it through Supercell ID.</param>
/// <param name="Avatar">The avatar of the farm bound to the Google Play Games account.</param>
public sealed record GoogleServiceAccountAlreadyBoundMessage(
    int SecondsSinceLastPlayed,
    string? GoogleServiceId,
    LongId? AccountId,
    string? PassToken,
    bool IsSupercellId,
    ClientAvatar Avatar
) : CodecMessage<GoogleServiceAccountAlreadyBoundMessage>, ICodecProvider<GoogleServiceAccountAlreadyBoundMessage>
{
    /// <inheritdoc />
    public static ICodec<GoogleServiceAccountAlreadyBoundMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.SecondsSinceLastPlayed,
        Codecs.OptionalString,
        static message => message.GoogleServiceId,
        Codecs.OptionalLongId,
        static message => message.AccountId,
        Codecs.OptionalString,
        static message => message.PassToken,
        Codecs.Boolean,
        static message => message.IsSupercellId,
        SupercellProxy.Networking.Protocol.Avatars.ClientAvatar.Codec,
        static message => message.Avatar,
        static (secondsSinceLastPlayed, googleServiceId, accountId, passToken, isSupercellId, avatar) => new GoogleServiceAccountAlreadyBoundMessage(secondsSinceLastPlayed, googleServiceId, accountId, passToken, isSupercellId, avatar)
    );
}
