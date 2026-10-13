using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Workbench;

/// <summary>Assigns a perk material to a workbench building mastery.</summary>
/// <param name="WorkbenchObjectGlobalId">The workbench object id.</param>
/// <param name="MaterialGlobalId">The material data id.</param>
/// <param name="PerkGlobalId">The perk data id.</param>
public sealed record AssignWorkbenchPerkCommand(int WorkbenchObjectGlobalId, int MaterialGlobalId, int PerkGlobalId) : CodecCommand<AssignWorkbenchPerkCommand>, ICommandCodecProvider<AssignWorkbenchPerkCommand>
{
    /// <inheritdoc />
    public static CommandCodec<AssignWorkbenchPerkCommand> Codec { get; } = Codecs.Command(
        type: 579,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.WorkbenchObjectGlobalId,
            Codecs.VarInt,
            static command => command.MaterialGlobalId,
            Codecs.VarInt,
            static command => command.PerkGlobalId,
            static (workbenchObjectGlobalId, materialGlobalId, perkGlobalId) => new AssignWorkbenchPerkCommand(workbenchObjectGlobalId, materialGlobalId, perkGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
