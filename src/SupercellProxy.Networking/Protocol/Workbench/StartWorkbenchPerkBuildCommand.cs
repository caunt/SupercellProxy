using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Workbench;

/// <summary>Starts building a workbench perk, paying its coin or diamond price.</summary>
/// <param name="WorkbenchObjectGlobalId">The workbench object id.</param>
/// <param name="PerkGlobalId">The perk data id.</param>
/// <param name="Price">The expected price.</param>
public sealed record StartWorkbenchPerkBuildCommand(int WorkbenchObjectGlobalId, int PerkGlobalId, int Price) : CodecCommand<StartWorkbenchPerkBuildCommand>, ICommandCodecProvider<StartWorkbenchPerkBuildCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StartWorkbenchPerkBuildCommand> Codec { get; } = Codecs.Command(
        type: 543,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.WorkbenchObjectGlobalId,
            Codecs.VarInt,
            static command => command.PerkGlobalId,
            Codecs.VarInt,
            static command => command.Price,
            static (workbenchObjectGlobalId, perkGlobalId, price) => new StartWorkbenchPerkBuildCommand(workbenchObjectGlobalId, perkGlobalId, price)
        ),
        CommandLayout.MetadataFirst
    );
}
