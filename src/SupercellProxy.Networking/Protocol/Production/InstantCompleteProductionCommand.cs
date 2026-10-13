using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Production;

/// <summary>Instantly completes production with a workbench perk.</summary>
/// <param name="SlotIndex">The slot index.</param>
/// <param name="BuildingObjectGlobalId">The building object id.</param>
public sealed record InstantCompleteProductionCommand(int SlotIndex, int BuildingObjectGlobalId) : CodecCommand<InstantCompleteProductionCommand>, ICommandCodecProvider<InstantCompleteProductionCommand>
{
    /// <inheritdoc />
    public static CommandCodec<InstantCompleteProductionCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.SlotIndex,
            Codecs.VarInt,
            static command => command.BuildingObjectGlobalId,
            static (slotIndex, buildingObjectGlobalId) => new InstantCompleteProductionCommand(slotIndex, buildingObjectGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
