using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Derby;

/// <summary>Removes a task from the player's derby task log (native execute 0x100490e20).</summary>
/// <param name="HomeId">Farm whose task log holds the task; native fails the command for another farm.</param>
/// <param name="NeighborhoodId">Neighbourhood of the derby; native fails the command for another neighbourhood.</param>
/// <param name="TaskGlobalId">Global id of the logged derby task.</param>
/// <param name="RewardPoints">Derby points the logged task is worth.</param>
/// <param name="TaskStatus">Native result state of the logged task.</param>
public sealed record DerbyTaskProgressServerCommand(LongId HomeId, LongId NeighborhoodId, int TaskGlobalId, int RewardPoints, int TaskStatus) : CodecServerCommand<DerbyTaskProgressServerCommand>, ICommandCodecProvider<DerbyTaskProgressServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DerbyTaskProgressServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.HomeId,
            Codecs.LongId,
            static command => command.NeighborhoodId,
            Codecs.Int32,
            static command => command.TaskGlobalId,
            Codecs.Int32,
            static command => command.RewardPoints,
            Codecs.Int32,
            static command => command.TaskStatus,
            static (homeId, neighborhoodId, taskGlobalId, rewardPoints, taskStatus) => new DerbyTaskProgressServerCommand(homeId, neighborhoodId, taskGlobalId, rewardPoints, taskStatus)
        )
    );
}
