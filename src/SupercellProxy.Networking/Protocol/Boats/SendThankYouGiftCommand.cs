using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Preserves the native SendThankYouGiftCommand wire contract.</summary>
/// <param name="GiftKind">The GiftKind value carried by the native command.</param>
/// <param name="TargetIndex">The TargetIndex value carried by the native command.</param>
/// <param name="HelperId">The HelperId value carried by the native command.</param>
public sealed record SendThankYouGiftCommand(int GiftKind, int TargetIndex, LongId HelperId) : CodecCommand<SendThankYouGiftCommand>, ICommandCodecProvider<SendThankYouGiftCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SendThankYouGiftCommand> Codec { get; } = Codecs.Command(
        type: 102,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.GiftKind,
            Codecs.VarInt,
            static command => command.TargetIndex,
            Codecs.LongId,
            static command => command.HelperId,
            static (giftKind, targetIndex, helperId) => new SendThankYouGiftCommand(giftKind, targetIndex, helperId)
        )
    );
}
