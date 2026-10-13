using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.DailyTasks;

/// <summary>Rerolls the daily knit tasks.</summary>
/// <param name="UseDiamonds">The use diamonds.</param>
public sealed record RerollDailyTasksCommand(bool UseDiamonds) : CodecCommand<RerollDailyTasksCommand>, ICommandCodecProvider<RerollDailyTasksCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RerollDailyTasksCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.Boolean, static command => command.UseDiamonds, static (useDiamonds) => new RerollDailyTasksCommand(useDiamonds)),
        CommandLayout.MetadataFirst
    );
}
