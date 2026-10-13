using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Production;

/// <summary>Pays diamonds to unlock a production slot of a building.</summary>
/// <param name="SlotIndex">The slot index.</param>
/// <param name="BuildingObjectGlobalId">The building object id.</param>
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
