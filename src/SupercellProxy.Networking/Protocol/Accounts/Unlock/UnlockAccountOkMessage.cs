using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Unlock;

/// <summary>Confirms the account unlock.</summary>
/// <param name="PassToken">Login secret the client stores and uses for the unlocked account from now on.</param>
/// <param name="AccountId">The unlocked account.</param>
public sealed record UnlockAccountOkMessage(string? PassToken, LongId AccountId) : CodecMessage<UnlockAccountOkMessage>, ICodecProvider<UnlockAccountOkMessage>
{
    /// <inheritdoc />
    public static ICodec<UnlockAccountOkMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.PassToken,
        Codecs.LongId,
        static message => message.AccountId,
        static (passToken, accountId) => new UnlockAccountOkMessage(passToken, accountId)
    );
}
