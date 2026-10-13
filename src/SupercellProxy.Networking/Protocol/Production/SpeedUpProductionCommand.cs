using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Production;

/// <summary>Finishes the running production of a building immediately for diamonds.</summary>
/// <param name="BuildingGlobalId">The production building game object id.</param>
public sealed record SpeedUpProductionCommand(int BuildingGlobalId) : CodecCommand<SpeedUpProductionCommand>, ICommandCodecProvider<SpeedUpProductionCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpeedUpProductionCommand> Codec { get; } = Codecs.Command(
        type: 96,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.BuildingGlobalId,
            static (buildingGlobalId) => new SpeedUpProductionCommand(buildingGlobalId)
        )
    );
}
