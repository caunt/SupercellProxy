using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Sets the effective derby opt-in preference for the matching neighborhood.</summary>
/// <param name="NeighborhoodId">The neighbourhood the change applies to, the first field.</param>
/// <param name="ActorId">The avatar id of the member who made the change.</param>
/// <param name="Enabled"><see langword="true"/> to opt the player into the derby; <see langword="false"/> to opt out.</param>
public sealed record SetDerbyParticipationServerCommand(LongId NeighborhoodId, LongId ActorId, bool Enabled) : CodecServerCommand<SetDerbyParticipationServerCommand>, ICommandCodecProvider<SetDerbyParticipationServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetDerbyParticipationServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.LongId,
        static command => command.NeighborhoodId,
        Codecs.LongId,
        static command => command.ActorId,
        Codecs.Boolean,
        static command => command.Enabled,
        static (neighborhoodId, actorId, enabled) => new SetDerbyParticipationServerCommand(neighborhoodId, actorId, enabled)));
}
