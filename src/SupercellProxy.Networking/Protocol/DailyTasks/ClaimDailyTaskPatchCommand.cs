using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.DailyTasks;

/// <summary>Spends yarn points on the next daily task quilt patch.</summary>
public sealed record ClaimDailyTaskPatchCommand : CodecCommand<ClaimDailyTaskPatchCommand>, ICommandCodecProvider<ClaimDailyTaskPatchCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimDailyTaskPatchCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new ClaimDailyTaskPatchCommand()));
}
