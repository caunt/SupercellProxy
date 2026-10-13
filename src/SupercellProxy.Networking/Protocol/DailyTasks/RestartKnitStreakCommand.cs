using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.DailyTasks;

/// <summary>Starts the daily task knit streak over.</summary>
public sealed record RestartKnitStreakCommand : CodecCommand<RestartKnitStreakCommand>, ICommandCodecProvider<RestartKnitStreakCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RestartKnitStreakCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new RestartKnitStreakCommand()));
}
