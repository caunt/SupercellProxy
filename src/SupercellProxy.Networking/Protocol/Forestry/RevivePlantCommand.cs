using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>Asks neighbours to revive a withered fruit tree or gatherer mine.</summary>
/// <param name="PlantGlobalId">The fruit tree or gatherer mine game object id.</param>
public sealed record RevivePlantCommand(int PlantGlobalId) : CodecCommand<RevivePlantCommand>, ICommandCodecProvider<RevivePlantCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RevivePlantCommand> Codec { get; } = Codecs.Command(
        type: 63,
        Codecs.Composite(Codecs.VarInt, static command => command.PlantGlobalId, static (plantGlobalId) => new RevivePlantCommand(plantGlobalId))
    );
}
