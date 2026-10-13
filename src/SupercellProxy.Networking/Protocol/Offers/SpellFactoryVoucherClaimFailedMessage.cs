using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Offers;

/// <summary>Reports that claiming a Spell Factory voucher failed.</summary>
/// <param name="Code">The code.</param>
/// <param name="Reason">1 already claimed, 2 expired, 3 invalid code, 4 invalidated, 5 conditions not met; anything else is unknown.</param>
public sealed record SpellFactoryVoucherClaimFailedMessage(string? Code, int Reason) : CodecMessage<SpellFactoryVoucherClaimFailedMessage>, ICodecProvider<SpellFactoryVoucherClaimFailedMessage>
{
    /// <inheritdoc />
    public static ICodec<SpellFactoryVoucherClaimFailedMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.Code,
        Codecs.VarInt,
        static message => message.Reason,
        static (code, reason) => new SpellFactoryVoucherClaimFailedMessage(code, reason)
    );
}
