using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Expansion;

/// <summary>Unlocks an expansion using its outstanding material requirements.</summary>
public sealed record ExpandLandCommand(int ExpansionDataGlobalId, int SpendReason) : CodecCommand<ExpandLandCommand>, ICommandCodecProvider<ExpandLandCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ExpandLandCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.ExpansionDataGlobalId,
            Codecs.VarInt,
            static command => command.SpendReason,
            static (expansionDataGlobalId, spendReason) => new ExpandLandCommand(expansionDataGlobalId, spendReason)
        )
    );
}
