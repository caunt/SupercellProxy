using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>
/// Requests automatic neighborhood creation from a news popup's <c>AutoCreateNeighborhood</c> deep link. Native builds it from the
/// popup's live event (0x100041604) with -1 and 1 as the second and fourth values.
/// </summary>
/// <param name="Name">The news popup live event's name.</param>
/// <param name="EventId">The news popup live event's id.</param>
/// <param name="Unknown1">Always -1 from the native deep-link handler.</param>
/// <param name="EventStartTime">The news popup live event's start as a Unix timestamp.</param>
/// <param name="Unknown3">Always 1 from the native deep-link handler.</param>
public sealed record RequestAutomaticNeighborhoodCreationCommand(string Name, int EventId, int Unknown1, int EventStartTime, int Unknown3) : CodecCommand<RequestAutomaticNeighborhoodCreationCommand>, ICommandCodecProvider<RequestAutomaticNeighborhoodCreationCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RequestAutomaticNeighborhoodCreationCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.String,
            static command => command.Name,
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarInt,
            static command => command.Unknown1,
            Codecs.VarInt,
            static command => command.EventStartTime,
            Codecs.VarInt,
            static command => command.Unknown3,
            static (name, eventId, unknown1, eventStartTime, unknown3) => new RequestAutomaticNeighborhoodCreationCommand(name, eventId, unknown1, eventStartTime, unknown3)
        )
    );
}
