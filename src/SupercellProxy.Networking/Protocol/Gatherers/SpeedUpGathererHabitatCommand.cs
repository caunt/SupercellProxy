using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Gatherers;

/// <summary>Finishes a gatherer habitat immediately for diamonds.</summary>
/// <param name="HabitatGlobalId">The gatherer habitat game object id.</param>
public sealed record SpeedUpGathererHabitatCommand(int HabitatGlobalId) : CodecCommand<SpeedUpGathererHabitatCommand>, ICommandCodecProvider<SpeedUpGathererHabitatCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpeedUpGathererHabitatCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.HabitatGlobalId,
            static (habitatGlobalId) => new SpeedUpGathererHabitatCommand(habitatGlobalId)
        )
    );
}
