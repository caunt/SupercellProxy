using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Accepts or declines extra time for an expired boat order.</summary>
public sealed record RespondToBoatTimeExtensionCommand(bool Extend) : CodecCommand<RespondToBoatTimeExtensionCommand>, ICommandCodecProvider<RespondToBoatTimeExtensionCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RespondToBoatTimeExtensionCommand> Codec { get; } = Codecs.Command(
        type: 712,
        Codecs.Composite(Codecs.Boolean, static command => command.Extend, static (extend) => new RespondToBoatTimeExtensionCommand(extend)),
        CommandLayout.MetadataFirst
    );
}
