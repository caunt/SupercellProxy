using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Production;

/// <summary>Finishes a production slot at once for diamonds.</summary>
/// <param name="BuildingObjectGlobalId">Game object id of the production building.</param>
/// <param name="SlotIndex">Production queue slot whose item finishes at once.</param>
public sealed record InstantCompleteProductionCommand(int BuildingObjectGlobalId, int SlotIndex) : CodecCommand<InstantCompleteProductionCommand>, ICommandCodecProvider<InstantCompleteProductionCommand>
{
    /// <inheritdoc />
    public static CommandCodec<InstantCompleteProductionCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.BuildingObjectGlobalId,
            Codecs.VarInt,
            static command => command.SlotIndex,
            static (buildingObjectGlobalId, slotIndex) => new InstantCompleteProductionCommand(buildingObjectGlobalId, slotIndex)
        ),
        CommandLayout.MetadataFirst
    );
}
