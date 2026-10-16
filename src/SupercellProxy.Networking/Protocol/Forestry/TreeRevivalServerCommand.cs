using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>Reports that a helper revived a fruit tree of a home and the helper's rewards.</summary>
/// <param name="OwnerHomeId">Farm whose tree was revived.</param>
/// <param name="HelperHomeId">Farm of the player who revived it.</param>
/// <param name="PlantHelpId">The revived plant's help id (not its global id).</param>
/// <param name="HelperRewards">Goods the helper earns for the revival.</param>
public sealed record TreeRevivalServerCommand(LongId OwnerHomeId, LongId HelperHomeId, int PlantHelpId, ReadOnlyMemory<RevivalReward> HelperRewards) : CodecServerCommand<TreeRevivalServerCommand>, ICommandCodecProvider<TreeRevivalServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<TreeRevivalServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.OwnerHomeId,
            Codecs.LongId,
            static command => command.HelperHomeId,
            Codecs.VarInt,
            static command => command.PlantHelpId,
            Codecs.Memory(RevivalReward.Codec),
            static command => command.HelperRewards,
            static (ownerHomeId, helperHomeId, plantHelpId, helperRewards) => new TreeRevivalServerCommand(ownerHomeId, helperHomeId, plantHelpId, helperRewards)
        )
    );
}
