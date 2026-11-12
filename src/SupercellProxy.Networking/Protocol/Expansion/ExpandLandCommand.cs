using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Expansion;

/// <summary>Unlocks an expansion using its outstanding material requirements.</summary>
/// <param name="ExpansionDataGlobalId">Global id of the expansion row to unlock: a row of the farm and fishing lake expansions table, or of the train expansions table when the town is loaded.</param>
/// <param name="SpendReason">Native spend-reason code written on the wire for the materials the unlock consumes: -38 on the farm and fishing lake, -63 in the town.</param>
public sealed record ExpandLandCommand(int ExpansionDataGlobalId, int SpendReason) : CodecCommand<ExpandLandCommand>, ICommandCodecProvider<ExpandLandCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ExpandLandCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.ExpansionDataGlobalId,
        Codecs.VarInt,
        static command => command.SpendReason,
        static (expansionDataGlobalId, spendReason) => new ExpandLandCommand(expansionDataGlobalId, spendReason)));
}
