using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Buildings.Upgrades;

/// <summary>Upgrades a storage building (silo, barn, tackle box, lobster pool or duck salon) to its next rank and grants the upgrade experience.</summary>
/// <param name="ObjectGlobalId">The storage building object id.</param>
/// <param name="Rank">The rank the native upgrade path applies.</param>
public sealed record UpgradeStorageBuildingCommand(int ObjectGlobalId, byte Rank) : CodecCommand<UpgradeStorageBuildingCommand>, ICommandCodecProvider<UpgradeStorageBuildingCommand>
{
    /// <inheritdoc />
    public static CommandCodec<UpgradeStorageBuildingCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.ObjectGlobalId,
            Codecs.Byte,
            static command => command.Rank,
            static (objectGlobalId, rank) => new UpgradeStorageBuildingCommand(objectGlobalId, rank)
        )
    );
}
