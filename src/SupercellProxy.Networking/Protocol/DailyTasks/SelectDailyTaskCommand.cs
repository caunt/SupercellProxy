using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.DailyTasks;

/// <summary>Selects a daily knit task.</summary>
/// <param name="TaskIndex">The task index.</param>
public sealed record SelectDailyTaskCommand(int TaskIndex) : CodecCommand<SelectDailyTaskCommand>, ICommandCodecProvider<SelectDailyTaskCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SelectDailyTaskCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.TaskIndex, static (taskIndex) => new SelectDailyTaskCommand(taskIndex)),
        CommandLayout.MetadataFirst
    );
}
