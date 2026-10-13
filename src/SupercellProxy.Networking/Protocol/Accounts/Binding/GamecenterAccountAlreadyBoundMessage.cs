using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Binding;

/// <summary>Reports that the Game Center account already belongs to another farm, which the player may load instead.</summary>
/// <param name="SecondsSinceLastPlayed">Seconds since the other farm was last played; the choose-farm popup shows it as the last played time.</param>
/// <param name="GamecenterId">The gamecenter id.</param>
/// <param name="AccountId">The account id of the farm bound to the Game Center account.</param>
/// <param name="PassToken">The pass token of the farm bound to the Game Center account.</param>
/// <param name="IsSupercellId">Whether the other farm is bound to a Supercell ID, in which case the client asks to load it through Supercell ID.</param>
/// <param name="AdditionalValue">A value the client decodes but never reads.</param>
/// <param name="Avatar">The avatar of the farm bound to the Game Center account.</param>
public sealed record GamecenterAccountAlreadyBoundMessage(
    int SecondsSinceLastPlayed,
    string? GamecenterId,
    LongId? AccountId,
    string? PassToken,
    bool IsSupercellId,
    int AdditionalValue,
    SupercellProxy.Networking.Protocol.Avatars.ClientAvatar Avatar
) : CodecMessage<GamecenterAccountAlreadyBoundMessage>, ICodecProvider<GamecenterAccountAlreadyBoundMessage>
{
    /// <inheritdoc />
    public static ICodec<GamecenterAccountAlreadyBoundMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.SecondsSinceLastPlayed,
        Codecs.OptionalString,
        static message => message.GamecenterId,
        Codecs.OptionalLongId,
        static message => message.AccountId,
        Codecs.OptionalString,
        static message => message.PassToken,
        Codecs.Boolean,
        static message => message.IsSupercellId,
        Codecs.VarInt,
        static message => message.AdditionalValue,
        SupercellProxy.Networking.Protocol.Avatars.ClientAvatar.Codec,
        static message => message.Avatar,
        static (secondsSinceLastPlayed, gamecenterId, accountId, passToken, isSupercellId, additionalValue, avatar) => new GamecenterAccountAlreadyBoundMessage(secondsSinceLastPlayed, gamecenterId, accountId, passToken, isSupercellId, additionalValue, avatar)
    );
}
