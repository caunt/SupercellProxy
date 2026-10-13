using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>Restarts a revived fruit tree or gatherer mine on the own farm.</summary>
/// <param name="PlantGlobalId">The fruit tree or gatherer mine game object id.</param>
public sealed record FinishPlantRevivalCommand(int PlantGlobalId) : CodecCommand<FinishPlantRevivalCommand>, ICommandCodecProvider<FinishPlantRevivalCommand>
{
    /// <inheritdoc />
    public static CommandCodec<FinishPlantRevivalCommand> Codec { get; } = Codecs.Command(
        type: 66,
        Codecs.Composite(Codecs.VarInt, static command => command.PlantGlobalId, static (plantGlobalId) => new FinishPlantRevivalCommand(plantGlobalId))
    );
}
