using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.SupercellId;

/// <summary>Reports that the Supercell ID already belongs to another farm, with that farm's credentials and avatar.</summary>
/// <param name="SupercellId">The supercell id.</param>
/// <param name="AccountId">The account id of the farm bound to the Supercell ID.</param>
/// <param name="PassToken">The pass token of the farm bound to the Supercell ID.</param>
/// <param name="Avatar">The avatar of the farm bound to the Supercell ID.</param>
/// <param name="Text">A trailing text the Supercell ID login manager receives with the message.</param>
public sealed record SupercellIdAccountAlreadyBoundMessage(
    string? SupercellId,
    LongId? AccountId,
    string? PassToken,
    SupercellProxy.Networking.Protocol.Avatars.ClientAvatar Avatar,
    string? Text
) : CodecMessage<SupercellIdAccountAlreadyBoundMessage>, ICodecProvider<SupercellIdAccountAlreadyBoundMessage>
{
    /// <inheritdoc />
    public static ICodec<SupercellIdAccountAlreadyBoundMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.SupercellId,
        Codecs.OptionalLongId,
        static message => message.AccountId,
        Codecs.OptionalString,
        static message => message.PassToken,
        SupercellProxy.Networking.Protocol.Avatars.ClientAvatar.Codec,
        static message => message.Avatar,
        Codecs.OptionalString,
        static message => message.Text,
        static (supercellId, accountId, passToken, avatar, text) => new SupercellIdAccountAlreadyBoundMessage(supercellId, accountId, passToken, avatar, text)
    );
}
