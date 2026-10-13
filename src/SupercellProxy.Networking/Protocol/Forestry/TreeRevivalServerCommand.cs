using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>Reports that a helper revived a fruit tree of a home and the helper's rewards.</summary>
/// <param name="OwnerHomeId">The owner home id.</param>
/// <param name="HelperHomeId">The helper home id.</param>
/// <param name="PlantObjectGlobalId">The plant object id.</param>
/// <param name="HelperRewards">The helper rewards.</param>
public sealed record TreeRevivalServerCommand(LongId OwnerHomeId, LongId HelperHomeId, int PlantObjectGlobalId, ReadOnlyMemory<RevivalReward> HelperRewards) : CodecServerCommand<TreeRevivalServerCommand>, ICommandCodecProvider<TreeRevivalServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<TreeRevivalServerCommand> Codec { get; } = Codecs.Command(
        type: 328,
        Codecs.Composite(
            Codecs.LongId,
            static command => command.OwnerHomeId,
            Codecs.LongId,
            static command => command.HelperHomeId,
            Codecs.VarInt,
            static command => command.PlantObjectGlobalId,
            Codecs.Memory(RevivalReward.Codec),
            static command => command.HelperRewards,
            static (ownerHomeId, helperHomeId, plantObjectGlobalId, helperRewards) => new TreeRevivalServerCommand(ownerHomeId, helperHomeId, plantObjectGlobalId, helperRewards)
        )
    );
}
