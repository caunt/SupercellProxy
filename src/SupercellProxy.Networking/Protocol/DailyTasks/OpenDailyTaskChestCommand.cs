using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.DailyTasks;

/// <summary>Opens the reached daily task chest.</summary>
public sealed record OpenDailyTaskChestCommand : CodecCommand<OpenDailyTaskChestCommand>, ICommandCodecProvider<OpenDailyTaskChestCommand>
{
    /// <inheritdoc />
    public static CommandCodec<OpenDailyTaskChestCommand> Codec { get; } = Codecs.Command(type: 604, Codecs.Unit(static () => new OpenDailyTaskChestCommand()));
}
