using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.DeviceLinking;

/// <summary>Completes a device link with the credentials of the linked farm.</summary>
/// <param name="AccountId">The account id.</param>
/// <param name="PassToken">The pass token.</param>
public sealed record LogicDeviceLinkDoneMessage(LongId AccountId, string? PassToken) : CodecMessage<LogicDeviceLinkDoneMessage>, ICodecProvider<LogicDeviceLinkDoneMessage>
{
    /// <inheritdoc />
    public static ICodec<LogicDeviceLinkDoneMessage> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static message => message.AccountId,
        Codecs.OptionalString,
        static message => message.PassToken,
        static (accountId, passToken) => new LogicDeviceLinkDoneMessage(accountId, passToken)
    );
}
