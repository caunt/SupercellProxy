using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Accepts or declines extra time for an expired boat order.</summary>
/// <param name="Extend"><see langword="true"/> to buy the extra time with diamonds; <see langword="false"/> to decline and let the boat leave.</param>
public sealed record RespondToBoatTimeExtensionCommand(bool Extend) : CodecCommand<RespondToBoatTimeExtensionCommand>, ICommandCodecProvider<RespondToBoatTimeExtensionCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RespondToBoatTimeExtensionCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.Boolean, static command => command.Extend, static extend => new RespondToBoatTimeExtensionCommand(extend)),
        CommandLayout.MetadataFirst);
}
