using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Buildings.Upgrades;

/// <summary>Repairs a building, replacing it with its repaired data and placing it at the given position.</summary>
/// <param name="DataGlobalId">The repaired building data id.</param>
/// <param name="ObjectGlobalId">The broken building object id.</param>
/// <param name="PositionX">The x coordinate.</param>
/// <param name="PositionY">The y coordinate.</param>
/// <param name="UseDiamonds">Whether missing materials are paid with diamonds.</param>
public sealed record RepairBuildingCommand(int DataGlobalId, int ObjectGlobalId, int PositionX, int PositionY, bool UseDiamonds) : CodecCommand<RepairBuildingCommand>, ICommandCodecProvider<RepairBuildingCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RepairBuildingCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.DataGlobalId,
            Codecs.VarInt,
            static command => command.ObjectGlobalId,
            Codecs.VarInt,
            static command => command.PositionX,
            Codecs.VarInt,
            static command => command.PositionY,
            Codecs.Boolean,
            static command => command.UseDiamonds,
            static (dataGlobalId, objectGlobalId, positionX, positionY, useDiamonds) => new RepairBuildingCommand(dataGlobalId, objectGlobalId, positionX, positionY, useDiamonds)
        )
    );
}
