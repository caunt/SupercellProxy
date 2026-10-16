using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Production;

/// <summary>Instantly completes production with a workbench perk.</summary>
/// <param name="SlotIndex">Production queue slot whose item finishes at once.</param>
/// <param name="BuildingObjectGlobalId">Game object id of the production building.</param>
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
