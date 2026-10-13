using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Buildings.Upgrades;

/// <summary>Pays diamonds to finish a building's pending upgrade.</summary>
/// <param name="ObjectGlobalId">The building object id.</param>
public sealed record FinishBuildingUpgradeWithDiamondsCommand(int ObjectGlobalId) : CodecCommand<FinishBuildingUpgradeWithDiamondsCommand>, ICommandCodecProvider<FinishBuildingUpgradeWithDiamondsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<FinishBuildingUpgradeWithDiamondsCommand> Codec { get; } = Codecs.Command(
        type: 125,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.ObjectGlobalId,
            static (objectGlobalId) => new FinishBuildingUpgradeWithDiamondsCommand(objectGlobalId)
        )
    );
}
