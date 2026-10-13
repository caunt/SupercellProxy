using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.DailyTasks;

/// <summary>Collects the yarn earned from daily knit tasks.</summary>
public sealed record CollectDailyTaskYarnCommand : CodecCommand<CollectDailyTaskYarnCommand>, ICommandCodecProvider<CollectDailyTaskYarnCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectDailyTaskYarnCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new CollectDailyTaskYarnCommand()));
}
