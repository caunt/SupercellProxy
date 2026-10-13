using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Sets the effective derby opt-in preference for the matching neighborhood.</summary>
public sealed record SetDerbyParticipationServerCommand(LongId NeighborhoodId, LongId ActorId, bool Enabled) : CodecServerCommand<SetDerbyParticipationServerCommand>, ICommandCodecProvider<SetDerbyParticipationServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetDerbyParticipationServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.NeighborhoodId,
            Codecs.LongId,
            static command => command.ActorId,
            Codecs.Boolean,
            static command => command.Enabled,
            static (neighborhoodId, actorId, enabled) => new SetDerbyParticipationServerCommand(neighborhoodId, actorId, enabled)
        )
    );
}
