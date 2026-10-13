using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.DailyTasks;

/// <summary>Marks the daily tasks as seen.</summary>
public sealed record MarkDailyTasksSeenCommand : CodecCommand<MarkDailyTasksSeenCommand>, ICommandCodecProvider<MarkDailyTasksSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkDailyTasksSeenCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new MarkDailyTasksSeenCommand()));
}
