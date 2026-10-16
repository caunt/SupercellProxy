using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Workbench;

/// <summary>Assigns a perk material to a workbench building mastery.</summary>
/// <param name="WorkbenchObjectGlobalId">Game object id of the workbench.</param>
/// <param name="MaterialGlobalId">Global id of the perk material being assigned.</param>
/// <param name="PerkGlobalId">Global id of the perk the material goes into.</param>
public sealed record AssignWorkbenchPerkCommand(int WorkbenchObjectGlobalId, int MaterialGlobalId, int PerkGlobalId) : CodecCommand<AssignWorkbenchPerkCommand>, ICommandCodecProvider<AssignWorkbenchPerkCommand>
{
    /// <inheritdoc />
    public static CommandCodec<AssignWorkbenchPerkCommand> Codec { get; } = Codecs.Command(
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
