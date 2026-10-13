using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Unlock;

/// <summary>Asks the server to unlock a locked account.</summary>
/// <param name="PassToken">The pass token.</param>
/// <param name="UnlockCode">The unlock code.</param>
/// <param name="AccountId">The account id.</param>
/// <param name="Email">The email.</param>
public sealed record UnlockAccountMessage(string? PassToken, string? UnlockCode, LongId AccountId, string? Email) : CodecMessage<UnlockAccountMessage>, ICodecProvider<UnlockAccountMessage>
{
    /// <inheritdoc />
    public static ICodec<UnlockAccountMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.PassToken,
        Codecs.OptionalString,
        static message => message.UnlockCode,
        Codecs.LongId,
        static message => message.AccountId,
        Codecs.OptionalString,
        static message => message.Email,
        static (passToken, unlockCode, accountId, email) => new UnlockAccountMessage(passToken, unlockCode, accountId, email)
    );
}
