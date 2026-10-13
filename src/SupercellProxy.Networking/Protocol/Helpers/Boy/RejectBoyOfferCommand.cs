using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Helpers.Boy;

/// <summary>Rejects the boy's current offer.</summary>
public sealed record RejectBoyOfferCommand : CodecCommand<RejectBoyOfferCommand>, ICommandCodecProvider<RejectBoyOfferCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RejectBoyOfferCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new RejectBoyOfferCommand()));
}
