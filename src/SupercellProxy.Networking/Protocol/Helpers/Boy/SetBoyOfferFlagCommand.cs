using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Helpers.Boy;

/// <summary>Stores the boy offer flag.</summary>
/// <param name="Enabled">The new flag value.</param>
public sealed record SetBoyOfferFlagCommand(bool Enabled) : CodecCommand<SetBoyOfferFlagCommand>, ICommandCodecProvider<SetBoyOfferFlagCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetBoyOfferFlagCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.Boolean, static command => command.Enabled, static enabled => new SetBoyOfferFlagCommand(enabled)));
}
