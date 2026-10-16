using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>Posts a revival request for a withered fruit tree or gatherer mine on the own farm.</summary>
/// <param name="PlantGlobalId">The fruit tree or gatherer mine game object id.</param>
public sealed record RequestPlantRevivalCommand(int PlantGlobalId) : CodecCommand<RequestPlantRevivalCommand>, ICommandCodecProvider<RequestPlantRevivalCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RequestPlantRevivalCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.PlantGlobalId, static plantGlobalId => new RequestPlantRevivalCommand(plantGlobalId))
    );
}
