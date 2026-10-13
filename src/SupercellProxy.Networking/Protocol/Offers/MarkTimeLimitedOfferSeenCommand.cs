using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Offers;

/// <summary>Records that the current time-limited offer was shown.</summary>
public sealed record MarkTimeLimitedOfferSeenCommand : CodecCommand<MarkTimeLimitedOfferSeenCommand>, ICommandCodecProvider<MarkTimeLimitedOfferSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkTimeLimitedOfferSeenCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new MarkTimeLimitedOfferSeenCommand()));
}
