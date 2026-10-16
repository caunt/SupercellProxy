using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.DailyTasks;

/// <summary>Pays diamonds to refill missed daily task knit streak days.</summary>
/// <param name="Days">The number of missed days refilled.</param>
public sealed record RefillKnitStreakCommand(int Days) : CodecCommand<RefillKnitStreakCommand>, ICommandCodecProvider<RefillKnitStreakCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RefillKnitStreakCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.Days, static days => new RefillKnitStreakCommand(days)),
        CommandLayout.MetadataFirst
    );
}
