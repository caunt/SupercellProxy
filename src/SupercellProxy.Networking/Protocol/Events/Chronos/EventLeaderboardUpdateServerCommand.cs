using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>Updates the player's standing in a board event's leaderboard (native execute 0x100494344).</summary>
/// <param name="EventId">Board event the standing belongs to.</param>
/// <param name="Rank">The standing native compares with the previous one to set the event's leaderboard button.</param>
/// <param name="Unknown2">A value native's update does not read.</param>
/// <param name="Completed">Whether the event's leaderboard is finished; native keeps the flag once set.</param>
/// <param name="Unknown4">A value native keeps on the event for its leaderboard view; its meaning is not identified.</param>
/// <param name="Unknown5">A second value native keeps on the event for its leaderboard view; its meaning is not identified.</param>
public sealed record EventLeaderboardUpdateServerCommand(int EventId, int Rank, int Unknown2, bool Completed, long Unknown4, int Unknown5)
    : CodecServerCommand<EventLeaderboardUpdateServerCommand>, ICommandCodecProvider<EventLeaderboardUpdateServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<EventLeaderboardUpdateServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarInt,
            static command => command.Rank,
            Codecs.VarInt,
            static command => command.Unknown2,
            Codecs.Boolean,
            static command => command.Completed,
            Codecs.VarLong,
            static command => command.Unknown4,
            Codecs.VarInt,
            static command => command.Unknown5,
            static (eventId, rank, unknown2, completed, unknown4, unknown5) => new EventLeaderboardUpdateServerCommand(eventId, rank, unknown2, completed, unknown4, unknown5)
        )
    );
}
