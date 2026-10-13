using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Workbench;

/// <summary>Selects a workbench building-mastery perk.</summary>
/// <param name="PerkGlobalId">The perk data id.</param>
public sealed record SelectWorkbenchPerkCommand(int PerkGlobalId) : CodecCommand<SelectWorkbenchPerkCommand>, ICommandCodecProvider<SelectWorkbenchPerkCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SelectWorkbenchPerkCommand> Codec { get; } = Codecs.Command(
        type: 540,
        Codecs.Composite(Codecs.VarInt, static command => command.PerkGlobalId, static (perkGlobalId) => new SelectWorkbenchPerkCommand(perkGlobalId)),
        CommandLayout.MetadataFirst
    );
}
