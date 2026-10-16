using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Unlock;

/// <summary>Asks the server to unlock a locked account.</summary>
/// <param name="PassToken">The locked account's stored login secret.</param>
/// <param name="UnlockCode">Code the player received to unlock the account.</param>
/// <param name="AccountId">The locked account.</param>
/// <param name="Email">E-mail address the player gives with the unlock request.</param>
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
