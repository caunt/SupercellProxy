using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Production;

/// <summary>Pays diamonds to unlock a production slot of a building.</summary>
/// <param name="SlotIndex">Locked production queue slot to open.</param>
/// <param name="BuildingObjectGlobalId">Game object id of the production building.</param>
public sealed record UnlockProductionSlotCommand(int SlotIndex, int BuildingObjectGlobalId) : CodecCommand<UnlockProductionSlotCommand>, ICommandCodecProvider<UnlockProductionSlotCommand>
{
    /// <inheritdoc />
    public static CommandCodec<UnlockProductionSlotCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.SlotIndex,
            Codecs.VarInt,
            static command => command.BuildingObjectGlobalId,
            static (slotIndex, buildingObjectGlobalId) => new UnlockProductionSlotCommand(slotIndex, buildingObjectGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
